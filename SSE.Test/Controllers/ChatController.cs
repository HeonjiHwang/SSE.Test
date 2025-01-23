using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSE.Test.Models;
using SSE.Test.Services.Chat;

namespace SSE.Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // 메시지 전송
        [HttpPost("send")]
        public async Task SendMessage([FromBody] ChatRequest request)
        {
            Response.ContentType = "text/event-stream";
            if(Response.Headers.ContainsKey("Cache-Control"))
            {
                Response.Headers.Remove("Cache-Control");
            }
            if(Response.Headers.ContainsKey("Connection"))
            {
                Response.Headers.Remove("Connection");
            }

            Response.Headers.Add("Cache-Control", "no-cache");
            Response.Headers.Add("Connection", "keep-alive");

            var cancellationToken = HttpContext.RequestAborted;

            await _chatService.GetBotReplyAsync(request, async (message) =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    var lines = message.Split('\n');
                    foreach (var line in lines)
                    {
                        await Response.WriteAsync($"data: {line}\n");
                    }
                    await Response.WriteAsync("\n");
                    await Response.Body.FlushAsync();
                }
            }, cancellationToken);
        }
    }
}
