using BotLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBotAddon
{
    public class TestButton : IButton
    {

        public Task Do(object obj)
        {
            return new Task(async () => {
                for (int i = 0; i < 10; i++)
                {
                    await Task.Delay(1000);
                    Debug.WriteLine($"Выполнено действие над {obj.ToString()} - {i}я итерация");
                }
            });
        }
    }
}
