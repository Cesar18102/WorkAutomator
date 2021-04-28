namespace WorkAutomatorLogic.Exceptions
{
    public class InvalidKeyException : LogicExceptionBase
    {
        public override string Message => "Presented key was destroyed or never existed";
    }
}
