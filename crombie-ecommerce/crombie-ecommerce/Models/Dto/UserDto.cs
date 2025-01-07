namespace crombie_ecommerce.Models.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? WishlistId { get; set; }
    }
}
