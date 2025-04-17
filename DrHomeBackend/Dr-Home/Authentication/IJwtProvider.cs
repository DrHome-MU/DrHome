namespace Dr_Home.Authentication
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}
