using SSE.Test.Models;

namespace SSE.Test.Services.Chat
{
    public interface IChatService
    {
        Task GetBotReplyAsync(ChatRequest request, Func<string, Task> onStreamChunk, CancellationToken cancellationToken);
    }
}
