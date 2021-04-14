using MassTransit;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QueueConsoleApp
{
    public static class BusConfigurator
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator> registrationAction = null)
        {                       
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(RabbitMqConstants.RabbitMqUri, hst => 
                {
                    hst.Username(RabbitMqConstants.UserName);
                    hst.Password(RabbitMqConstants.Password);
                });

                registrationAction?.Invoke(cfg);
                //cfg.Durable = false;
            });
            
        }
    }
}

