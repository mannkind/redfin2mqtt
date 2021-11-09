using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Redfin.DataAccess;
using Redfin.Liasons;
using Redfin.Models.Options;
using Redfin.Models.Shared;

namespace RedfinTest.Liasons;

[TestClass]
public class SourceLiasonTest
{
    [TestMethod]
    public async Task FetchAllAsyncTest()
    {
        var tests = new[] {
                new {
                    Q = new SlugMapping { RPID = BasicRPID, Slug = BasicSlug },
                    Expected = new { RPID = BasicRPID, Estimate = BasicAmount }
                },
            };

        foreach (var test in tests)
        {
            var logger = new Mock<ILogger<SourceLiason>>();
            var sourceDAO = new Mock<ISourceDAO>();
            var opts = Options.Create(new SourceOpts());
            var sharedOpts = Options.Create(new SharedOpts
            {
                Resources = new[] { test.Q }.ToList(),
            });

            sourceDAO.Setup(x => x.FetchOneAsync(test.Q, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new Redfin.Models.Source.Response
                 {
                     RPID = test.Expected.RPID,
                     Estimate = test.Expected.Estimate,
                 });

            var sourceLiason = new SourceLiason(logger.Object, sourceDAO.Object, opts, sharedOpts);
            await foreach (var result in sourceLiason.ReceiveDataAsync())
            {
                Assert.AreEqual(test.Expected.RPID, result.RPID);
                Assert.AreEqual(test.Expected.Estimate, result.Estimate);
            }
        }
    }

    private static string BasicSlug = "totallyaslug";
    private static decimal BasicAmount = 10000.52M;
    private static string BasicRPID = "15873525";
}
