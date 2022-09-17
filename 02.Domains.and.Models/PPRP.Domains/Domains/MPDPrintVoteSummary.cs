using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPRP.Domains
{
    public class MPDPrintVoteSummary
    {
        #region Person 1

        public byte[] Logo1 { get; set; }
        public byte[] PersonImage1 { get; set; }
        public string PartyName1 { get; set; }
        public string FullName1 { get; set; }
        public int VoteCount1 { get; set; }

        #endregion

        #region Person 2

        public byte[] Logo2 { get; set; }
        public byte[] PersonImage2 { get; set; }
        public string PartyName2 { get; set; }
        public string FullName2 { get; set; }
        public int VoteCount2 { get; set; }

        #endregion

        #region Person 3

        public byte[] Logo3 { get; set; }
        public byte[] PersonImage3 { get; set; }
        public string PartyName3 { get; set; }
        public string FullName3 { get; set; }
        public int VoteCount3 { get; set; }

        #endregion

        #region Person 4

        public byte[] Logo4 { get; set; }
        public byte[] PersonImage4 { get; set; }
        public string PartyName4 { get; set; }
        public string FullName4 { get; set; }
        public int VoteCount4 { get; set; }

        #endregion

        #region Person 5

        public byte[] Logo5 { get; set; }
        public byte[] PersonImage5 { get; set; }
        public string PartyName5 { get; set; }
        public string FullName5 { get; set; }
        public int VoteCount5 { get; set; }

        #endregion

        #region Person 6

        public byte[] Logo6 { get; set; }
        public byte[] PersonImage6 { get; set; }
        public string PartyName6 { get; set; }
        public string FullName6 { get; set; }
        public int VoteCount6 { get; set; }

        #endregion

        #region Candidate

        public byte[] CandidateImage { get; set; }
        public string CandidateFullName { get; set; }
        public string CandidateSubGroup { get; set; }
        public int CandidatePrevYear { get; set; }
        public string CandidatePrevVote { get; set; }
        public string CandidateRemark { get; set; }

        #endregion

        #region General Vote information

        public int PrevVoteYear { get; set; }
        public int PollingUnitCount { get; set; }
        public int RightsCount { get; set; }
        public int ExcerciseCount { get; set; }
        public decimal ExcercisePercent { get; set; }
        public int DifferenceVoteFromNo2 { get; set; }
        public int VoteCount7toLast { get; set; }

        #endregion
    }
}
