using EmailPost.BLL;
using EmailPost.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmailPost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isRunning = false;
        public MainWindow()
        {
            InitializeComponent();
            Core.MainWindow = this;
            ConfigHelper.Init();
            ConfigPanel.DataContext = Core.Config;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void Control_Click(object sender, RoutedEventArgs e)
        {
            if (URL.Text.Equals(""))
            {
                ConsoleHelper.WriteLine("请输入接口地址");
                return;
            }
            else if(Core.PostThread == null)
            {
                ConsoleHelper.WriteLine("请导入数据");
                return;
            }
            else if (isRunning == false)
            {
                ControlButton.Content = "停止发送";
                Core.PostThread.Start();
                isRunning = true;
                ConsoleHelper.WriteLine("开始Post....");
            }
            else
            {
                isRunning = false;
                ControlButton.Content = "开始发送";
            }
        }
        private void Posting(DataTable dt)
        {
            int cnt = 0;
            Random random = new Random();
            foreach (DataRow row in dt.Rows)
            {
                if(isRunning == false)
                {
                    ConsoleHelper.WriteLine($"Post已停止");
                    break;
                }
                if (row.Field<string>("状态") == null)
                {
                    PostData data = new PostData();
                    data.Token = Core.Config.Token;
                    data.To = row.Field<string>("mail");
                    data.dynamicParams = new List<NameValue>();
                    foreach (DataColumn column in dt.Columns)
                    {
                        data.dynamicParams.Add(new NameValue(column.ColumnName, row.Field<string>(column.ColumnName)));
                    }
                    string a = Post.EmailPost(Core.Config.URL, data);
                    row.SetField("状态", "√");
                    ++cnt;
                    Thread.Sleep(random.Next(Core.Config.MinTime*1000, Core.Config.MaxTime*1000));
                }
            }
            Core.MainWindow.Dispatcher.Invoke(() =>
            {
                ControlButton.Content = "开始发送";
                Core.PostThread = null;
                isRunning = false;
                ConsoleHelper.WriteLine($"累计成功发送{cnt}位");
            });
        }

        private void DataListView_Drop(object sender, DragEventArgs e)
        {
            if (Core.PostThread != null)
            {
                if(Core.PostThread.ThreadState == ThreadState.Running) isRunning = false;
                ConsoleHelper.WriteLine("发现导入新数据,旧数据请求已经终止。");
            }
            GridView gv = new GridView();
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString(); //获得路径
            if (path != null)
            {
                DataTable dt = LoadData(path);
                foreach(DataColumn column in dt.Columns)
                {
                    GridViewColumn gvc = new GridViewColumn();
                    gvc.Header = column.ColumnName;
                    gvc.DisplayMemberBinding = new Binding(column.ColumnName);
                    gv.Columns.Add(gvc);
                }
                DataListView.View = gv;
                DataListView.ItemsSource = dt.DefaultView;
                DataListView.DataContext = dt;
                DragLabel.Visibility = Visibility.Hidden;
                TipLabel.Visibility = Visibility.Hidden;
                Core.PostThread = new Thread(() => {
                    Posting(dt);
                });
            }
            else ConsoleHelper.WriteLine("未找到文件路径！");

        }
        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable LoadData(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                ConsoleHelper.WriteLine(strLine);
                strLine = Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("gb2312"), Encoding.UTF8, Encoding.GetEncoding("gb2312").GetBytes(strLine)));
                //strLine = Common.ConvertStringUTF8(strLine);
                ConsoleHelper.WriteLine(strLine);
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                    dt.Columns.Add("状态");
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ConfigHelper.Save();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "http://www.nezhau.com");
        }
    }
}
