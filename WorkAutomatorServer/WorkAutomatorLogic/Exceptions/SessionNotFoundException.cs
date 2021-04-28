namespace WorkAutomatorLogic.Exceptions
{
    public class SessionNotFoundException : LogicExceptionBase
    {
        public override string Message => "Session not found";
    }
}
