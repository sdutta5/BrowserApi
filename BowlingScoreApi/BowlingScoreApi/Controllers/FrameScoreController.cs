using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using BowlingScoreApi.Services;

namespace BowlingScoreApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FrameScoreController : ApiController
    {
        public readonly IScoreService scoreService;
        /// <summary>
        /// 
        /// </summary>
        public FrameScoreController()
        {
            scoreService = new ScoreService();
        }
        // GET api/framescore/5
        /// <summary>
        /// REST API GET METHOD FOR ACCESSIBILITY
        /// </summary>
        /// <param name="firstrollpins">Pins down in first roll in a frame</param>
        /// <param name="secondrollpins">Pins down in second roll in a frame</param>
        /// <param name="thirdrollpins">Pins down in third roll in a frame</param>
        /// <param name="strike">specifies whether the last frame was strike</param>
        /// <param name="spare">specifies whether the last frame was spare</param>
        /// <param name="totalscore">specifies the total score of all the frames</param>
        /// <returns></returns>
        public string Get(string firstrollpins,
            string secondrollpins,
            string thirdrollpins = "0",
            string strike = "false",
            string spare = "false",
            string totalscore = "0",
            bool pstrike = false,
            bool pspare = false,
            int framenum = 0)
        {
            try
            {
                int firstRollPins = Convert.ToInt32(firstrollpins);
                int secondRollPins = Convert.ToInt32(secondrollpins);
                int thirdRollPins = Convert.ToInt32(thirdrollpins);
                int totalScore = Convert.ToInt32(totalscore);
                int frameNumber = Convert.ToInt32(framenum);
                bool Strike = Convert.ToBoolean(strike);
                bool Spare = Convert.ToBoolean(spare);
                bool PrevStrike = Convert.ToBoolean(pstrike);
                bool PrevSpare = Convert.ToBoolean(pspare);

                var model = scoreService.CalculateFrameScore(firstRollPins, secondRollPins, thirdRollPins, totalScore, frameNumber, Strike, Spare, PrevStrike, PrevSpare);
                return JsonConvert.SerializeObject(model);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
