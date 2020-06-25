using CraftBackEnd.Services.Interfaces;
using CraftBackEnd.Common.Models.Exception;

namespace CraftBackEnd.Services
{
    public class ValidationService : IValidationService
    {
        public void ThrowValidationErrors(string error) {
            throw new ValidationErrorException(error);
        }
    }
}
