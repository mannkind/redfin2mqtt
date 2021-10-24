using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Redfin.DataAccess;
using Redfin.Liasons;
using Redfin.Models.Options;
using Redfin.Models.Shared;
using TwoMQTT;
using TwoMQTT.Extensions;
using TwoMQTT.Interfaces;
using TwoMQTT.Managers;

await ConsoleProgram<Resource, object, SourceLiason, MQTTLiason>.
    ExecuteAsync(args,
        envs: new Dictionary<string, string>()
        {
            {
                $"{MQTTOpts.Section}:{nameof(MQTTOpts.TopicPrefix)}",
                MQTTOpts.TopicPrefixDefault
            },
            {
                $"{MQTTOpts.Section}:{nameof(MQTTOpts.DiscoveryName)}",
                MQTTOpts.DiscoveryNameDefault
            },
        },
        configureServices: (HostBuilderContext context, IServiceCollection services) =>
        {
            services
                .AddHttpClient()
                .AddOptions<SharedOpts>(SharedOpts.Section, context.Configuration)
                .AddOptions<SourceOpts>(SourceOpts.Section, context.Configuration)
                .AddOptions<TwoMQTT.Models.MQTTManagerOptions>(MQTTOpts.Section, context.Configuration)
                .AddSingleton<IThrottleManager, ThrottleManager>(x =>
                {
                    var opts = x.GetRequiredService<IOptions<SourceOpts>>();
                    return new ThrottleManager(opts.Value.PollingInterval);
                })
                .AddSingleton<ISourceDAO, SourceDAO>();
        });