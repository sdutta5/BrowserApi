using BowlingScoreApi.Models;

namespace BowlingScoreApi.Services
{
    public class ScoreService : IScoreService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRollPinsCount"></param>
        /// <param name="secondRollPinsCount"></param>
        /// <param name="thirdRollPinsCount"></param>
        /// <param name="strike"></param>
        /// <param name="spare"></param>
        /// <returns></returns>
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
            if (firstRollPinsCount + secondRollPinsCount < FrameScore.TotalPinsCount && strike == false && spare == false)
            {
                frameModel.TotalScore += firstRollPinsCount + secondRollPinsCount + thirdRollPinsCount;
            }
            if (strike)
            {
                frameModel.TotalScore += StrikeFrame(firstRollPinsCount, secondRollPinsCount, pstrike);
            }
            if (spare)
            {
                frameModel.TotalScore += SpareFrame(firstRollPinsCount, pspare) + secondRollPinsCount;
            }
            return frameModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRollPinsCount"></param>
        /// <param name="secondRollPinsCount"></param>
        /// <returns></returns>
        private int StrikeFrame(int firstRollPinsCount, int secondRollPinsCount, bool pstrike)
        {
            int total = FrameScore.TotalPinsCount + (2 * (firstRollPinsCount + secondRollPinsCount));
            if (pstrike)
            {
                total -= FrameScore.TotalPinsCount;
                total += firstRollPinsCount;
            }
            return total;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstRollPinsCount"></param>
        /// <returns></returns>
        private int SpareFrame(int firstRollPinsCount, bool pspare)
        {
            int total = FrameScore.TotalPinsCount + (2 * firstRollPinsCount);
            if (pspare)
            {
                total -= FrameScore.TotalPinsCount;
            }
            return total;
        }
    }
}