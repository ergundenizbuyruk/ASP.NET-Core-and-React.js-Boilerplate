# ASP.NET-Core-API-Pattern
My starter template project

I'm still developing

- I used the generic repository pattern. I scan all my entity classes that inherit from the Entity class, create a Repository class and automatically add to the IoC container. Thus, I can use it directly in my service classes as IRepository<TEntity>.

- It automatically performs soft delete for classes that implement the ISoftDelete interface.

- It automatically sets the creation, modification and deletion times for classes that implement the IFullAudited interface.

- I used the generic response pattern.

- All service classes inherited from the BaseService class, include IMapper and IUnitOfWork objects.

- It contains basic endpoints such as token creation, deletion, user CRUD operations, forget password, change email, change password...

Libraries used:
- Entity Framework Core
- Identity
- AutoMapper
- Jwt Bearer

What I will add in the future:
- Caching
- Logging
- Localization
- Notification System (Email, Sms, Push)
- Maybe Multitenancy