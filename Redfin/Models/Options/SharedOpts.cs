using System.Collections.Generic;
using Redfin.Models.Shared;
using TwoMQTT.Interfaces;

namespace Redfin.Models.Options;

/// <summary>
/// The shared options across the application
/// </summary>
public record SharedOpts : ISharedOpts<SlugMapping>
{
    public const string Section = "Redfin";

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="SlugMapping"></typeparam>
    /// <returns></returns>
    public List<SlugMapping> Resources { get; init; } = new();
}
