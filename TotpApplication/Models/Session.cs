namespace TotpApplication.Models
{
    public class Session
    {
        private static Session? _instance;
        private static readonly object lockObject = new();

        public List<User> AllUsers { get; private set; }
        public User CurrentUser { get; set; }

        private Session()
        {
            AllUsers = new List<User>
            {
                // Initialize users
                new() { Id = 1, Email = "saeed@gmail.com", Name = "Saeed", IsTotpEnabled = false, Password = "Pakistan", SecretKey = null, IsCodeValidated = false }
            };
            /*CurrentUser = null;*/
        }

        public static Session GetInstance()
        {
            lock (lockObject)
            {
                _instance ??= new Session();
                return _instance;
            }
        }
    }
}
