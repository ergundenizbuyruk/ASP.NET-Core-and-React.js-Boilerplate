using Pattern.Core.Entites.BaseEntity;

namespace Pattern.Core.Entites
{
	public class Province : Entity
	{
		public int Plaka { get; set; }
		public string ProvinceText { get; set; }
		public string AreaCode { get; set; }
		public List<District> Districts { get; set; }
	}
}
