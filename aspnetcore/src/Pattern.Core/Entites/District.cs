using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Core.Entites
{
	public class District : Entity
	{
		public string DistrictText { get; set; }
		public Province Province { get; set; }
		public int ProvinceId { get; set; }
	}
}
