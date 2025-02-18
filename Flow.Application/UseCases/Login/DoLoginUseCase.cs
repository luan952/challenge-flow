using Flow.Core.DTOs;
using Flow.Core.Repositories;
using Flow.Core.Security.Tokens;
using Flow.Exceptions;

namespace Flow.Application.UseCases.Login
{
    public class DoLoginUseCase : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository;
        private readonly ITokenGenerate _tokenGenerate;

        public DoLoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, ITokenGenerate tokenGenerate)
        {
            _userReadOnlyRepository = userReadOnlyRepository;
            _tokenGenerate = tokenGenerate;
        }

        public async Task<string> Execute(DoLoginRequest request)
        {
            var user = await _userReadOnlyRepository.GetUserByLogin(request.Login);
            if (user == null)
            {
                throw new FlowException("User not found");
            }

            if (user.Password != request.Password)
            {
                throw new FlowException("Invalid password");
            }

            return _tokenGenerate.GenerateToken(user.Id);
        }
    }
}
