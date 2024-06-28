using ConsysApi.Data.Model;

namespace ConsysApi.Interfaces
{
    public interface ITokenService
    {
        string GenereteToken(Usuarios user);
    }
}
