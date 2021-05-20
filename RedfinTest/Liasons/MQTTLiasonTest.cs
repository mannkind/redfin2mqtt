using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TwoMQTT.Utils;
using Redfin.Liasons;
using Redfin.Models.Options;
using Redfin.Models.Shared;

namespace RedfinTest.Liasons
{
    [TestClass]
    public class MQTTLiasonTest
    {
        [TestMethod]
        public void MapDataTest()
        {
            var tests = new[] {
                new {
                    Q = new SlugMapping { RPID = BasicRPID, Slug = BasicSlug },
                    Resource = new Resource { RPID = BasicRPID, Estimate = BasicAmount },
                    Expected = new { RPID = BasicRPID, Estimate = BasicAmount, Slug = BasicSlug, Found = true }
                },
                new {
                    Q = new SlugMapping { RPID = BasicRPID, Slug = BasicSlug },
                    Resource = new Resource { RPID = $"{BasicRPID}-fake" , Estimate = BasicAmount },
                    Expected = new { RPID = string.Empty, Estimate = 0.0M, Slug = string.Empty, Found = false }
                },
            };

            foreach (var test in tests)
            {
                var logger = new Mock<ILogger<MQTTLiason>>();
                var generator = new Mock<IMQTTGenerator>();
                var sharedOpts = Options.Create(new SharedOpts
                {
                    Resources = new[] { test.Q }.ToList(),
                });

                generator.Setup(x => x.BuildDiscovery(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<System.Reflection.AssemblyName>(), false))
                    .Returns(new TwoMQTT.Models.MQTTDiscovery());
                generator.Setup(x => x.StateTopic(test.Q.Slug, nameof(Resource.Estimate)))
                    .Returns($"totes/{test.Q.Slug}/topic/{nameof(Resource.Estimate)}");

                var mqttLiason = new MQTTLiason(logger.Object, generator.Object, sharedOpts);
                var results = mqttLiason.MapData(test.Resource);
                var actual = results.FirstOrDefault();

                Assert.AreEqual(test.Expected.Found, results.Any(), "The mapping should exist if found.");
                if (test.Expected.Found)
                {
                    Assert.IsTrue(actual.topic.Contains(test.Expected.Slug), "The topic should contain the expected RPID.");
                    Assert.AreEqual(test.Expected.Estimate.ToString(), actual.payload, "The payload be the expected Estimate.");
                }
            }
        }

        [TestMethod]
        public void DiscoveriesTest()
        {
            var tests = new[] {
                new {
                    Q = new SlugMapping { RPID = BasicRPID, Slug = BasicSlug },
                    Resource = new Resource { RPID = BasicRPID, Estimate = BasicAmount },
                    Expected = new { RPID = BasicRPID, Estimate = BasicAmount, Slug = BasicSlug }
                },
            };

            foreach (var test in tests)
            {
                var logger = new Mock<ILogger<MQTTLiason>>();
                var generator = new Mock<IMQTTGenerator>();
                var sharedOpts = Options.Create(new SharedOpts
                {
                    Resources = new[] { test.Q }.ToList(),
                });

                generator.Setup(x => x.BuildDiscovery(test.Q.Slug, nameof(Resource.Estimate), It.IsAny<System.Reflection.AssemblyName>(), false))
                    .Returns(new TwoMQTT.Models.MQTTDiscovery());

                var mqttLiason = new MQTTLiason(logger.Object, generator.Object, sharedOpts);
                var results = mqttLiason.Discoveries();
                var result = results.FirstOrDefault();

                Assert.IsNotNull(result, "A discovery should exist.");
            }
        }

        private static string BasicSlug = "totallyaslug";
        private static decimal BasicAmount = 10000.52M;
        private static string BasicRPID = "15873525";
    }
}
