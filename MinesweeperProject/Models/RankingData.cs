using System;
using System.Collections.Generic;

namespace MinesweeperProject.Models
{
        public class RankingData
        {
            // 난이도별로 리스트 관리 (Key: 난이도명, Value: 상위 3명 리스트)
            public Dictionary<string, List<RankingEntry>> DifficultyRankings { get; set; } = new()
                {
                    { "쉬움", new List<RankingEntry>() },
                    { "보통", new List<RankingEntry>() },
                    { "어려움", new List<RankingEntry>() },
                    { "극한", new List<RankingEntry>() }
                };
        }

        public class RankingEntry
        {
            public int Rank { get; set; }           // 순위 (1, 2, 3...)
            public string Nickname { get; set; }    // 닉네임
            public string Difficulty { get; set; }  // 난이도
            public int Time { get; set; } // 시간

            // 화면에 "01:25" 형태로 보여주기 위한 속성
            public string TimeDisplay
            {
                get
                {
                    int min = Time / 60;
                    int sec = Time % 60;
                    return $"{min:D2}:{sec:D2}";
                }
            }
        }

        
}
