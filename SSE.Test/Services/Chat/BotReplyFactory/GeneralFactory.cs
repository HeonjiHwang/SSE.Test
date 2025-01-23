using SSE.Test.Models;

namespace SSE.Test.Services.Chat.BotReplyFactory
{
    public class GeneralFactory : IBotReplyFactory
    {
        private readonly string _question;
        public GeneralFactory(ChatRequest request)
        {
            _question = request.Question;
        }

        public Task GetBotReplyContextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
