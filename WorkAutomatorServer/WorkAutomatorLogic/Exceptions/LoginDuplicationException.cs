namespace WorkAutomatorLogic.Exceptions
{
    public class LoginDuplicationException : LogicExceptionBase
    {
        public override string Message => $"User with such login already exists";
    }
}
