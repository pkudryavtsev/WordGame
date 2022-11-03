namespace WordGame
{
    public interface IValidWords
    {
        /// <summary>
        ///     Gets the size of the valid words collection.
        /// </summary>
        int Size { get; }

        /// <summary>
        ///     Checks if a word is valid
        /// </summary>
        /// <param name="word">the word to check against the valid words collection</param>
        /// <returns>true if the valid words collection contains the word</returns>
        bool Contains(string word);
    }
}
