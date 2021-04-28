namespace WorkAutomatorLogic.Models
{
    public class LogInFormModel : ModelBase
    {
        public string Login { get; set; }
        public string PasswordSalted { get; set; }
        public string Salt { get; set; }
    }
}
