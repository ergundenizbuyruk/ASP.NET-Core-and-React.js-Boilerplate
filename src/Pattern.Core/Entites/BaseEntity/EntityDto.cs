namespace Pattern.Core.Entites.BaseEntity
{
	public abstract class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
	{
		public TPrimaryKey Id { get; set; }

		public EntityDto(TPrimaryKey id)
		{
			Id = id;
		}
	}

	public abstract class EntityDto : EntityDto<int>, IEntityDto
	{
		public EntityDto(int id) : base(id)
		{
			Id = id;
		}
	}
}
