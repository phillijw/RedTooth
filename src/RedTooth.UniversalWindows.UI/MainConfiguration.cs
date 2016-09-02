using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace RedTooth.UniversalWindows.UI
{
    //*********************************************************
    //
    // Copyright (c) Microsoft. All rights reserved.
    // This code is licensed under the MIT License (MIT).
    // THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
    // ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
    // IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
    // PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
    //
    //*********************************************************
    public partial class MainPage : Page
    {
        public const string FEATURE_NAME = "Bluetooth Low Energy Advertisement Demo";

        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario()
            {
                Title = "Nearby Devices",
                ClassType = typeof(NearbyDevices)
            }            
        };
    }

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
    }
}
