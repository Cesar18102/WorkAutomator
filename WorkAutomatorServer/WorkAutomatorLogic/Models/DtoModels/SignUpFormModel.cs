namespace WorkAutomatorLogic.Models
{
    public class SignUpFormModel : ModelBase
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordEncrypted { get; set; }
        public string Email { get; set; }
        public PublicKeyModel PublicKey { get; set; }
    }
}