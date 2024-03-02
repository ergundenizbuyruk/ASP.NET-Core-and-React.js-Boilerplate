using Microsoft.AspNetCore.Identity;
using Pattern.Core.Interfaces;

namespace Pattern.Core.Entites.Authentication
{
	public class Role : IdentityRole<Guid>, IFullAudited
	{
		public List<Permission> Permissions { get; set; }
		public DateTimeOffset CreationTime { get; set; }
		public Guid? CreatorUserId { get; set; }
		public DateTimeOffset? LastModificationTime { get; set; }
		public Guid? LastModifierUserId { get; set; }
		public DateTimeOffset? DeletionTime { get; set; }
		public Guid? DeleterUserId { get; set; }
		public bool IsDeleted { get; set; }
	}
}
