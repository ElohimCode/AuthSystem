namespace Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<string> ErrorMessages { get; set; }
        public string ClientErrorMessage { get; set; }
        public CustomValidationException(List<string> errorMessages, string clientErrorMessage)
        {
            ErrorMessages = errorMessages;
            ClientErrorMessage = clientErrorMessage;
        }
    }
}