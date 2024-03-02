namespace Pattern.Core.Interfaces
{
	public interface IFullAudited : ISoftDelete
	{
		public DateTime CreationTime { get; set; }
		public Guid? CreatorUserId { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public Guid? LastModifierUserId { get; set; }
	}
}
