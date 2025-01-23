
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace SSE.Test.Services.LLM
{
    public class LLMService : ILLMService
    {
        private readonly AzureOpenAIClient _client;
        private readonly ChatClient _chatClient;

        public LLMService()
        {
            _client = new(new Uri("https://langcode-cxp-aoai.openai.azure.com"), new AzureKeyCredential("1d7ccf059ffb426d9c8adc882a20baea"));
            _chatClient = _client.GetChatClient("gpt-4o-0806");
        }

        public async Task StreamLLMResponse(string systemMessage, string prompt, Func<string, Task> onStreamChunk, CancellationToken cancellationToken)
        {
            var answer = string.Empty;

            var chatCompletionOptions = new ChatCompletionOptions
            {
                Temperature = 0
            };

            try
            {
                // _chatClient.CompleteChatStreamingAsync() 호출
                var chatStream = _chatClient.CompleteChatStreamingAsync(
                    new List<ChatMessage>
                    {
                        new SystemChatMessage(systemMessage),
                        new UserChatMessage(prompt)
                    },
                    chatCompletionOptions);

                
                // 스트림에서 데이터를 순차적으로 읽기
                await foreach (var choice in chatStream.WithCancellation(cancellationToken))
                {
                    // ContentUpdate가 존재하면 처리
                    if (choice.ContentUpdate is not null && choice.ContentUpdate.Count > 0)
                    {
                        var chunk = choice.ContentUpdate[0].Text;
                        answer += chunk;

                        // 클라이언트로 chunk 전송
                        await onStreamChunk(answer);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 요청이 취소된 경우
                Console.WriteLine("Request was canceled.");
            }
            catch (Exception ex)
            {
                // 예외 처리
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

}
