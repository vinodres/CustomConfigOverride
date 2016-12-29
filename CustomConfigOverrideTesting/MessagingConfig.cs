using System;
using System.Collections.Generic;
using System.Configuration;

namespace CustomConfigOverrideTesting
{
    /// <summary>
    /// Custom configuration section to define attributes for a message subscriber. 
    /// The proxy client and the data contract information is loaded from the configuration file.
    /// </summary>
    public class MessageSection : ConfigurationSection
    {
        [ConfigurationProperty("connections", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ConnectionElementCollection),
            AddItemName = "connection")]
        public ConnectionElementCollection Connections
        {
            get
            {
                return (ConnectionElementCollection)this["connections"];
            }
            set
            {
                this["connections"] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - Message connection element collection which allows for mulitiple connections to be defined
    /// </summary>
    public class ConnectionElementCollection : ConfigurationElementCollection, IEnumerable<ConnectionElement>
    {
        public ConnectionElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ConnectionElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var connectionElement = new ConnectionElement
            {
                BrokerUrl = DefaultBrokerUrl,
                LogMessagePayload = DefaultLogMessagePayload,
                Subscribers =
                {
                    LogMessagePayload = DefaultLogMessagePayload
                },
                Producers =
                {
                    LogMessagePayload = DefaultLogMessagePayload
                }
            };

            return connectionElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionElement)element).Name;
        }

        public new IEnumerator<ConnectionElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ConnectionElement;
            }
        }

        [ConfigurationProperty("defaultBrokerUrl", IsRequired = false)]
        public string DefaultBrokerUrl
        {
            get
            {
                return (string)this["defaultBrokerUrl"];
            }
            set
            {
                this["defaultBrokerUrl"] = value;
            }
        }

        /// <summary>
        /// DefaultLogMessagePayload, It will be used to log actual message payload.
        /// </summary>
        [ConfigurationProperty("defaultLogMessagePayload", DefaultValue = "false", IsRequired = false)]
        public string DefaultLogMessagePayload
        {
            get
            {
                return (string)this["defaultLogMessagePayload"];
            }
            set
            {
                this["defaultLogMessagePayload"] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - Topic element  which defines the attrributes of a message connection
    /// </summary>
    public class ConnectionElement : ConfigurationElement
    {
        [ConfigurationProperty(MessagingConstants.NAME, DefaultValue = "connection", IsRequired = false)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{};'\"|\\")]
        public string Name
        {
            get
            {
                return (string)this[MessagingConstants.NAME];
            }
            set
            {
                this[MessagingConstants.NAME] = value;
            }
        }

        [ConfigurationProperty("brokerUrl", IsRequired = false)]
        public string BrokerUrl
        {
            get
            {
                return (string)this["brokerUrl"];
            }
            set
            {
                this["brokerUrl"] = value;
            }
        }

        [ConfigurationProperty(MessagingConstants.PRODUCERS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProducerElementCollection),
            AddItemName = "producer")]
        public ProducerElementCollection Producers
        {
            get
            {
                return (ProducerElementCollection)this[MessagingConstants.PRODUCERS];
            }
            set
            {
                this[MessagingConstants.PRODUCERS] = value;
            }
        }

        [ConfigurationProperty(MessagingConstants.SUBSCRIBERS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SubscriberElementCollection),
            AddItemName = "subscriber")]
        public SubscriberElementCollection Subscribers
        {
            get
            {
                return (SubscriberElementCollection)this[MessagingConstants.SUBSCRIBERS];
            }
            set
            {
                this[MessagingConstants.SUBSCRIBERS] = value;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration section to define attributes for a message producer. 
    // </summary>
    public class ProducersSection : ConfigurationSection
    {
        [ConfigurationProperty(MessagingConstants.PRODUCERS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProducerElementCollection),
            AddItemName = "producer")]
        public ProducerElementCollection Producers
        {
            get
            {
                return (ProducerElementCollection)this[MessagingConstants.PRODUCERS];
            }
            set
            {
                this[MessagingConstants.PRODUCERS] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - Producer collection which allows for multiple producers to be configured
    /// </summary>
    public class ProducerElementCollection : ConfigurationElementCollection, IEnumerable<ProducerElement>
    {
        public ProducerElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ProducerElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var producerElement = new ProducerElement
            {
                LogMessagePayload = LogMessagePayload,
                Topics =
                {
                    LogMessagePayload = LogMessagePayload
                }
            };
            return producerElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProducerElement)element).Name;
        }

        public new IEnumerator<ProducerElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ProducerElement;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - subscriber element which defines the attributes for a subscriber
    /// </summary>
    public class ProducerElement : ConfigurationElement
    {
        [ConfigurationProperty(MessagingConstants.NAME, DefaultValue = "producer", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string Name
        {
            get
            {
                return (string)this[MessagingConstants.NAME];
            }
            set
            {
                this[MessagingConstants.NAME] = value;
            }
        }

        [ConfigurationProperty(MessagingConstants.CLIENT_ID, DefaultValue = "producerId", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string ClientId
        {
            get
            {
                return (string)this[MessagingConstants.CLIENT_ID];
            }
            set
            {
                this[MessagingConstants.CLIENT_ID] = value;
            }
        }

        [ConfigurationProperty(MessagingConstants.TOPICS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ProducerTopicElementCollection),
            AddItemName = "topic")]
        public ProducerTopicElementCollection Topics
        {
            get
            {
                return (ProducerTopicElementCollection)this[MessagingConstants.TOPICS];
            }
            set
            {
                this[MessagingConstants.TOPICS] = value;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Topic element collection - allows for mulitiple topics to be defined
    /// </summary>
    public class ProducerTopicElementCollection : ConfigurationElementCollection, IEnumerable<ProducerTopicElement>
    {
        public ProducerTopicElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ProducerTopicElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var producerTopicElement = new ProducerTopicElement
            {
                LogMessagePayload = LogMessagePayload
            };
            return producerTopicElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProducerTopicElement)element).Name;
        }

        public new IEnumerator<ProducerTopicElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as ProducerTopicElement;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// producer element - defines the attributes for a subscriber
    /// </summary>
    public class ProducerTopicElement : ConfigurationElement
    {
        [ConfigurationProperty(MessagingConstants.NAME, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string Name
        {
            get
            {
                return (string)this[MessagingConstants.NAME];
            }
            set
            {
                this[MessagingConstants.NAME] = value;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration section to define attributes for a message subscriber. 
    /// </summary>
    public class SubscribersSection : ConfigurationSection
    {
        [ConfigurationProperty(MessagingConstants.SUBSCRIBERS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SubscriberElementCollection),
            AddItemName = "subscriber")]
        public SubscriberElementCollection Subscribers
        {
            get
            {
                return (SubscriberElementCollection)this[MessagingConstants.SUBSCRIBERS];
            }
            set
            {
                this[MessagingConstants.SUBSCRIBERS] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - Subscriber collection which allows for multiple subscribers to be configured
    /// </summary>
    public class SubscriberElementCollection : ConfigurationElementCollection, IEnumerable<SubscriberElement>
    {
        public SubscriberElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as SubscriberElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var subscriberElement = new SubscriberElement
            {
                LogMessagePayload = LogMessagePayload,
                Topics =
                {
                    LogMessagePayload = LogMessagePayload
                }
            };
            return subscriberElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SubscriberElement)element).Name;
        }

        public new IEnumerator<SubscriberElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as SubscriberElement;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Custom configuration - subscriber element which defines the attributes for a subscriber
    /// </summary>
    public class SubscriberElement : ConfigurationElement
    {
        private const string RETRY_TIME = "retryTime";
        private const string NO_OF_SHARED_SUBSCRIBERS = "noOfSharedSubscribers";
        private const string MESSAGE_TYPE = "messageType";

        [ConfigurationProperty(MessagingConstants.NAME, DefaultValue = "subscriber", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string Name
        {
            get
            {
                return (string)this[MessagingConstants.NAME];
            }
            set
            {
                this[MessagingConstants.NAME] = value;
            }
        }

        /// <summary>
        /// Broker client id for the subscriber. For shared subscribers, each shared subscriber instance of the same topic must have a unique client id.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.CLIENT_ID, DefaultValue = "subscriberId", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\")]
        public string ClientId
        {
            get
            {
                return (string)this[MessagingConstants.CLIENT_ID];
            }
            set
            {
                this[MessagingConstants.CLIENT_ID] = value;
            }
        }

        /// <summary>
        /// RetryTime in seconds. Default is 300 seconds (5 min). It is used to retry failed message indefinetly with pausing specified number of seconds between each retry.
        /// </summary>
        [ConfigurationProperty(RETRY_TIME, DefaultValue = 300, IsRequired = false)]
        public int RetryTime
        {
            get
            {
                return (int)this[RETRY_TIME];
            }
            set
            {
                this[RETRY_TIME] = value;
            }
        }

        /// <summary>
        /// No of shared subscribers listining for messages on the same topic. The topic name must be prefixed with [[name]]. When this is greater than 0, shared subscribers are auto generated by appending subscriber number to the client id.
        /// </summary>
        [ConfigurationProperty(NO_OF_SHARED_SUBSCRIBERS, DefaultValue = 0, IsRequired = false)]
        public int NoOfSharedSubscribers
        {
            get
            {
                return (int)this[NO_OF_SHARED_SUBSCRIBERS];
            }
            set
            {
                this[NO_OF_SHARED_SUBSCRIBERS] = value;
            }
        }

        [ConfigurationProperty(MessagingConstants.TOPICS, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SubscriberTopicElementCollection), AddItemName = "topic")]
        public SubscriberTopicElementCollection Topics
        {
            get
            {
                return (SubscriberTopicElementCollection)this[MessagingConstants.TOPICS];
            }
            set
            {
                this[MessagingConstants.TOPICS] = value;
            }
        }

        [ConfigurationProperty(MESSAGE_TYPE, DefaultValue = "JSON", IsRequired = false)]
        public string MessageType
        {
            get
            {
                return (string)this[MESSAGE_TYPE];
            }
            set
            {
                this[MESSAGE_TYPE] = value;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Topic element collection - allows for mulitiple topics to be defined
    /// </summary>
    public class SubscriberTopicElementCollection : ConfigurationElementCollection, IEnumerable<SubscriberTopicElement>
    {
        public SubscriberTopicElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as SubscriberTopicElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            var subscriberTopicElement = new SubscriberTopicElement
            {
                LogMessagePayload = LogMessagePayload
            };
            return subscriberTopicElement;
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SubscriberTopicElement)element).Name;
        }

        public new IEnumerator<SubscriberTopicElement> GetEnumerator()
        {
            int count = base.Count;
            for (int i = 0; i < count; i++)
            {
                yield return base.BaseGet(i) as SubscriberTopicElement;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }

    /// <summary>
    /// Subscriber element - defines the attributes for a subscriber
    /// </summary>
    public class SubscriberTopicElement : ConfigurationElement
    {
        private const string NAME = "name";

        [ConfigurationProperty(NAME, IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*(){}/;'\"|\\")]
        public String Name
        {
            get
            {
                return (String)this[NAME];
            }
            set
            {
                this[NAME] = value;
            }
        }

        /// <summary>
        /// LogMessagePayload will be used to log message payload.
        /// </summary>
        [ConfigurationProperty(MessagingConstants.LOG_MESSAGE_PAYLOAD, IsRequired = false)]
        public string LogMessagePayload
        {
            get
            {
                return (string)this[MessagingConstants.LOG_MESSAGE_PAYLOAD];
            }
            set
            {
                this[MessagingConstants.LOG_MESSAGE_PAYLOAD] = value;
            }
        }
    }
}
