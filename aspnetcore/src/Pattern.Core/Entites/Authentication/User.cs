using Microsoft.AspNetCore.Identity;
using Pattern.Core.Interfaces;

namespace Pattern.Core.Entites.Authentication
{
    public class User : IdentityUser<Guid>, ICanBePassive, IFullAudited, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreationTime { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public DateTime? DeletionTime { get; set; }
	}
}
