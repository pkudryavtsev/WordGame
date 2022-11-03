using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame
{
    public class Leaderboard
    {
        private PlayerScore[] Scores { get; } = new PlayerScore[10];
        private int Length { get; set; } = 0;
        private readonly object balanceLock = new object();

        public void AddToLeaderboard(PlayerScore playerScore)
        {
            lock (balanceLock)
            {
                // Find the place to insert a score
                int insertIndex = 0;
                if (Length == 0)
                {
                    Scores[0] = playerScore;
                    return;
                }

                while (Scores[insertIndex].Score > playerScore.Score)
                {
                    insertIndex++;
                }

                // Move right side of the array to right to free space for insert
                for (int i = Scores.Length - 1; i > insertIndex; i--)
                {
                    Scores[i] = Scores[i - 1];
                }

                Scores[insertIndex] = playerScore;
                Length++;
            }
        }

        public bool CheckIfShouldBeAdded(PlayerScore playerScore)
        {
            if (Length < 10)
                return true;

            // Get min score to get to leaderboard
            var minScore = Scores[Length - 1];

            // Check if playerScore.Score is bigger than min
            if (playerScore.Score < minScore.Score)
                return false;

            // Check if same score already exists and compare the submission time
            if (Scores.Any(s => s.Score == playerScore.Score && s.SubmissionTime < playerScore.SubmissionTime))
                return false;

            return true;
        }

        public string GetPlayerNameAtPosition(int position)
        {
            if (!ValidPosition(position))
                return null;

            return Scores[position - 1].PlayerName;
        }

        public int? GetScoreAtPosition(int position)
        {
            if (!ValidPosition(position))
                return null;

            return Scores[position - 1].Score;
        }

        public string GetWordEntryAtPosition(int position)
        {
            if (!ValidPosition(position))
                return null;

            return Scores[position - 1].Word;
        }

        // Check if position is in range of 1 to Length - 1 (current taken spots in leaderboard)
        private bool ValidPosition(int position) => position < 1 || position > Length - 1? false : true;

    }
}
