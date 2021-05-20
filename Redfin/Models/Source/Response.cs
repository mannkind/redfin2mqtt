using System;

namespace Redfin.Models.Source
{
    /// <summary>
    /// The response from the source
    /// </summary>
    public record Response
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
}