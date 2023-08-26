using OpenAI_API;

namespace EPA.OpenAI.Core
{
    public class OpenAIApiHandler
    {
        public OpenAIAPI Api { get; private set; }
        public OpenAIApiHandler() { Init(); }
        private void Init()
        {
            var path = Directory.GetParent(Environment.CurrentDirectory)
                                .GetDirectories()
                                .FirstOrDefault(x => x.GetFiles("EPA.OpenAI.Core.csproj").Any())?
                                .FullName;

            Api = new OpenAIAPI(APIAuthentication.LoadFromPath(path));
        }
    }
}