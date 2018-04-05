using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotLibrary
{
    public interface IButton
    {
        Task Do(Object obj);
    }
}
