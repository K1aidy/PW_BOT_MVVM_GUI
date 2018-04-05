using BotLibrary;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading;
using PWFramework;

namespace TelegramBotAddon
{
    public class TelegramLogger:Bot
    {
        TelegramBotClient bot;
        Int32 msgCount;
        List<long> chats = new List<long>() { -317512838 };
        Hashtable ht = new Hashtable();
        Dictionary<string, int> oldMoneyValue = new Dictionary<string, int>();
        ObservableCollection<PwClient> pwclients = new ObservableCollection<PwClient>();
        List<String> botList = new List<string>()
        {

        };

        public override void Do(object param)
        {
            if (!this.IsStart)
            {
                //запускаем бота
                bot = new TelegramBotClient(param.ToString());
                bot.SetWebhookAsync("");
                
                this.IsStart = !this.IsStart;
            }

            while (true)
            {
                Pw_CLient_Search(pwclients);
                Thread.Sleep(120000);
                foreach (var pw in pwclients)
                {
                    if (!botList.Contains(pw.Name))
                        continue;
                    PwUtils.CheckMoney(pw);
                    if (oldMoneyValue[pw.Name] == pw.Money)
                        chats.ForEach(s => bot.SendTextMessageAsync(s, $"У {pw.Name} не меняется количество денег").Wait());
                    oldMoneyValue[pw.Name] = pw.Money;
                }
                var updates = bot.GetUpdatesAsync(msgCount).Result;

                foreach (var update in updates) // Перебираем все обновления
                {
                    //System.Windows.MessageBox.Show(update.Message.Chat.Id.ToString() + " " + update.Message.MessageId + " " + update.Id);
                    if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                    {
                        if (update.Message.Text == "/addme")
                        {
                            //обработка текста
                        }
                        //chats.ForEach(s => bot.SendTextMessageAsync(s, $"{msgCount} - {update.Message.Chat.Id} - {update.Message.Text}").Wait());
                    }
                    if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.StickerMessage)
                    {
                        chats.ForEach(s => bot.SendStickerAsync(s, new Telegram.Bot.Types.FileToSend(update.Message.Sticker.FileId)).Wait());
                    }
                    msgCount = update.Id + 1;
                }
            }
        }

        private void Pw_CLient_Search(ObservableCollection<PwClient> coll)
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
                        oldMoneyValue.Remove(((PwClient)coll[i]).Name);
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
                oldMoneyValue.Add(pw.Name, pw.Money);
            }
        }

        private Int32 SearchIndexInCollection(PwClient pw, List<PwClient> coll)
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
