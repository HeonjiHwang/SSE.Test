using SSE.Test.Models;

namespace SSE.Test.Services.Chat.BotReplyFactory
{
    public class RAGFactory : IBotReplyFactory
    {
        private readonly string _question;
        public RAGFactory(ChatRequest request) 
        {
            _question = request.Question.Message;
        }

        public Task GetBotReplyContextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
