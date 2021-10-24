namespace Redfin.Models.Shared;

/// <summary>
/// The shared resource across the application
/// </summary>
public record Resource
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string RPID { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public decimal Estimate { get; init; } = 0.0M;
}
