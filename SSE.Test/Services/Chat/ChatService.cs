
using SSE.Test.Models;
using SSE.Test.Services.Chat.BotReplyFactory;
using SSE.Test.Services.LLM;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SSE.Test.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly ILLMService _llmService;
        public ChatService(ILLMService llmService)
        {
            _llmService = llmService;
        }

        public async Task GetBotReplyAsync(ChatRequest request, Func<string, Task> onStreamChunk, CancellationToken cancellationToken)
        {
            try
            {
                // 봇 타입 별로 컨텍스트 만드는 서비스 나누기
                IBotReplyFactory factory = new GeneralFactory(request);

                string systemMessage = "";
                ChatMessage chatMessage = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = ""
                };
                string json = JsonSerializer.Serialize(chatMessage);
                string pattern = "(\"Message\"\\s*:\\s*\").*?(\"\\s*[},])";

                await _llmService.StreamLLMResponse(systemMessage, request.Question, async (message) =>
                {
                    string updatedJson = Regex.Replace(json, pattern, $"$1{message}$2");
                    onStreamChunk(updatedJson);
                }, cancellationToken);
            }
            catch(Exception ex)
            {
                throw new Exception("error");
            }
        }
    }
}
