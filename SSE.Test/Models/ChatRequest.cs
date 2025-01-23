namespace SSE.Test.Models
{
    public class ChatRequest
    {
        public string BotId { get; set; } = string.Empty;
        public ChatMessage Question { get; set; } = new();
    }
}
