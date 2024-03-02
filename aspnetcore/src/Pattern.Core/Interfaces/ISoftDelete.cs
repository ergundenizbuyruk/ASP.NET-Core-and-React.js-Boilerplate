namespace Pattern.Core.Interfaces
{
	public interface ISoftDelete
	{
		public bool IsDeleted { get; set; }
		public DateTime? DeletionTime { get; set; }
		public Guid? DeleterUserId { get; set; }
	}
}
