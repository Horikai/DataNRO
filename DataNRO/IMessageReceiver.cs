using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataNRO
{
    internal interface IMessageReceiver
    {
        void OnMessageReceived(MessageReceive message);
    }
}
