using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Redfin.Models.Options;
using Redfin.Models.Shared;
using TwoMQTT;
using TwoMQTT.Interfaces;
using TwoMQTT.Liasons;
using TwoMQTT.Models;
using TwoMQTT.Utils;

namespace Redfin.Liasons;

/// <inheritdoc />
public class MQTTLiason : MQTTLiasonBase<Resource, object, SlugMapping, SharedOpts>, IMQTTLiason<Resource, object>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="generator"></param>
    /// <param name="sharedOpts"></param>
    public MQTTLiason(ILogger<MQTTLiason> logger, IMQTTGenerator generator, IOptions<SharedOpts> sharedOpts) :
        base(logger, generator, sharedOpts)
    {
        this.DataReceivedExpiration = sharedOpts.Value.DataReceivedExpiration;
    }

    /// <inheritdoc />
    public IEnumerable<(string topic, string payload)> MapData(Resource input)
    {
        var results = new List<(string, string)>();
        var slug = this.Questions
            .Where(x => x.RPID == input.RPID)
            .Select(x => x.Slug)
            .FirstOrDefault() ?? string.Empty;

        if (string.IsNullOrEmpty(slug))
        {
            this.Logger.LogDebug("Unable to find slug for {rpid}", input.RPID);
            return results;
        }

        this.Logger.LogDebug("Found slug {slug} for incoming data for {rpid}", slug, input.RPID);
        results.AddRange(new[]
            {
                this.Generator.DataReceivedTopicPayload(slug),

                (this.Generator.StateTopic(slug, nameof(Resource.Estimate)), input.Estimate.ToString()),
            }
        );

        return results;
    }

    /// <inheritdoc />
    public IEnumerable<(string slug, string sensor, string type, MQTTDiscovery discovery)> Discoveries()
    {
        var discoveries = new List<(string, string, string, MQTTDiscovery)>();
        var assembly = Assembly.GetAssembly(typeof(Program))?.GetName() ?? new AssemblyName();
        var mapping = new[]
        {
            new { Sensor = nameof(Resource.Estimate), Type = Const.SENSOR },
        };

        foreach (var input in this.Questions)
        {
            discoveries.Add(this.Generator.DataReceivedDiscovery(input.Slug, assembly, this.DataReceivedExpiration));

            foreach (var map in mapping)
            {
                this.Logger.LogDebug("Generating discovery for {rpid} - {sensor}", input.RPID, map.Sensor);
                var discovery = this.Generator.BuildDiscovery(input.Slug, map.Sensor, assembly, false);
                discovery = discovery with { Icon = "mdi:home-variant" };

                discoveries.Add((input.Slug, map.Sensor, map.Type, discovery));
            }
        }

        return discoveries;
    }

    /// <summary>
    /// 
    /// </summary>
    private readonly TimeSpan DataReceivedExpiration;
}
