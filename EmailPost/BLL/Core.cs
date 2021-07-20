using EmailPost.MODEL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EmailPost.BLL
{
    public static class Core
    {
        private static MainWindow mainWindow;
        private static Config config;
        private static Thread postThread;
        public static MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }
        public static Config Config { get => config; set => config = value; }

        public static Thread PostThread { get => postThread; set => postThread = value; }
    }
}
