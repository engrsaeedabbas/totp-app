namespace TotpApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecretKey { get; set; }
        public bool IsTotpEnabled { get; set; }
        public bool IsCodeValidated { get; set; }
    }  

}
