
namespace BowlingScoreApi.Services
{
    /// <summary>
    /// Service interface for the score calculation
    /// </summary>
    public interface IScoreService
    {
        BowlingScoreApi.Models.FrameScore CalculateFrameScore(int firstRollPinsCount, 
            int secondRollPinsCount, 
            int thirdRollPinsCount, 
            int totalScore,
            int frame,
            bool strike, 
            bool spare,
            bool PrevStrike, 
            bool PrevSpare);
    }
}
