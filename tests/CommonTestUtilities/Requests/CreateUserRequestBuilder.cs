using Bogus;
using Tickets.Application.DTOs.Users;

namespace CommonTestUtilities.Requests
{
    public class CreateUserRequestBuilder
    {
        public static CreateUserRequestDto Build()
        {
            return new Faker<CreateUserRequestDto>()
                .RuleFor(user => user.Name, f => f.Name.FullName())
                .RuleFor(user => user.Email, f => f.Internet.Email())
                .RuleFor(user => user.RoleID, f => f.Random.Int(1, 3))
                .Generate();
        }
    }
}
