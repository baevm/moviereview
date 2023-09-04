namespace moviereview.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActivated { get; set; }
        public string AvatarURL { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}