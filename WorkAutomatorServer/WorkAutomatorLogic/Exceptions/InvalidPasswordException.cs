namespace WorkAutomatorLogic.Exceptions
{
    public class InvalidPasswordException : LogicExceptionBase
    {

        public override string Message => "Password must consist of English letters and digits between 8 and 32 symbols";
    }
}
