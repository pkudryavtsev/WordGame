namespace WordGame
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class ValidWords : IValidWords
    {
        private HashSet<string> _words = new HashSet<string>();

        public ValidWords()
        {
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                stream = Assembly.GetAssembly(typeof(ValidWords)).GetManifestResourceStream("WordGame.wordlist.txt");
                reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    _words.Add(reader.ReadLine());
                }
            }
            finally
            {
                reader.Dispose();
                stream.Dispose();
            }
        }

        public int Size
        {
            get { return _words.Count; }
        }

        public bool Contains(string word)
        {
            return _words.Contains(word);
        }
    }
}
