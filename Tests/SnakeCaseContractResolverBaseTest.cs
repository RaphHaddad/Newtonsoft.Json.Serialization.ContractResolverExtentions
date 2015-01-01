using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Serialization.ContractResolverExtentions;
using System.Reflection;
namespace Newtonsoft.Json.Tests.Serialization
{
    [TestClass]
    public class SnakeCaseContractResolverBaseTest<T> where T : DefaultContractResolver, new()
    {
        public void JsonConvertSerializerSettings()
        {
            Person person = new Person();
            person.BirthDate = new DateTime(2000, 11, 20, 23, 55, 44, DateTimeKind.Utc);
            person.LastModified = new DateTime(2000, 11, 20, 23, 55, 44, DateTimeKind.Utc);
            person.Name = "Name!";

            string json = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new T()
            });

            Assert.AreEqual(@"{
  ""name"": ""Name!"",
  ""birth_date"": ""2000-11-20T23:55:44Z"",
  ""last_modified"": ""2000-11-20T23:55:44Z""
}", json);

            Person deserializedPerson = JsonConvert.DeserializeObject<Person>(json, new JsonSerializerSettings
            {
                ContractResolver = new T()
            });

            Assert.AreEqual(person.BirthDate, deserializedPerson.BirthDate);
            Assert.AreEqual(person.LastModified, deserializedPerson.LastModified);
            Assert.AreEqual(person.Name, deserializedPerson.Name);

            json = JsonConvert.SerializeObject(person, Formatting.Indented);
            Assert.AreEqual(@"{
  ""Name"": ""Name!"",
  ""BirthDate"": ""2000-11-20T23:55:44Z"",
  ""LastModified"": ""2000-11-20T23:55:44Z""
}", json);
        }

        [TestMethod]
        public void JTokenWriter()
        {
            JsonIgnoreAttributeOnClassTestClass ignoreAttributeOnClassTestClass = new JsonIgnoreAttributeOnClassTestClass();
            ignoreAttributeOnClassTestClass.Field = int.MinValue;

            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = new T();

            JTokenWriter writer = new JTokenWriter();

            serializer.Serialize(writer, ignoreAttributeOnClassTestClass);

            JObject o = (JObject)writer.Token;
            JProperty p = o.Property("the_field");

            Assert.IsNotNull(p);
            Assert.AreEqual(int.MinValue, (int)p.Value);

            string json = o.ToString();
        }

        [TestMethod]
        public void BlogPostExample()
        {
            Product product = new Product
            {
                ExpiryDate = new DateTime(2010, 12, 20, 18, 1, 0, DateTimeKind.Utc),
                Name = "Widget",
                Price = 9.99m,
                Sizes = new[] { "Small", "Medium", "Large" }
            };

            string json =
                JsonConvert.SerializeObject(
                    product,
                    Formatting.Indented,
                    new JsonSerializerSettings { ContractResolver = new T() }
                    );

            //{
            //  "name": "Widget",
            //  "expiryDate": "\/Date(1292868060000)\/",
            //  "price": 9.99,
            //  "sizes": [
            //    "Small",
            //    "Medium",
            //    "Large"
            //  ]
            //}

            Assert.AreEqual(@"{
  ""name"": ""Widget"",
  ""expiry_date"": ""2010-12-20T18:01:00Z"",
  ""price"": 9.99,
  ""sizes"": [
    ""Small"",
    ""Medium"",
    ""Large""
  ]
}", json);
        }



        [TestMethod]
        public void DictionaryCamelCasePropertyNames()
        {
            Dictionary<string, string> values = new Dictionary<string, string>
            {
                { "First", "Value1!" },
                { "Second", "Value2!" }
            };

            string json = JsonConvert.SerializeObject(values, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new T()
                });

            Assert.AreEqual(@"{
  ""first"": ""Value1!"",
  ""second"": ""Value2!""
}", json);
        }


        [TestMethod]
        public virtual void IntegersPropertyNames()
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
                    ContractResolver = new T()
                });

            Assert.AreEqual(@"{
  ""first_1"": ""Value1!"",
  ""second_1"": ""Value2!"",
  ""1_two_three"": ""Value3!"",
  ""one_2_three"": ""Value4!"",
  ""one_two_3"": ""Value5!"",
  ""one_23"": ""Value6!"",
  ""12_three"": ""Value7!"",
  ""123"": ""Value8!""
}", json);
        }
    }
}