namespace Dating.Models
{
    public class ChatViewModel
    {
        public List<MessagesSModels> Messages { get; set; }  // Danh sách tin nhắn giữa hai người
        public int ReceiverId { get; set; }            // ID người nhận
        public string ReceiverUsername { get; set; }   // Tên người nhận
        public string SenderProfilePicture { get; set; }
    }
}
