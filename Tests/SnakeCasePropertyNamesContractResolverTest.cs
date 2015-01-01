using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization.ContractResolverExtentions;
using Newtonsoft.Json.Tests.Serialization;

namespace Tests
{
    [TestClass]
    public class SnakeCasePropertyNamesContractResolverTest : SnakeCaseContractResolverBaseTest<SnakeCasePropertyNamesContractResolver>
    {

        [TestMethod]
        public override void IntegersPropertyNames()
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "First1", "Value1!" },
                { "Second1", "Value2!" },
                { "1TwoThree", "Value3!"},
                { "One2Three", "Value4!"},
                { "OneTwo3", "Value5!"},
                { "One23", "Value6!"},
                { "12Three", "Value7!"},
                { "123", "Value8!"}
            };

            string json = JsonConvert.SerializeObject(values, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new SnakeCasePropertyNamesContractResolver()
                });

            Assert.AreEqual(@"{
  ""first1"": ""Value1!"",
  ""second1"": ""Value2!"",
  ""1_two_three"": ""Value3!"",
  ""one2_three"": ""Value4!"",
  ""one_two3"": ""Value5!"",
  ""one23"": ""Value6!"",
  ""12_three"": ""Value7!"",
  ""123"": ""Value8!""
}", json);
        }
    }
}
