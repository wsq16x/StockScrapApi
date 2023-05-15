using StockScrapApi.Dtos;

namespace StockScrapApi.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto userDTO);
        Task<string> CreateToken();
    }
}
