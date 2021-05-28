namespace WorkAutomatorLogic.Exceptions
{
    public class PointsBusyException : LogicExceptionBase
    {
        public override string Message => "Points are already used";
    }
}
