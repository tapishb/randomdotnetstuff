using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueConsoleApp
{

    class Program
    {
        public static async Task Main()
        {
            var bus = BusConfigurator.ConfigureBus();
            //var sendToUri = new Uri($"{RabbitMqConstants.RabbitMqUri}" + $"{RabbitMqConstants.TestServiceQueue}");



            //var endPoint = await bus.GetSendEndpoint(sendToUri);

            //await endPoint.Send<IMessage>(new Message()
            //{
            //    MessageId = new Guid(),
            //    Text = "hello",
            //    Description = "This is test"
            //});

            await bus.Publish<IMessage>(new Message()
            {
                MessageId = new Guid(),
                Text = "hello",
                Description = "This is test"
            });

            //var endPoint = bus.GetSendEndpoint(sendToUri);

            //await endPoint.Send(new Message()
            //{
            //    MessageId = new Guid(),
            //    Text = "hello",
            //    Description = "This is test"
            //}, new CancellationToken()); 

            //bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            //{
            //    sbc.ReceiveEndpoint("test_queue", ep =>
            //    {
            //        ep.Handler<Message>(context =>
            //        {
            //            return Console.Out.WriteLineAsync($"Received: {context.Message.Description}");
            //        });
            //    });


            //});

            //await bus.StartAsync(); // This is important!

            //await bus.Publish(new Message { Text = "Hi", Description = "This is test" });

            Console.WriteLine("Press any key to exit");
            await Task.Run(() => Console.ReadKey());

            //await bus.StopAsync();
        }
    }
}
