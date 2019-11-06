using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SDDH.Test.UtilityTest
{
    [TestClass]
    public class DemoTest
    {
        [TestMethod]
        public void DynamicObjectTest()
        {
            List<object> objList = new List<object>();
            for (int i = 0; i < 5; i++)
            {
                var parent = new { id = i, pId = 0, name = "Name-" + i, open = true };
                objList.Add(parent);
                for (int j = 0; j < 3; j++)
                {
                    var child = new { id = "__" + j, pId = i, name = "Name-" + i + "-" + j, open = false };
                    objList.Add(child);
                }
            }

            foreach (var obj in objList)
            {
                var id = obj.GetType().GetProperty("id").GetValue(obj, null).ToString();
                var trimId = obj.GetType().GetProperty("id").GetValue(obj, null).ToString().TrimStart('_');
                var name = obj.GetType().GetProperty("name").GetValue(obj).ToString();
                var name2 = obj.GetType().GetProperty("name").GetValue(obj, null).ToString();

            }


        }

        [TestMethod]
        public void EnumToListTest()
        {
            var enumList = Enum.GetValues(typeof(DemoEnum));
            foreach (var e in enumList)
            {
                var test = e;
            }

        }

        [TestMethod]
        public void RepalceTest()
        {
            var oldStr = "ORICO 2577U3 2.5\" USB 3.0 SATA HDD Box HDD Hard Disk Drive External HDD Enclosure Black Case";
            var newStr = oldStr.Replace('"', '\"');
            var newStr2 = oldStr.Replace("\"", "\\\"");
        }


    }

    public enum DemoEnum
    {
        First,
        Second,
        Third
    }
}
