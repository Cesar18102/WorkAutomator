namespace WorkAutomatorLogic.Exceptions
{
    public class NotFoundException : LogicExceptionBase
    {
        public string TypeName { get; private set; }
        public override string Message => $"{TypeName} not found";

        public NotFoundException(string typeName)
        {
            TypeName = typeName;
        }
    }
}
