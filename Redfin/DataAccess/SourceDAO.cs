using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TwoMQTT.Interfaces;
using Redfin.Models.Shared;
using Redfin.Models.Source;
using System.Text;

namespace Redfin.DataAccess
{
    public interface ISourceDAO : IPollingSourceDAO<SlugMapping, Response, object, object>
    {
    }

    /// <summary>
    /// An class representing a managed way to interact with a source.
    /// </summary>
    public class SourceDAO : ISourceDAO
    {
        /// <summary>
        /// Initializes a new instance of the SourceDAO class. 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClientFactory"></param>
        /// <returns></returns>
        public SourceDAO(ILogger<SourceDAO> logger, IHttpClientFactory httpClientFactory)
        {
            this.Logger = logger;
            this.Client = httpClientFactory.CreateClient();
        }

        /// <inheritdoc />
        public async Task<Response?> FetchOneAsync(SlugMapping data,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await this.FetchAsync(data.RPID, cancellationToken);
            }
            catch (Exception e)
            {
                var msg = e switch
                {
                    HttpRequestException => "Unable to fetch from the Redfin API",
                    JsonException => "Unable to deserialize response from the Redfin API",
                    _ => "Unable to send to the Redfin API"
                };
                this.Logger.LogError(msg + "; {exception}", e);
                return null;
            }
        }

        /// <summary>
        /// The logger used internally.
        /// </summary>
        private readonly ILogger<SourceDAO> Logger;

        /// <summary>
        /// The client used to access the source.
        /// </summary>
        private readonly HttpClient Client;

        /// <summary>
        /// Fetch one response from the source
        /// </summary>
        /// <param name="rpid"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<Response?> FetchAsync(string rpid,
            CancellationToken cancellationToken = default)
        {
            this.Logger.LogInformation("Started finding {rpid} from Redfin", rpid);


            var url = "https://redfin.com/stingray/api/home/details/avm";
            var query = $"propertyId={rpid}&accessLevel=1";
            var msg = new HttpRequestMessage(HttpMethod.Get, $"{url}?{query}");
            msg.Headers.TryAddWithoutValidation("user-agent", "redfin");
            var resp = await this.Client.SendAsync(msg, cancellationToken);
            resp.EnsureSuccessStatusCode();
            var content = await resp.Content.ReadAsStringAsync();
            if (content.StartsWith(JUNK))
            {
                content = content.Substring(JUNK.Length);
            }
            var obj = JsonConvert.DeserializeObject<ApiResponse>(content);

            this.Logger.LogDebug("Finished finding {rpid} from Redfin", rpid);

            return obj?.ResultCode switch
            {
                0 => new Response
                {
                    RPID = rpid,
                    Estimate = obj.Payload.PredictedValue
                },
                _ => null,
            };
        }

        private const string JUNK = "{}&&";
    }

    record ApiResponse
    {
        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; init; } = string.Empty;

        [JsonProperty("resultCode")]
        public int ResultCode { get; init; } = 0;

        [JsonProperty("payload")]
        public ApiResponsePayload Payload { get; init; } = new();
    }

    record ApiResponsePayload
    {
        [JsonProperty("predictedValue")]
        public decimal PredictedValue { get; init; } = 0.0M;
    }
}
