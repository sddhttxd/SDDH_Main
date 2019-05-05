using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Test
{
    [TestClass]
    public class MathTest
    {

        [TestMethod]
        public void SearchTest()
        {
            int num = 8;
            int[] arr = { 0, 1, 2, 4, 5, 6, 7, 8, 9, 10 };
            int index = arr.Length / 2;
            int result = SearchDemo(arr, num, index);
        }

        private int SearchDemo(int[] arr, int num, int index)
        {
            int result = -1;
            try
            {
                if (num < arr[0] || num > arr[arr.Length - 1])
                {
                    return result;
                }
                else
                {
                    if (num < arr[index])
                    {
                        index = index / 2;
                        return SearchDemo(arr, num, index);
                    }
                    else if (num > arr[index])
                    {
                        index += (arr.Length - index) / 2;
                        return SearchDemo(arr, num, index);
                    }
                    else if (num == arr[index])
                    {
                        result = index;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

    }
}
