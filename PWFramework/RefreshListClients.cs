using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;

namespace PWFramework_Mnogoletov
{
    public class RefreshListClients
    {
        public static Hashtable ht = new Hashtable();

        private static Timer timerHead;

        private static ObservableCollection<PwClient> coll;

        public static void CloseTimer()
        {
            timerHead.Enabled = false;
        }
        public static void SetTimer(ObservableCollection<PwClient> coll_out)
        {
            coll = coll_out;
            if (timerHead == null)
                timerHead = new System.Windows.Forms.Timer();
            timerHead.Enabled = true;
            timerHead.Interval = 2000;
            timerHead.Tick += new EventHandler(updateTimerHead_Tick);
        }

        private static void updateTimerHead_Tick(object sender, EventArgs e)
        {
            Pw_CLient_Search();
        }

        private static void Pw_CLient_Search()
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
                if (temp_client.Name.Length > 0)
                    temp_coll.Add(temp_client);
            }
            //цикл для удаления из комбобокса и хэштаблицы неактуальных объектов 
            for (int i = coll.Count - 1; i > -1; i--)
            {
                if (!temp_coll.Contains(coll[i]))
                {
                    {
                        ht.Remove(((PwClient)coll[i]).Name);
                        coll.Remove((PwClient)coll[i]);
                    }
                }
            }

            //цикл для удаления из временного хранилища уже запущенных клиентов
            for (int i = coll.Count - 1; i > -1; i--)
            {
                //если временное хранилище имеет элемент, которое уже есть в комбобоксе
                //то ищем индекс этого объекта во временном хранилище и удаляем по индексу
                PwClient temp = (PwClient)coll[i];
                if (temp_coll.Contains(temp))
                {
                    Int32 iter = SearchIndexInCollection((PwClient)coll[i], temp_coll);
                    if (iter > -1)
                    {
                        temp_coll.RemoveAt(iter);
                    }

                }
            }
            
            
            //добавляем оставшиеся во временном хранилище объекты (новые)
            //в комбобокс и хэш-таблицу
            foreach (PwClient pw in temp_coll)
            {
                coll.Add(pw);
                ht.Add(pw.Name, pw);
            }
        }

        private static Int32 SearchIndexInCollection(PwClient pw, List<PwClient> coll)
        {
            //пробегаемся по коллекции и ищем индекс нужного элемента
            for (Int32 i = 0; i < coll.Count; i++)
            {
                if (pw == coll[i])
                    return i;
            }
            return -1;
        }
    }
}
