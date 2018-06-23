﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

namespace cj
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Core.SqlAction.AddH(initDic(dt + qh, jh));
            InitializeComponent();
            conn.Init();


            JObject init_result = JsonConvert.DeserializeObject(zhcw()) as JObject;
            foreach (JObject j in init_result["list"])
            {
                string qh = j["issue"].ToString();
                string jh = j["winNum"].ToString();

                Core.SqlAction.AddH(initDic(qh, jh));
                //htmlRTB.AppendText("[ QH"+qh+"+  JH"+jh+"  ]插入完成");
                //MessageBox.Show(qh+":"+jh);
            }

            //htmlRTB.AppendText(zhcw());

        }

        private string tempw()
        {
            string url = "http://www.km28.com/gp_chart/jlks/0/50.html";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.AllowAutoRedirect = false;
            req.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            string html=new StreamReader (res.GetResponseStream()).ReadToEnd();
            //return html;
            //MessageBox.Show(cookies);
            return html;

        }

        private string  zhcw()
        {
            
            string url = "http://data.zhcw.com/k3/index.php?act=kstb&provinceCode=22";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.AllowAutoRedirect = false;
            req.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            string cookies = res.Headers.Get("Set-Cookie");
            //string html=new StreamReader (res.GetResponseStream()).ReadToEnd();
            //return html;
            //MessageBox.Show(cookies);
            string jsonUrl = "http://data.zhcw.com/port/client_json.php?transactionType=10120105&type=zongzs&daytime=today&timeId=-1";
            //string jsonUrl = "http://data.zhcw.com/port/client_json.php?transactionType=10120105&type=zongzs&period=2000&timeId=-1";
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(jsonUrl);
            req1.Method = "GET";
            req1.AllowAutoRedirect = false;
            req1.ContentType = "application/x-www-form-urlencoded";
            req1.CookieContainer = new CookieContainer();
            req1.CookieContainer.SetCookies(req1.RequestUri, cookies);
            HttpWebResponse res1 = (HttpWebResponse)req1.GetResponse();
            string html = new StreamReader(res1.GetResponseStream()).ReadToEnd();
            return html;
        }
        private Dictionary<string, object> initDic(string qh, string jh)
        {

            var dic = new Dictionary<string, object>();
            dic["qh"] = qh;
            dic["jh"] = jh;
            return dic;
        }

        private void dispBtn_Click(object sender, RoutedEventArgs e)
        {
            dispDG.ItemsSource = Core.SqlAction.SelectH("").DefaultView;
            dispDG.GridLinesVisibility = DataGridGridLinesVisibility.All;
        }
    }
}