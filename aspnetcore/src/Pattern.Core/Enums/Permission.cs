namespace Pattern.Core.Enums
{
	public enum Permission
	{
		UserDefault = 1,
		UserCreate,
		UserUpdate,
		UserDelete,

		AccountDefault,
		AccountUpdate,
		AccountDelete,
		EmailChange,
		ChangePassword,

		RoleDefault,
		RoleCreate,
		RoleUpdate,
		RoleDelete
	}
}
