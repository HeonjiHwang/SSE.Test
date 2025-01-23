namespace SSE.Test.Models
{
    public class ChatMessage
    {
        public string Id { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string SenderType { get; set; } = string.Empty;
        public string BodyType { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }
        public string MessageType { get; set; } = string.Empty;
    }
}
