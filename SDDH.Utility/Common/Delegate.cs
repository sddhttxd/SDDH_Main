using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Utility.Common
{
    /* 委托
     * 1.delegate：0-32个参数，可有可无返回值
     * 2.Action泛型委托：0-16个参数，没有返回值
     * 3.Func泛型委托：0-16个参数，必须有返回值
     * 4.Predicate泛型委托：1个参数，bool类型返回值
    */
    public class Delegate
    {
        #region Delegate
        private delegate void DemoHandler();
        private delegate void DemoHandler1(int param1);
        private delegate int DemoHandler2(int param1, int param2);

        public void DelegateDemo()
        {
            DemoHandler demo = new DemoHandler(Function);
            demo();
            demo.BeginInvoke(null, null);

            int p1 = 1;
            int p2 = 2;
            DemoHandler1 demo1 = new DemoHandler1(Function1);
            demo1(p1);
            demo1.BeginInvoke(p1, null, null);

            DemoHandler2 demo2 = new DemoHandler2(Function2);
            int result1 = demo2(p1, p2);
            var result2 = demo2.BeginInvoke(p1, p2, null, null);


        }
        #endregion  

        public void DoHandler()
        {
            ActionHandler(Function);
            ActionHandler<int>(Function1, 1);
            ActionHandler<int>(p => { var result = p; }, 1);//lambda
            ActionHandler<string>(Function3, "hello world");
            ActionHandler<string>(p => { var result = p; }, "hello world");
            ActionHandler<string, int>(Function5, "visit times:", 5);
            ActionHandler<string, int>((p1, p2) => { var result = p1 + p2.ToString(); }, "visit times:", 5);

            var r1 = FuncHandler(Function6);
            r1 = FuncHandler(() => { return "hello world"; });
            var r2 = FuncHandler<string>(Function1, "hello world");
            r2 = FuncHandler<string>(p => { return p; }, "hello world");
            var r3 = FuncHandler<int, int>(Function2, 2, 3);
            r3 = FuncHandler<int, int>((p1, p2) => { return p1 + p2; }, 2, 3);
            var r4 = FuncHandlerStr<string, string>(Function4, "abc", "b");
            r4 = FuncHandlerStr<string, string>((p1, p2) => { return p1.Contains(p2); }, "abc", "b");

            var r5 = PredicateHandler<int>(Function7, 5);
            r5 = PredicateHandler<int>(p => { return p == 5; }, 5);
        }

        #region Action
        /// <summary>
        /// 无参数Action
        /// </summary>
        /// <param name="action"></param>
        public void ActionHandler(Action action)
        {
            action();
        }

        /// <summary>
        /// 一个参数Action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="param"></param>
        public void ActionHandler<T>(Action<T> action, T param)
        {
            action(param);
        }

        /// <summary>
        /// 两个参数Action
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="action"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void ActionHandler<T1, T2>(Action<T1, T2> action, T1 p1, T2 p2)
        {
            action(p1, p2);
        }
        #endregion

        #region Func
        /// <summary>
        /// 无参数返回字符串Func
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public string FuncHandler(Func<string> func)
        {
            return func();
        }

        /// <summary>
        /// 一个参数返回字符串Func
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public string FuncHandler<T>(Func<T, string> func, T param)
        {
            return func(param);
        }

        /// <summary>
        /// 两个参数返回整型Func
        /// </summary>
        /// <typeparam name="T1">泛型参数1</typeparam>
        /// <typeparam name="T2">泛型参数2</typeparam>
        /// <param name="func">实现方法</param>
        /// <param name="p1">参数1</param>
        /// <param name="p2">参数2</param>
        /// <returns></returns>
        public int FuncHandler<T1, T2>(Func<T1, T2, int> func, T1 p1, T2 p2)
        {
            return func(p1, p2);
        }

        /// <summary>
        /// 两个参数返回布尔Func
        /// </summary>
        /// <typeparam name="T1">泛型参数1</typeparam>
        /// <typeparam name="T2">泛型参数2</typeparam>
        /// <param name="func">实现方法</param>
        /// <param name="p1">参数1</param>
        /// <param name="p2">参数2</param>
        /// <returns></returns>
        public bool FuncHandlerStr<T1, T2>(Func<T1, T2, bool> func, T1 p1, T2 p2)
        {
            return func(p1, p2);
        }
        #endregion

        #region Predicate
        /// <summary>
        /// 只能一个参数返回布尔Predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public bool PredicateHandler<T>(Predicate<T> predicate, T param)
        {
            return predicate(param);
        }
        #endregion

        #region Function demos
        private void Function()
        {
            //do something
        }

        private string Function6()
        {
            return "hello world";
        }

        private void Function1(int p)
        {
            var result = p;
        }
        private bool Function7(int p)
        {
            return (p == 5);
        }

        private string Function1(string p)
        {
            return p;
        }

        private int Function2(int p1, int p2)
        {
            return p1 + p2;
        }

        private void Function3(string p)
        {
            var result = p;
        }

        private bool Function4(string p1, string p2)
        {
            return p1.Contains(p2);
        }

        private void Function5(string p1, int p2)
        {
            var result = p1 + p2.ToString();
        }
        #endregion
    }
}
