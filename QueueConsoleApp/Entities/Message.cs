using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueConsoleApp
{
    class Message : IMessage
    {
        public Guid MessageId { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}
