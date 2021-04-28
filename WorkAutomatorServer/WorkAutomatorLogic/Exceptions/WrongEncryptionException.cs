namespace WorkAutomatorLogic.Exceptions
{
    public class WrongEncryptionException : LogicExceptionBase
    {
        public string WrongEncryptedField { get; private set; }
        public override string Message => $"{WrongEncryptedField} was encrypted incorrectly";

        public WrongEncryptionException(string wrongEncryptedField)
        {
            WrongEncryptedField = wrongEncryptedField;
        }
    }
}
