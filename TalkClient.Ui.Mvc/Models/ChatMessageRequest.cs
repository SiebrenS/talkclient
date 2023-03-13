namespace TalkClient.Ui.Mvc.Models
{
    public class ChatMessageRequest
    {
        public required string Author { get; set; }
        public required string Channel { get; set; }
        public required string Message { get; set; }
    }
}
