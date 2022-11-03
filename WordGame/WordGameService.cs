namespace WordGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WordGameService : IWordGameService
    {
        private IValidWords ValidWords { get; }
        private Dictionary<char, int> ValidChars { get; }
        private Leaderboard Leaderboard { get; }

        public WordGameService(string letters, IValidWords validWords) : this(letters.ToCharArray(), validWords)
        {
        }

        public WordGameService(char[] letters, IValidWords validWords)
        {
            ValidWords = validWords;
            Leaderboard = new Leaderboard();
            ValidChars = GenerateDictFromChars(letters);
        }

        private Dictionary<char, int> GenerateDictFromChars(char[] letters)
        {
            var dict = new Dictionary<char, int>();
            foreach(var l in letters)
            {
                if (!dict.ContainsKey(l))
                    dict.Add(l, 1);
                else
                    dict[l]++;
            }

            return dict;
        }

        public string GetPlayerNameAtPosition(int position)
        {
            return Leaderboard.GetPlayerNameAtPosition(position);
        }

        public int? GetScoreAtPosition(int position)
        {
            return Leaderboard.GetScoreAtPosition(position);
        }

        public string GetWordEntryAtPosition(int position)
        {
            return Leaderboard.GetWordEntryAtPosition(position);
        }

        public int? SubmitWord(string playerName, string word)
        {
            // Save the submission time as concurrent submissions might be checked not in the arrival order
            var submissionTime = DateTimeOffset.Now;


            // First check whether all letters are valid.
            var canBeMadeFromLetters = ValidateFromLetters(word);

            if (!canBeMadeFromLetters)
                return null;

            // Then check whether is in the list of words.
            if (!ValidWords.Contains(word))
                return null;

            int score = word.Length;

            // Create instance to store in leaderboard
            var playerScore = new PlayerScore(score, word, playerName, submissionTime);

            // Check if playerScore should be added
            var shouldBeAdded = Leaderboard.CheckIfShouldBeAdded(playerScore);

            // Add the word to the leaderboard
            if (shouldBeAdded)
                Leaderboard.AddToLeaderboard(playerScore);

            return score;
        }

        private bool ValidateFromLetters(string word)
        {
            var localDict = new Dictionary<char, int>(ValidChars);

            foreach (char l in word)
            {
                if (localDict.ContainsKey(l) && localDict[l] > 0)
                    localDict[l]--;
                else
                    return false;
            }

            return true;
        }
    }

    // Would rather use records in this case, but the Framework version does not allow
    public class PlayerScore
    {
        public int Score { get; }
        public string Word { get; }
        public string PlayerName { get; }
        public DateTimeOffset SubmissionTime { get; }

        public PlayerScore(int score, string word, string playerName, DateTimeOffset submissionTime)
        {
            Score = score;
            Word = word;
            PlayerName = playerName;
            SubmissionTime = submissionTime;
        }
    }
}
