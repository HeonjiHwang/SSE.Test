namespace SSE.Test.Services.LLM
{
    public interface ILLMService
    {
        Task StreamLLMResponse(string systemMessage, string prompt, Func<string, Task> onStreamChunk, CancellationToken cancellationToken);
    }
}
