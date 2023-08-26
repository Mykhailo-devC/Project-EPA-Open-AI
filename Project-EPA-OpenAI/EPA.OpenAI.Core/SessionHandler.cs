using EPA.OpenAI.Models.Logic_Models;
using System.Diagnostics;

namespace EPA.OpenAI.Core
{
    public class SessionHandler
    {
        private ICollection<Session> Sessions {  get; set; } = new List<Session>();

        public Session NewSession(IEnumerable<SentenceData> sentences, string userName, ConversationHandler conversation)
        {
            var newSession = new Session
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                StartTime = DateTime.Now,
                Sentences = new Stack<SentenceData>(sentences.Reverse()),
                Conversation = conversation,
                Timer = new Stopwatch()
            };
            newSession.Timer.Start();

            Sessions.Add(newSession);
            return newSession;
        }

        public Session FindSession(Guid id, string userName)
        {
            return Sessions.FirstOrDefault(x => x.Id == id && x.UserName == userName);
        }

        public Session FindSession(Guid id)
        {
            return Sessions.FirstOrDefault(x => x.Id == id);
        }

        public TimeSpan EndSession(Guid id)
        {
            var session = FindSession(id);
            if (session == null) return TimeSpan.Zero;
            Sessions.Remove(session);
            session.Timer.Stop();
            return session.Timer.Elapsed;
        }

    }
}