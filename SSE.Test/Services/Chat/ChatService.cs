
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
                    Message = "",
                    SenderType = "Bot",
                    BodyType = "Plain",
                    SentDate = DateTime.UtcNow,
                    MessageType = "loading"
                };
                string updatedJson = "";
                string json = JsonSerializer.Serialize(chatMessage);
                string pattern = "(\"Message\"\\s*:\\s*\").*?(\"\\s*[},])";

                await _llmService.StreamLLMResponse(systemMessage, request.Question.Message, async (message) =>
                {
                    updatedJson = Regex.Replace(json, pattern, $"$1{message}$2");
                    onStreamChunk(updatedJson);
                }, cancellationToken);

                var pattern2 = "(\"MessageType\"\\s*:\\s*\").*?(\"\\s*[},])";
                updatedJson = Regex.Replace(updatedJson, pattern2, $"$1{""}$2");
                onStreamChunk(updatedJson);
            }
            catch(Exception ex)
            {
                throw new Exception("error");
            }
        }
    }
}
