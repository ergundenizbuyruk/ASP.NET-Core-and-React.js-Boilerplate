using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Core.Entites.Authentication
{
    public class UserRefreshToken : Entity
    {
        public string Code { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}