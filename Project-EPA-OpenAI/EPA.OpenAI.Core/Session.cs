using EPA.OpenAI.Models.Logic_Models;
using System.Diagnostics;

namespace EPA.OpenAI.Core
{
    public class Session
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Stack<SentenceData> Sentences { get; set; }
        public DateTime StartTime { get; set; }
        public ConversationHandler Conversation { get; set; }
        public Stopwatch Timer { get; set; }
    }
}
