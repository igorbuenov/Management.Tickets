namespace Tickets.WebAPI.Models.Users.Response
{
    public class CreateUserResponseModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

        public CreateUserResponseModel()
        {

        }
        public void Success()
        {
            Message = $"Usuário para {Name}, criado com sucesso! UserId: {UserId}";
        }

        public void Error()
        {
            Message = $"Falha ao criar usuário para {Name}!";
        }
    }
}
