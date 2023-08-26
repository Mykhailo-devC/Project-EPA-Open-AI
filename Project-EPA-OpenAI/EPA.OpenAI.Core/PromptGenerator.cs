namespace EPA.OpenAI.Core
{
    public static class PromptGenerator
    {
        private const string GenereteSentencesPrompt = "Hello, I give you this list of words: ({0}). " +
                "Please, generate sentences with each word of this list.";
        private const string AnilyzeSentencePrompt = "Hello, please analyze, how good I translate these sentences:" +
                "({0}) and ({1}), in context ({2}).";

        public static string CreatePromptForGeneratingSentences(IEnumerable<string> words)
        {
            return string.Format(GenereteSentencesPrompt, string.Join(",", words));
        }
        public static string CreatePromptForAnalyzingSentence(params string[] arg)
        {
            return string.Format(AnilyzeSentencePrompt, arg);
        }
    }
}