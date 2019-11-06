using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDDH.Demo
{
    public class AutoMapperDemo
    {
        public static void SetMapping()
        {
            ////初始化AutoMapper
            //Mapper.Initialize(config => { });
            ////源数据对象
            //var source = new Source
            //{
            //    Id = 1,
            //    Name = "张三"
            //};
            ////映射
            //var target = Mapper.Map<Source, Target>(source);
            //Console.WriteLine(target.Id);
            //Console.WriteLine(target.Name);
        }
    }


    public class Source
    {
    }

    public class Target
    {
    }

}
