# ASP.NET-Core-and-React.js-Boilerplate

My starter template project

I'm still developing

## Core Layer

- It automatically performs soft delete for classes that implement the **ISoftDelete** interface.

- It automatically sets the creation, modification and deletion times for classes that implement the **IFullAudited** interface.

- I used the generic response pattern.

## Persistence Layer

- I used the generic repository pattern. I scan all my entity classes that inherit from the Entity class, create a Repository class and automatically add to the IoC container. Thus, I can use it directly in my service classes as IRepository<TEntity>.

## Application Layer

- All service classes inherited from the **ApplicationService** or **CrudService** class, include IMapper and IUnitOfWork objects.

- Your service classes that inherit from the **CrudService** class have CRUD operations and are automatically added to the IoC container. You do not need to write AddScoped in Program.cs file. You can review the sample code below.

```
public class MyService : CrudService<MyEntity, int, MyEntityDto, MyCreateEntityDto, MyUpdateEntityDto>, IMyService {
    public MyService(IUnitOfWork unitOfWork, IMapper objectMapper, IRepository<MyEntity, int> repository) : base(unitOfWork, objectMapper, repository)
    {
    }
}

public interface IMyService : ICrudService<MyEntity, int, MyEntityDto, MyCreateEntityDto, MyUpdateEntityDto {
}
```

- Now all your CRUD services are ready. Additionally, your MyService class has been added to IoC with the IMyService interface. You can use it directly in your Controller classes with contructor injection using the IMyService reference.

- You can customize the services you want to change by overriding them.

- If you do not want CRUD services, you can create service classes as follows.

```
public class MyService : ApplicationService, IMyService {
    public MyService(IUnitOfWork unitOfWork, IMapper objectMapper) : base(unitOfWork, objectMapper)
    {
    }
}

public interface IMyService : IApplicationService {
}
```

- This way, you will have services automatically added to the IoC container. Additionally, ObjectMapper and UnitOfWork objects will be added. You can use the function from the superclass to commit to the database with the SaveChangesAsync method.

## Api Layer

- It contains basic endpoints such as token creation, deletion, user CRUD operations, forget password, change email, change password, reset password, email confirm...

- You can send e-mail by entering the Mail server information in the **EmailConfiguration** section in the appsettings.json file.

- All exceptions are managed using the **UseGlobalExceptionHandler** middleware. You can return a custom response by writing your own custom exception classes and editing them here.

Libraries used:

- Entity Framework Core
- Identity
- AutoMapper
- Jwt Bearer

What I will add in the future:

- Caching
- Logging
- Localization
- Notification System (Sms, Push)
- Multitenancy
