using System;

namespace QueueConsoleApp
{
    interface IMessage
    {
        Guid MessageId { get; set; }
        string Description { get; set; }
        string Text { get; set; }
        
    }
}