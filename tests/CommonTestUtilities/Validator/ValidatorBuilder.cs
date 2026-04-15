using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace CommonTestUtilities.Validator
{
    public class ValidatorBuilder
    {
        public static IValidator<T> Build<T>(bool isValid = true, IEnumerable<ValidationFailure>? failures = null)
        {
            var mock = new Mock<IValidator<T>>();

            var providedFailures = failures?.ToList() ?? new List<ValidationFailure>();
            if (!isValid && !providedFailures.Any())
            {
                providedFailures.Add(new ValidationFailure("Request", "Validation failed"));
            }

            var validationResult = new ValidationResult(providedFailures);

            // Setup synchronous Validate(T) if available
            mock.Setup(v => v.Validate(It.IsAny<T>()))
                .Returns(validationResult);

            // Setup synchronous Validate(ValidationContext<T>)
            mock.Setup(v => v.Validate(It.IsAny<ValidationContext<T>>()))
                .Returns(validationResult);

            // Setup asynchronous ValidateAsync(ValidationContext<T>, CancellationToken)
            mock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<T>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Setup asynchronous ValidateAsync(T, CancellationToken) (extension method signature)
            mock.Setup(v => v.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            return mock.Object;

        }
    }
}
