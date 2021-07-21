using EmailPost.MODEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EmailPost.BLL
{
    public class ConfigHelper
    {
        public static void Init()
        {
            //获取或设置当前工作目录的完全限定路径。
            string sPath = Environment.CurrentDirectory;
            //将两个字符串合并到一个路径中。
            sPath = Path.Combine(Environment.CurrentDirectory, "Config.json");
            //判断文件夹是否存在，如果不存在就创建file文件夹
            if (!File.Exists(sPath))
            {
                Core.Config = new Config();
                Core.Config.Token = "";
                Core.Config.URL = "";
                Core.Config.MinTime = 8;
                Core.Config.MaxTime = 10;
                Save();
            }
            else
            {
                string json = File.ReadAllText(sPath);
                Core.Config = JsonConvert.DeserializeObject<Config>(json);
            }
        }
        public static void Save()
        {
            string json = JsonConvert.SerializeObject(Core.Config);
            //获取或设置当前工作目录的完全限定路径。
            string sPath = Environment.CurrentDirectory;
            //将两个字符串合并到一个路径中。
            sPath = Path.Combine(Environment.CurrentDirectory, "Config.json");
            try
            {
                File.WriteAllText(sPath, json);
            }
            catch(Exception e)
            {
                //ConsoleHelper.WriteLine("程序权限不足，下次启动请以管理员身份启动");
            }
        }
    }
}
