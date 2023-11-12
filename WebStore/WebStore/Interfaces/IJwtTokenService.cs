using WebStore.Data.Entities.Identity;

public interface IJwtTokenService
{
    Task<string> CreateToken(UserEntity user);
}