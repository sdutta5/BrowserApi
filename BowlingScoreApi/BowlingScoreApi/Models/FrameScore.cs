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

        public const int TotalPinsCount = 10;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// 
        /// </summary>
        public int FirstRollPinsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SecondRollPinsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ThirdRollPinsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalScore { get; set; }       
        #endregion
    }
}