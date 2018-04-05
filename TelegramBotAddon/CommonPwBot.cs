using BotLibrary;
using PWFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBotAddon
{
    public class CommonPwBot: Bot
    {
        PwClient bot;
        TelegramBotClient telebot;
        Int32[] btn_address;
        Int32 oldMoneyValue;
        Int64 chatid;
        Int32 msgCount;
        Int32 tempCount;
        String botId;

        public override void Do(object param)
        {

            if (!this.IsStart)
            {
                botId = ConfigurationManager.AppSettings[param.ToString()];
                if (String.IsNullOrEmpty(botId)) throw new Exception($"Не найден телеграм-бот {param.ToString()}");
                //запускаем бота
                telebot = new TelegramBotClient(botId);
                telebot.SetWebhookAsync("");
                //ищем id чата
                Int64.TryParse(ConfigurationManager.AppSettings["ChatId"], out chatid);
                bot = PwUtils.Pw_CLient_Search(param.ToString(), bot);
                if (bot == null) return;



                this.IsStart = !this.IsStart;
            }

            while (true)
            {
                Thread.Sleep(1000);
                tempCount++;

                bot = PwUtils.Pw_CLient_Search(param.ToString(), bot);
                if (bot == null) continue;

                btn_address = CalcMethods.CalcControlAddress(bot.Handle, "Win_QuickbarPetH", "Btn_Attack");
                //раз в две минуты проверка
                if (tempCount % 120 == 0)
                {
                    //если не изменилось количество денег, то отправляем уведомление в телеграм
                    PwUtils.CheckMoney(bot);
                    if (oldMoneyValue == bot.Money)
                        telebot.SendTextMessageAsync(chatid, $"У меня не меняется количество денег").Wait();
                    oldMoneyValue = bot.Money;

                    //обработка последних сообщений в телеграме
                    var updates = telebot.GetUpdatesAsync(msgCount).Result;
                    foreach (var update in updates) // Перебираем все обновления
                    {

                        if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
                        {
                            if (update.Message.Text == "/addme")
                            {
                                //обработка текста
                            }
                        }
                        msgCount = update.Id + 1;
                    }
                }
            }
        }
    }
}
