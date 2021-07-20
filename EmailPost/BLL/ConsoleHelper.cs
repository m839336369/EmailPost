using System;
using System.Collections.Generic;
using System.Text;

namespace EmailPost.BLL
{
    public class ConsoleHelper
    {
        public static void WriteLine(string msg)
        {
            Core.MainWindow.Dispatcher.Invoke(()=>
            {
                if (Core.MainWindow.Console.Text.Length >= 10000) Clear();
                DateTime dt = DateTime.Now;
                Core.MainWindow.Console.AppendText(dt + "=>" + msg + "\n");
            });
        }
        public static void Clear()
        {
            Core.MainWindow.Console.Text = "";
        }
    }
}
