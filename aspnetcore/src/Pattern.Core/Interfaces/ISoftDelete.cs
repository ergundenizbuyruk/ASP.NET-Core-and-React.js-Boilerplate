namespace Pattern.Core.Interfaces
{
	public interface ISoftDelete
	{
		public bool IsDeleted { get; set; }
		public DateTimeOffset? DeletionTime { get; set; }
		public Guid? DeleterUserId { get; set; }
	}
}
