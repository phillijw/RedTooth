﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedTooth.Core.Model;



namespace MKBLE
{
    //All code from https://github.com/jrowberg/bglib/tree/master/MSVCSharp/Examples
    public class ConnectionManager
    {
        public Bluegiga.BGLib bglib = new Bluegiga.BGLib();
        System.IO.Ports.SerialPort serialAPI = new System.IO.Ports.SerialPort();
        Dictionary<String, Tool> localTools = new Dictionary<String, Tool>(); 
        //
        //Only one connection for now. Will update later
        public Byte connection_handle = 0;              // connection handle (will always be 0 if only one connection happens at a time)
        private String logLocation = "c:\\temp\\hackout\\blelog.txt";
        public void OpenConnection()
        {
            var comPort = System.IO.Ports.SerialPort.GetPortNames().Single();

            serialAPI.Handshake = System.IO.Ports.Handshake.RequestToSend;
            serialAPI.BaudRate = 115200;
            serialAPI.PortName = comPort;
            serialAPI.DataBits = 8;
            serialAPI.StopBits = System.IO.Ports.StopBits.One;
            serialAPI.Parity = System.IO.Ports.Parity.None;
            serialAPI.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(DataReceivedHandler);
            serialAPI.Open();
            Console.WriteLine(string.Format("Port {0} (open)", serialAPI.PortName));

            bglib.BLEEventSystemBoot += new Bluegiga.BLE.Events.System.BootEventHandler(this.SystemBootEvent);
            bglib.BLEEventGAPScanResponse += new Bluegiga.BLE.Events.GAP.ScanResponseEventHandler(this.GAPScanResponseEvent);
            bglib.BLEEventConnectionStatus += new Bluegiga.BLE.Events.Connection.StatusEventHandler(this.ConnectionStatusEvent);
            Console.WriteLine("Events Set");

        }

        public void Scan()
        {
            bglib.SendCommand(serialAPI, bglib.BLECommandGAPDiscover(1));
        }

        public void StopScan()
        {
            bglib.SendCommand(serialAPI, bglib.BLECommandGAPEndProcedure());
        }

        private void DataReceivedHandler(
                                object sender,
                                System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            var bytesToRead = sp.BytesToRead;
            var inData = new Byte[bytesToRead];

            // read all available bytes from serial port in one chunk
            var bytesRead = sp.Read(inData, 0, bytesToRead);

            // DEBUG: display bytes read
            //Console.WriteLine("<= RX ({0}) [ {1}]", inData.Length, ByteArrayToHexString(inData));

            // parse all bytes read through BGLib parser
            for (int i = 0; i < inData.Length; i++)
            {
                bglib.Parse(inData[i]);
            }
        }

        public void SystemBootEvent(object sender, Bluegiga.BLE.Events.System.BootEventArgs e)
        {
            String log = String.Format("ble_evt_system_boot:" + Environment.NewLine + "\tmajor={0}, minor={1}, patch={2}, build={3}, ll_version={4}, protocol_version={5}, hw={6}" + Environment.NewLine,
                e.major,
                e.minor,
                e.patch,
                e.build,
                e.ll_version,
                e.protocol_version,
                e.hw
                );
            Console.Write(log);
            Debug.Print(log);
        }

        public void GAPScanResponseEvent(object sender, Bluegiga.BLE.Events.GAP.ScanResponseEventArgs e)
        {
            String log = String.Format("ble_evt_gap_scan_response:" + Environment.NewLine + "\trssi={0}, packet_type={1}, bd_addr=[ {2}], address_type={3}, bond={4}, data=[ {5}]" + Environment.NewLine,
                (SByte)e.rssi,
                (SByte)e.packet_type,
                ByteArrayToHexString(e.sender),
                (SByte)e.address_type,
                (SByte)e.bond,
                ByteArrayToHexString(e.data)
                );
            List<Byte[]> services = AdvertisedServices(e.data);
            File.AppendAllText(logLocation, "Services Start" + Environment.NewLine);
            foreach (var service in services)
            {
                //Console.WriteLine(ByteArrayToHexString(service));
                File.AppendAllText(logLocation, ByteArrayToHexString(service) + Environment.NewLine);
            }
            File.AppendAllText(logLocation, "Services End " + Environment.NewLine);
            //Console.WriteLine("Services End");
            if (isMT(services))
            {
                String mpbid;
                mpbid = ParseMPBID(e.data);
                if (!localTools.ContainsKey(mpbid))
                {
                    Tool t = new Tool();
                    t.BluetoothAddress = e.sender;
                    t.MPBID = mpbid;
                    t.AddressType = e.address_type;
                    t.RSSI = e.rssi;
                    t.ID = localTools.Count() + 1;
                    localTools.Add(mpbid, t);
                    Console.WriteLine("Found: " + mpbid + " : " + localTools.Count.ToString());
                    if (mpbid == "0007000D36")
                    {
                        Console.WriteLine("Found 5698-----------------------------------");
                        //Connect
                        ConnectToTool(t);
                        
                    }
                        
                }
                
            }
            //Console.Write(log);
            Debug.Print(log);
        }

        public void ConnectionStatusEvent(object sender, Bluegiga.BLE.Events.Connection.StatusEventArgs e)
        {
            String log = String.Format("ble_evt_connection_status: connection={0}, flags={1}, address=[ {2}], address_type={3}, conn_interval={4}, timeout={5}, latency={6}, bonding={7}" + Environment.NewLine,
                e.connection,
                e.flags,
                ByteArrayToHexString(e.address),
                e.address_type,
                e.conn_interval,
                e.timeout,
                e.latency,
                e.bonding
                );
            Console.Write(log);
            

            if ((e.flags & 0x05) == 0x05)
            {
                // connected, now perform service discovery
                connection_handle = e.connection;
                Console.WriteLine(String.Format("Connected to {0}", ByteArrayToHexString(e.address)) + Environment.NewLine);
                Byte[] cmd = bglib.BLECommandATTClientReadByGroupType(e.connection, 0x0001, 0xFFFF, new Byte[] { 0x00, 0x28 }); // "service" UUID is 0x2800 (little-endian for UUID uint8array)
                // DEBUG: display bytes written
                Console.WriteLine(String.Format("=> TX ({0}) [ {1}]", cmd.Length, ByteArrayToHexString(cmd)) + Environment.NewLine);
                bglib.SendCommand(serialAPI, cmd);
                //while (bglib.IsBusy()) ;

                // update state
                //app_state = STATE_FINDING_SERVICES;
            }
        }

        private void ConnectToTool(Tool t)
        {
            Byte[] cmd = bglib.BLECommandGAPConnectDirect(t.BluetoothAddress, t.AddressType, 0x20, 0x30, 0x100, 0);
            bglib.SendCommand(serialAPI, cmd);
            t.BLEState = BluetoothState.STATE_CONNECTING;

        }

        private bool isMT(List<Byte[]> services)
        {
            if (services.Any(a => a.SequenceEqual(new Byte[] {0xa3, 0xe6, 0x8a, 0x83, 0x4c, 0x4d, 0x47, 0x78, 0xbd, 0x0a, 0x82, 0x9f, 0xb4, 0x34, 0xa7, 0xa1})))
            {
                //Console.WriteLine("Found MKE tool");
                return true;
            }
            return false;
        }

        
        private List<Byte[]> AdvertisedServices(byte[] data)
        {
            // pull all advertised service info from ad packet
            List<Byte[]> ad_services = new List<Byte[]>();
            Byte[] this_field = { };
            int bytes_left = 0;
            int field_offset = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (bytes_left == 0)
                {
                    bytes_left = data[i];
                    this_field = new Byte[data[i]];
                    field_offset = i + 1;
                }
                else
                {
                    this_field[i - field_offset] = data[i];
                    bytes_left--;
                    if (bytes_left == 0)
                    {
                        if (this_field[0] == 0x02 || this_field[0] == 0x03)
                        {
                            // partial or complete list of 16-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(2).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x04 || this_field[0] == 0x05)
                        {
                            // partial or complete list of 32-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(4).Reverse().ToArray());
                        }
                        else if (this_field[0] == 0x06 || this_field[0] == 0x07)
                        {
                            // partial or complete list of 128-bit UUIDs
                            ad_services.Add(this_field.Skip(1).Take(16).Reverse().ToArray());
                        }
                    }
                }
            }

            return ad_services;
        }

        private String ParseMPBID(Byte[] data)
        {
            Byte[] mpbidBytes;
            mpbidBytes = data.Reverse().Skip(1).Take(5).Reverse().ToArray();
            return ByteArrayToHexString(mpbidBytes).Replace(" ", "").ToUpper();
        }

        public string ByteArrayToHexString(Byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString();
        }
    }
}
