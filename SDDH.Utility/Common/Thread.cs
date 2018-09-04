using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Common
{
    /// <summary>
    /// 线程、异步
    /// </summary>
    public class Thread
    {
        /// <summary>
        /// 声明委托
        /// </summary>
        public delegate void Handler1();
        public delegate void Handler2(string str);
        public delegate string Handler3();
        public delegate string Handler4(string str);
        public delegate object Handler5(string str, int num);

        /// <summary>
        /// 异步调用
        /// </summary>
        public void DoAsynAction()
        {
            Handler1 handler1 = new Handler1(AsynDemo1);
            handler1.BeginInvoke(null, null);

            Handler2 handler2 = new Handler2(AsynDemo2);
            handler2.BeginInvoke("string", null, null);

            Handler3 handler3 = new Handler3(AsynDemo3);
            //handler3.BeginInvoke(null, null);
            IAsyncResult result3 = handler3.BeginInvoke(null, null);

            Handler4 handler4 = new Handler4(AsynDemo4);
            //handler4.BeginInvoke("string", null, null);
            IAsyncResult result4 = handler4.BeginInvoke("string", null, null);

            Handler5 handler5 = new Handler5(AsynDemo5);
            //handler5.BeginInvoke("", 0, null, null);
            IAsyncResult result5 = handler5.BeginInvoke("string", 0, null, null);
        }

        public void AsynDemo1()
        {
            //Do something
        }

        public void AsynDemo2(string str)
        {
            //Do something
        }

        public string AsynDemo3()
        {
            //Do something
            return "";
        }

        public string AsynDemo4(string str)
        {
            //Do something
            return "";
        }

        public object AsynDemo5(string str, int num)
        {
            //Do something
            return new object();
        }

    }
}
