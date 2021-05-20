using System;
using System.ComponentModel.DataAnnotations;

namespace Redfin.Models.Options
{
    /// <summary>
    /// The source options
    /// </summary>
    public record SourceOpts
    {
        public const string Section = "Redfin";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = Section + ":" + nameof(PollingInterval) + " is missing")]
        public TimeSpan PollingInterval { get; init; } = new(24, 3, 31);
    }
}
