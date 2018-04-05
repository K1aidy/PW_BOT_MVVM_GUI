using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWFramework
{
    public static class PwUtils
    {
        public static void CheckMoney(PwClient pw)
        {
            pw.Money = CalcMethods.ReadInt(pw.Handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+Money"));
        }

        public static PwClient Pw_CLient_Search(String name, PwClient pw)
        {
            //Задаем начало отсчета
            IntPtr hwnd = IntPtr.Zero;
            //Задаем временное хранилище запущенных клиентов
            List<PwClient> temp_coll = new List<PwClient>();
            //В бесконечном цикле перебираем все запущенные окна с классом ElementClient Window
            while (true)
            {
                //очищаем коллекцию клиентов и начинаем заполнять заново
                //получаем следующее окно с классом ElementClient Window. 
                hwnd = WinApi.FindWindowEx(IntPtr.Zero, hwnd, "ElementClient Window", null);
                //Если наткнулись на ноль - значит выходим 
                if (hwnd == IntPtr.Zero) break;
                //задаем временную ссылку на объект нашего клиента
                PwClient temp_client = new PwClient(hwnd);
                //если персонаж запущен (удалось прочесть имя), то добавляем наш объект во временное хранилище
                if (temp_client.Name == name)
                {
                    if (pw?.ProcessID != temp_client.ProcessID)
                        return temp_client;
                    break;
                }
            }
            return pw;
        }
        public static void GetCords(PwClient pw, out double x, out double y, out double z)
        {
            x = Math.Round(CalcMethods.ReadFloat(pw.Handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+X")), 1);
            y = Math.Round(CalcMethods.ReadFloat(pw.Handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+Y")), 1);
            z = Math.Round(CalcMethods.ReadFloat(pw.Handle, OfsPresenter.getInstance("BA")[0], OfsPresenter.getInstance("GA+Player+Z")), 1);
        }
    }
}
