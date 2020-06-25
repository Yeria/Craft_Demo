namespace CraftBackEnd.Common.Models.Exception
{
    public class ValidationErrorException : System.Exception
    {
        public ValidationErrorException(string message)
            : base(message) {
        }

        public ValidationErrorException(string message, System.Exception inner)
            : base(message, inner) {
        }
    }
}
