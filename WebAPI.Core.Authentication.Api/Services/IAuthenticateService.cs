using System.Threading;
using System.Threading.Tasks;
using WebAPI.Core.Authentication.Api.Models;

namespace WebAPI.Core.Authentication.Api.Services
{
    public interface IAuthenticateService
    {
        Task<User> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken);
    }
}
