using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BowlingScoreApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class FrameScore
    {
        #region CONSTANTS
        /// <summary>
        /// Total number of pins in a frame
        /// </summary>
        public const int TotalPinsCount = 10;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Pins down in first roll in a frame
        /// </summary>
        public int FirstRollPinsCount { get; set; }
        /// <summary>
        /// Pins down in second roll in a frame
        /// </summary>
        public int SecondRollPinsCount { get; set; }
        /// <summary>
        /// Pins down in third roll in a frame in case of tenth frame
        /// </summary>
        public int ThirdRollPinsCount { get; set; }
        /// <summary>
        /// Total Score of all played frames
        /// </summary>
        public int TotalScore { get; set; }       
        #endregion
    }
}