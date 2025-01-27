namespace crombie_ecommerce.Models.Dto
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }
        public string Address { get; set; }
    }
}
