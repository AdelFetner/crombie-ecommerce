namespace crombie_ecommerce.DTOs
{
    public class NotificationDTO
    {
        public string NotificationType { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public Guid ProductId { get; set; }
    }
}
