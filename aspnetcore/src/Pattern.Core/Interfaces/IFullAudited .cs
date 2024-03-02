namespace Pattern.Core.Interfaces
{
	public interface IFullAudited : ISoftDelete
	{
		public DateTimeOffset CreationTime { get; set; }
		public Guid? CreatorUserId { get; set; }
		public DateTimeOffset? LastModificationTime { get; set; }
		public Guid? LastModifierUserId { get; set; }
	}
}
