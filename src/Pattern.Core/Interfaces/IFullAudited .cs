namespace Pattern.Core.Interfaces
{
    public interface IFullAudited
    {
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
