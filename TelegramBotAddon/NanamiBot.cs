using BotLibrary;
using PWFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Telegram.Bot;

namespace TelegramBotAddon
{
    public class NanamiBot: Bot
    {
        PwClient bot;
        Int32[] btn_address;
        Int32 oldMoneyValue;
        Int64 chatid;
        Int32 msgCount;
        Int32 tempCount;
        String botId;
        Double x, y, z;

        public override void Do(object param)
        {

            if (!this.IsStart)
            {
                bot = PwUtils.Pw_CLient_Search(param.ToString(), bot);
                if (bot == null) return;
                PwUtils.GetCords(this.bot, out this.x, out this.y, out this.z);

                this.IsStart = !this.IsStart;
            }

            while (true)
            {
                Thread.Sleep(1000);
                tempCount++;

                bot = PwUtils.Pw_CLient_Search(param.ToString(), bot);
                if (bot == null) continue;
                //поиск адреса контрола
                btn_address = CalcMethods.CalcControlAddress(bot.Handle, "Win_QuickbarPetH", "Btn_Attack");
                //раз в 10 секунд проверяем бота на "стояние"
                if (tempCount % 10 == 0)
                {
                    double x_new, y_new, z_new;
                    //чекам координаты
                    PwUtils.GetCords(this.bot, out x_new, out y_new, out z_new);
                    if (x_new == this.x && y_new == this.y && z_new == this.z)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            //Посылаем F12 4 раза
                            WinApi.PostMessage(bot.Descrypt, WinApi.WM_KEYDOWN, (int)Keys.F12, 0);
                            Thread.Sleep(400);
                        }
                    }
                    this.x = x_new; this.y = y_new; this.z = z_new;
                }

                //раз в две минуты проверка
                if (tempCount % 120 == 0)
                {
                    //если не изменилось количество денег, то отправляем уведомление в телеграм
                    PwUtils.CheckMoney(bot);
                    oldMoneyValue = bot.Money;
                }
                //жмем атаку
                var visible = CalcMethods.CalcByteValue(bot.Handle, btn_address[0] + OfsPresenter.getInstance("WND_VIS")[0]);
                if (visible == 1)
                {
                    Injects.GUI_Inject(btn_address[0], btn_address[1], bot.Handle);
                }
            }
        }
    }
}
