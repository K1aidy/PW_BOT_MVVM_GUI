using BotLibrary;
using Dapper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotAddon
{
    class LoginButton : IButton
    {
        readonly string connString = @".\PW.db";

        public async Task Do(object obj)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=" + connString + ";Version=3"))
            {
                var result = conn.Query("select * from accounts").ToList();
            }

            var client = new RestClient("https://auth.mail.ru/cgi-bin/auth?Login=pazzle2001@mail.ru&amp;agent=AG_FnRl5vTXAeYCNmU76ybQc&amp;page=http%3A%2F%2Fdl.mail.ru%2Frobots.txt");
            var request = new RestRequest(Method.GET);
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Downloader/14310 MailRuGameCenter/1431 Safari/537.36");
            request.AddCookie("GarageID", "032780778fb047d59474ddc4daf16c03");
            IRestResponse response = client.Execute(request);
        }
    }
}
