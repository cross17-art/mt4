using SwapControl.Structure;
using SwapControl.Structure.Configs;
using SwapControl.Structure.Configs.Factories;
using SwapControl.Structure.Configs.Interfaces;
using SwapControl.Structure.Configs.Products;
using System.Xml;

namespace SwapControl.Tests
{
    public class ConfigFactoryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCreateMTSettings()
        {
            ConfigFactory configFactory = new MTSettingsFactory();
            
            Config mtConfig = configFactory.CreateConfig();

            Assert.IsInstanceOf<MTSettings>(mtConfig);
        }

        [Test]
        public void TestMTSettingsInitFields()
        {
            ConfigFactory configFactory = new MTSettingsFactory();
            Config mtConfig = configFactory.CreateConfig();

            string xmlString = "<Server>" +
                                "<Address>10.248.0.43:443</Address>" +
                                "<Login>60</Login>" +
                                "<Password Secured=\"False\">1234</Password>" +
                                "<syncSwapTime>" +
                                  "<MON>" +
                                    "<time>00:00</time>" +
                                    "<time>10:15</time>" +
                                    "<time>19:30</time>" +
                                  "</MON>" +
                                  "<TUE>" +
                                    "<time>00:01</time>" +
                                  "</TUE>" +
                                  "<WED>" +
                                    "<time>00:34</time>" +
                                  "</WED>" +
                                  "<THU>" +
                                    "<time>00:39</time>" +
                                    "<time>23:59</time>" +
                                  "</THU>" +
                                  "<FRI>" +
                                    "<time>23:00</time>" +
                                  "</FRI>" +
                                  "<SAT>" +
                                    "<time>11:00</time>" +
                                  "</SAT>" +
                                  "<SUN>" +
                                    "<time>14:23</time>" +
                                  "</SUN>" +
                                "</syncSwapTime>" +
                                "</Server>";


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);


            mtConfig.InitFields(xmlDoc.DocumentElement);


            XmlNode securedNode = xmlDoc.DocumentElement["Password"].GetAttributeNode("Secured");
            string passwordValue = xmlDoc.DocumentElement["Password"].InnerText;
            MTSettings mTSettings2 = (MTSettings)mtConfig;

            Assert.AreEqual("True", securedNode.Value, "Secured attribute should be set to 'True'");
            Assert.AreNotEqual("1234", passwordValue, "Password should be encrypted");

            Assert.AreNotEqual("", mTSettings2.address, "The address field should not be empty");
            Assert.AreNotEqual(null, mTSettings2.login, "The login field should not be empty");

        }


        [Test]
        public void TestMTSettingsCorrectnessCreateSwapTime()
        {

            string xmlString = "<syncSwapTime>" +
                                  "<MON>" +
                                    "<time>00:00</time>" +
                                    "<time>10:15</time>" +
                                    "<time>19:30</time>" +
                                  "</MON>" +
                                  "<time>00:01</time>" +
                                  "<time>03:02</time>"+
                                "</syncSwapTime>";


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            MTSettings mTSettings2 = new MTSettings();
            mTSettings2.CreateSwapTimeList(xmlDoc["syncSwapTime"]);

            Assert.AreEqual("0 1 0 ? * *", mTSettings2.swapTimes[0].cronListStr[0], "The cron expressions must be the same");
            Assert.AreEqual("0 2 3 ? * *", mTSettings2.swapTimes[1].cronListStr[0], "The cron expressions must be the same");
            Assert.AreEqual(2, mTSettings2.GetArrayOfSwapTimes().Count, "The number of crowns of expressions (startup time) should be equal to 10");

        }



        [Test]
        public void TestCreateSQLSettings()
        {
            ConfigFactory configFactory = new SQLSettingsFactory();

            Config sqlConfig = configFactory.CreateConfig();

            Assert.IsInstanceOf<SQLSettings>(sqlConfig);
        }

        [Test]
        public void TestSQLSettingsInitFields()
        {
            ConfigFactory configFactory = new SQLSettingsFactory();
            Config sqlConfig = configFactory.CreateConfig();

            string xmlString = "<Server>" +
                                  "<Address>localhost</Address>" +
                                  "<Port>5432</Port>" +
                                  "<User>postgres</User>" +
                                  "<Password Secured=\"False\">1234</Password>" +
                                  "<Database>MT4</Database>" +
                                  "<SQLQuery>asdasdsadas</SQLQuery>" +
                                "</Server>";


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);


            sqlConfig.InitFields(xmlDoc.DocumentElement);
            SQLSettings sqlSettings2 = (SQLSettings)sqlConfig;
            
            XmlNode securedNode = xmlDoc.DocumentElement["Password"].GetAttributeNode("Secured");
            string passwordValue = xmlDoc.DocumentElement["Password"].InnerText;

            Assert.AreEqual("True", securedNode.Value, "Secured attribute should be set to 'True'");
            Assert.AreNotEqual("1234", passwordValue, "Password should be encrypted");

            Assert.AreEqual("host=localhost;port=5432;database=MT4;username=postgres;password=1234", sqlSettings2.GetConnectionString(), "The string must not be empty");

        }

    }
}