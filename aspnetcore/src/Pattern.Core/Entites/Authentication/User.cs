using Microsoft.AspNetCore.Identity;
using Pattern.Core.Interfaces;

namespace Pattern.Core.Entites.Authentication
{
	public class User : IdentityUser<Guid>, ICanBePassive, IFullAudited
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public Guid? CreatorUserId { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public Guid? LastModifierUserId { get; set; }
		public DateTime? DeletionTime { get; set; }
		public Guid? DeleterUserId { get; set; }
		public bool IsDeleted { get; set; }
	}
}
