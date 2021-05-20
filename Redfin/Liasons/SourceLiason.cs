using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TwoMQTT.Interfaces;
using TwoMQTT.Liasons;
using Redfin.DataAccess;
using Redfin.Models.Options;
using Redfin.Models.Shared;
using Redfin.Models.Source;

namespace Redfin.Liasons
{
    /// <summary>
    /// A class representing a managed way to interact with a source.
    /// </summary>
    public class SourceLiason : PollingSourceLiasonBase<Resource, SlugMapping, ISourceDAO, SharedOpts>, ISourceLiason<Resource, object>
    {
        public SourceLiason(ILogger<SourceLiason> logger, ISourceDAO sourceDAO,
            IOptions<SourceOpts> opts, IOptions<SharedOpts> sharedOpts) :
            base(logger, sourceDAO, sharedOpts)
        {
            this.Logger.LogInformation(
                "PollingInterval: {pollingInterval}\n" +
                "Resources: {@resources}\n" +
                "",
                opts.Value.PollingInterval,
                sharedOpts.Value.Resources
            );
        }

        /// <inheritdoc />
        protected override async Task<Resource?> FetchOneAsync(SlugMapping key, CancellationToken cancellationToken)
        {
            var result = await this.SourceDAO.FetchOneAsync(key, cancellationToken);
            return result switch
            {
                Response => new Resource
                {
                    RPID = result.RPID,
                    Estimate = result.Estimate,
                },
                _ => null,
            };
        }
    }
}
