using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace CustomConfigOverrideTesting.Tests
{
    [TestFixture]
    public class MessagingTests
    {
        [Test]
        public void Can_Override_DefaultLogMessagePayload_from_Topic_Node()
        {
            var config =
                (MessageSection)ConfigurationManager.GetSection("messageGroup/messageConfiguration");

            IEnumerable<ConnectionElement> crmConnections = config.Connections.Where(c => c.Name == "CRMConnection");
            foreach (ConnectionElement crmConnection in crmConnections)
            {
                Assert.IsTrue(string.Compare(crmConnection.BrokerUrl, "tcp://testcrm02:2400", StringComparison.OrdinalIgnoreCase) == 0);
                
                foreach (var subscriber in crmConnection.Subscribers)
                {
                    switch (subscriber.Name)
                    {
                        case "SALES_PRODUCT_X":
                            foreach (var topic in subscriber.Topics)
                            {
                                if (topic.Name == "SALES.PRODUCT.X")
                                {
                                    Assert.IsTrue(string.Compare(topic.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                                }
                                else
                                {
                                    Assert.IsTrue(string.Compare(topic.LogMessagePayload, "false", StringComparison.OrdinalIgnoreCase) == 0);
                                }
                            }
                            break;
                        default:
                            Assert.IsTrue(string.Compare(subscriber.LogMessagePayload, "false", StringComparison.OrdinalIgnoreCase) == 0);
                            break;
                    }
                }
            }
        }

        [Test]
        public void Can_Override_DefaultLogMessagePayload_from_Connection_Node()
        {
            var config = (MessageSection)ConfigurationManager.GetSection("messageGroup/messageConfiguration");

            var orderConnections = config.Connections.Where(c => c.Name == "OrdersConnection");
            foreach (var orderConnection in orderConnections)
            {
                Assert.IsTrue(string.Compare(orderConnection.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                Assert.IsTrue(string.Compare(orderConnection.Subscribers.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                foreach (var rcSubscriber in orderConnection.Subscribers)
                {
                    Assert.IsTrue(string.Compare(rcSubscriber.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                    Assert.IsTrue(string.Compare(rcSubscriber.Topics.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                    foreach (var rcTopic in rcSubscriber.Topics)
                    {
                        Assert.IsTrue(string.Compare(rcTopic.LogMessagePayload, "true", StringComparison.OrdinalIgnoreCase) == 0);
                    }
                }
            }
        }
    }
}
