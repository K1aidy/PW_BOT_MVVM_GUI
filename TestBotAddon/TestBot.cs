using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotLibrary;
using System.Diagnostics;
using System.Threading;

namespace TestBotAddon
{
    public class TestBot : Bot
    {

        //public override Task Do(object param)
        //{
        //    //if (!this.Checked) return new Task(() => { return; });
        //    return new Task(async () => {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            await Task.Delay(1000);
        //            Debug.WriteLine($"Выполнено действие над {param} - {i}я итерация");
        //        }
        //    });
        //}
        public override void Do(object param)
        {
            int i = 0;
            while (this.Check(param))
            {
                Thread.Sleep(1000);
                Debug.WriteLine($"Выполнено действие над {param} - {i}я итерация");
                i++;
            }
        }
        public override bool Check(object param)
        {
            return base.Check(param);
        }
    }
}
