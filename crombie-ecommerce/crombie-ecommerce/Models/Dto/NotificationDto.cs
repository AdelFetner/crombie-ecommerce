namespace crombie_ecommerce.DTOs
{
    public class NotificationDTO
    {
        public Guid NotfId { get; set; }

        public string NotificationType { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsRead { get; set; }
    }
}
