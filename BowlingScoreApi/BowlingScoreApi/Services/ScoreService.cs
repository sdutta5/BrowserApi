using BowlingScoreApi.Models;

namespace BowlingScoreApi.Services
{
    /// <summary>
    /// Service class with definition to interface methods
    /// </summary>
    public class ScoreService : IScoreService
    {
        /// <summary>
        /// Calculates the frame score and current total score based on input received in querystring
        /// </summary>
        /// <param name="firstRollPinsCount">Pins down in first roll</param>
        /// <param name="secondRollPinsCount">Pins down in second roll</param>
        /// <param name="thirdRollPinsCount">in case of tenth frame, pins down in third roll</param>
        /// <param name="strike">specifies whether last frame was a strike</param>
        /// <param name="spare">specifies whether last frame was a spare</param>
        /// <returns>An object of FrameScore Model class</returns>
        public FrameScore CalculateFrameScore(int firstRollPinsCount,
            int secondRollPinsCount,
            int thirdRollPinsCount,
            int totalScore,
            int frame,
            bool strike,
            bool spare,
            bool pstrike,
            bool pspare)
        {
            FrameScore frameModel = new FrameScore()
            {
                FirstRollPinsCount = firstRollPinsCount,
                SecondRollPinsCount = secondRollPinsCount,
                ThirdRollPinsCount = thirdRollPinsCount,
                TotalScore = totalScore
            };

            if (FrameScore.TotalPinsCount == frame && (firstRollPinsCount == FrameScore.TotalPinsCount || firstRollPinsCount + secondRollPinsCount == FrameScore.TotalPinsCount) && strike == false && spare == false)
            {
                frameModel.TotalScore += firstRollPinsCount + secondRollPinsCount + thirdRollPinsCount;
            }
            else if (firstRollPinsCount + secondRollPinsCount < FrameScore.TotalPinsCount && strike == false && spare == false)
            {
                frameModel.TotalScore += firstRollPinsCount + secondRollPinsCount + thirdRollPinsCount;
            }

            if (strike)
            {
                frameModel.TotalScore += StrikeFrame(firstRollPinsCount, secondRollPinsCount, thirdRollPinsCount, pstrike);
            }
            if (spare)
            {
                frameModel.TotalScore += SpareFrame(firstRollPinsCount, pspare) + secondRollPinsCount + thirdRollPinsCount;
            }
            return frameModel;
        }

        #region PRIVATE METHODS
        /// <summary>
        /// Calculates score for the current frame and previous frame which was strike.
        /// Also keeps track of and calculates the previous to previous strike.
        /// </summary>
        /// <param name="firstRollPinsCount">Pins down in first roll</param>
        /// <param name="secondRollPinsCount">Pins down in second roll</param>
        /// <param name="thirdRollPinsCount">Pins down in third roll</param>
        /// <param name="pstrike">Specifies whether the previous to previous frame was strike</param>
        /// <returns>Total score of the current frame</returns>
        private int StrikeFrame(int firstRollPinsCount, int secondRollPinsCount, int thirdRollPinsCount, bool pstrike)
        {
            int total = (2 * (firstRollPinsCount + secondRollPinsCount)) + thirdRollPinsCount;
            if (pstrike)
            {
                total += firstRollPinsCount;
                return total;
            }
            total += FrameScore.TotalPinsCount;
            return total;
        }

        /// <summary>
        /// Calculates score for the current frame and previous frame which was strike.
        /// Also keeps track of and calculates the previous to previous spare 
        /// </summary>
        /// <param name="firstRollPinsCount">Pins down in first roll</param>
        /// <param name="pspare">Specifies whether the previous to previous frame was spare</param>
        /// <returns></returns>
        private int SpareFrame(int firstRollPinsCount, bool pspare)
        {
            int total = 2 * firstRollPinsCount;
            if (!pspare)
            {
                total += FrameScore.TotalPinsCount;
            }
            return total;
        }
        #endregion

    }
}