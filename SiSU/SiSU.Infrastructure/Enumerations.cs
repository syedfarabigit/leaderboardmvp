
namespace SiSU.Infrastructure
{
    public enum UserRole
    {
        Referee = 1,
        Member = 2
    }

    public enum SiSUAuthorizationPolicy
    {
        RefreeAccess = 1,
        MemberAccess
    }

    public enum GameResult
    {
        Lost = 0,
        Drawn = 1,
        Won = 2
    }
}
