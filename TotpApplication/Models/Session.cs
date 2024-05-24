namespace TotpApplication.Models
{
    public class Session
    {
        private static Session? _instance;
        private static readonly object lockObject = new();

        public List<User> AllUsers { get; private set; }
        public User CurrentUser { get; private set; }

        private Session()
        {
            AllUsers = new List<User>
            {
                // Initialize users
                new() { Id = 1, Email = "abbas@gmail.com", Name = "Abbas", IsTotopEnabled = false, Password = "Pakistan", SecretKey = "MYEQW3UI4I22I===" }
            };
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
