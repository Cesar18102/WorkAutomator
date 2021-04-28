namespace WorkAutomatorLogic.Models
{
    public class SessionCredentialsModel : ModelBase
    {
        public int UserId { get; set; }
        public string SessionTokenSalted { get; set; }
        public string Salt { get; set; }
    }
}
