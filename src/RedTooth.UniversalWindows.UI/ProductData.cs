using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedTooth.UniversalWindows.UI
{
    public static class ProductData
    {        
        private static Dictionary<int, string> LoadProductData()
        {
            return new Dictionary<int, string>
            {
                {1,"Compact Brushless Drill Driver"},
                {3,"Compact Brushless Impact Driver"},
                {8,"FUEL G2 Impact Driver - 1/4\" Hex"},
                {12,"FUEL G2 Impact Driver - 1/4\" Hex w/Bluetooth"},
                {2,"Compact Brushless Hammer Drill"},
                {4,"FUEL G2 Drill Driver"},
                {5,"FUEL G2 Hammer Drill"},
                {6,"FUEL G2 Drill Driver w/Bluetooth"},
                {7,"FUEL G2 Hammer Drill w/Bluetooth"},
                {9,"FUEL G2 Impact Wrench - 3/8\" Friction Ring"},
                {10,"FUEL G2 Impact Wrench - 1/2\" Friction Ring "},
                {11,"FUEL G2 Impact Wrench - 1/2\" Detent Pin"},
                {13,"FUEL G2 Impact Wrench - 3/8\" FR w/Bluetooth"},
                {14,"FUEL G2 Impact Wrench - 1/2\" FR w/Bluetooth"},
                {15,"FUEL G2 Impact Wrench - 1/2\" DP w/Bluetooth"},
                {16,"6T Hydraulics - Utility Crimper"},
                {20,"6T Hydraulics - Commercial Crimper U Die"},
                {21,"6T Hydraulics - Cutter"},
                {23,"OpenLink Adaptor"},
                {24,"FUEL Super Hawg 1/2\" Chuck"},
                {25,"FUEL Super Hawg Quik-Loc"},
                {26,"FUEL Braking Grinder"},
                {36,"M18 Battery Pack 1.5Ahr CP"},
                {37,"M18 Battery Pack 2.0Ahr CP"},
                {38,"M18 Battery Pack 3.0Ahr XC"},
                {39,"M18 Battery Pack 4.0Ahr XC"},
                {40,"M18 Battery Pack 5.0Ahr XC"},
                {27,"M18 FUEL™ 18ga Brad Nailer"},
                {41,"M18 FUEL™ 16ga Straight Finish Nailer Kit"},
                {42,"M18 FUEL™ 16ga Angled Finish Nailer Kit"},
                {43,"M18 FUEL™ 15ga Finish Nailer Kit"},
                {45,"FUEL Sawzall"},
                {51,"M18 12T Utility Crimper"},
                {52,"M18 12T Commercial Crimper"},
                {18,"Compact Site Light Single Pack"},
                {28,"M18 Compact Site Light DP One-Key"},
                {46,"M18 Battery Pack 6.0Ahr XC"},
                {47,"M18 Battery Pack 9.0Ahr HD"}
            };
            
        }
        public static string LookupProductName(string MPBID)
        {
            string ProductName = "";
            var products = LoadProductData();
            var ProductId = int.Parse(MPBID.Substring(0,4),System.Globalization.NumberStyles.HexNumber);
                        
            //var ProductId = Encoding.UTF8.GetBytes(hexValue);            

            if (products.ContainsKey(ProductId))
            {
                ProductName = products[ProductId];
            }
            return ProductName;
        }
        public static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
    }
}
