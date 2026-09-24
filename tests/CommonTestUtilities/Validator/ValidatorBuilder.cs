using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace CommonTestUtilities.Validator
{
    public class ValidatorBuilder
    {
        public static IValidator<T> Build<T>(
            bool isValid = true,
            IEnumerable<ValidationFailure>? failures = null)
        {
            var mock = new Mock<IValidator<T>>();

            var providedFailures = failures?.ToList() ?? new List<ValidationFailure>();

            if (!isValid && !providedFailures.Any())
            {
                providedFailures.Add(
                    new ValidationFailure("Request", "Validation failed"));
            }

            var validationResult = new ValidationResult(providedFailures);

            mock.Setup(v => v.Validate(It.IsAny<T>()))
                .Returns(validationResult);

            return mock.Object;
        }
    }
}