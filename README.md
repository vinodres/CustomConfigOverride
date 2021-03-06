# CustomConfigOverride
Custom Config Override

I am trying to streamline custom configuration settings in a windows service project with default values and overriding capability by child nodes. Here is how custom configuration section looks like.

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
      <configSections>
        <sectionGroup name="messageGroup">
          <section name="messageConfiguration" type="CustomConfigOverrideTesting.MessageSection, CustomConfigOverrideTesting" allowLocation="true" allowDefinition="Everywhere"/>
        </sectionGroup>
      </configSections>
      <messageGroup>
        <messageConfiguration>
          <connections defaultLogMessagePayload="false" defaultBrokerUrl="tcp://test01:2300">
            <connection name="CrmConnection" brokerUrl="tcp://testcrm02:2400">
              <producers>
                <producer name="CRM_SALES" clientId="CRM_SALES">
                  <topics>
                    <topic name="SALES.PRODUCT.X"/>
                    <topic name="SALES.PRODUCT.Y"/>
                  </topics>
                </producer>
              </producers>
              <subscribers>
                <subscriber name="SALES_PRODUCT_X" clientId="SALES_PRODUCT_X" retryTime="300" messageType="XML" noOfSharedSubscribers="4">
                  <topics>
                    <topic name="SALES.PRODUCT.X" logMessagePayload="true"/>
                  </topics>
                </subscriber>
                <subscriber name="SALES_PRODUCT_Y" clientId="SALES_PRODUCT_Y" retryTime="300" messageType="XML" noOfSharedSubscribers="4">
                  <topics>
                    <topic name="SALES.PRODUCT.Y"/>
                  </topics>
                </subscriber>
              </subscribers>
            </connection>
            <connection name="OrdersConnection" logMessagePayload="true">
              <producers>
                <producer name="ORDER_SYSTEM" clientId="ORDER_SYSTEM">
                  <topics>
                    <topic name="ORDER.UPDATE" />
                  </topics>
                </producer>
              </producers>
              <subscribers>
                <subscriber name="CRM_SALES" clientId="CRM_SALES">
                  <topics>
                    <topic name="ORDER.UPDATE"/>
                  </topics>
                </subscriber>
              </subscribers>
            </connection>
          </connections>
        </messageConfiguration>
      </messageGroup>
    </configuration>

At the root connections configuration node, I have a default value set to false

> connections defaultLogMessagePayload="false"

I am able to override it at subscriber leaf Topic node with LogMessagePayload attribute by doing something like this

    <topic name="SALES.PRODUCT.X" LogMessagePayload="true"/>

But when I try to override using LogMessagePayload at an intermediate parent such as connection name="OrdersConnection" by doing this 

    <connection name="OrdersConnection" LogMessagePayload="true">

The LogMessagePayload value true does not propagate to its decedents. 

I am trying to figure out what is the best way to achieve it?

Please take a look at MessagingTests.cs for unit tests. Can_Override_DefaultLogMessagePayload_from_Connection_Node() tests demonstrates failing use case.
