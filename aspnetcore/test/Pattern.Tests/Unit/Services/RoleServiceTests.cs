using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pattern.Application.Services.Roles;
using Pattern.Application.Services.Roles.Dtos;
using Pattern.Core;
using Pattern.Core.Entites.Authentication;
using Pattern.Core.Exceptions;
using Pattern.Persistence.Context;
using Pattern.Persistence.Repositories;
using Pattern.Persistence.UnitOfWork;
using Xunit;

namespace Pattern.Tests.Unit.Services;

public class RoleServiceTests
{
    private readonly Mock<IUnitOfWork> unitOfWorkMock = new();
    private readonly Mock<IMapper> mapperMock = new();
    private readonly Mock<RoleManager<Role>> roleManagerMock;
    private readonly Mock<IRepository<Permission, int>> permissionRepoMock = new();
    private readonly Mock<IResourceLocalizer> localizerMock = new();
    private readonly ApplicationDbContext dbContext;
    private readonly RoleService roleService;

    public RoleServiceTests()
    {
        var store = new Mock<IRoleStore<Role>>();
        roleManagerMock = new Mock<RoleManager<Role>>(store.Object, null, null, null, null);

        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns((HttpContext)null);

        dbContext = new ApplicationDbContext(options, httpContextAccessorMock.Object);

        roleService = new RoleService(
            unitOfWorkMock.Object,
            mapperMock.Object,
            roleManagerMock.Object,
            permissionRepoMock.Object,
            dbContext,
            localizerMock.Object
        );
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldCreateRoleAndReturnDto()
    {
        // Arrange
        var dto = new CreateRoleDto { Name = "Admin", PermissionIds = [] };
        var createdRole = new Role { Id = Guid.NewGuid(), Name = dto.Name };

        roleManagerMock.Setup(r => r.CreateAsync(It.IsAny<Role>()))
            .ReturnsAsync(IdentityResult.Success);

        mapperMock.Setup(m => m.Map<RoleDto>(It.IsAny<Role>()))
            .Returns(new RoleDto { Id = createdRole.Id, Name = createdRole.Name });

        // Act
        var result = await roleService.CreateRoleAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.Name);
    }

    [Fact]
    public async Task DeleteRoleAsync_ShouldThrowNotFound_WhenRoleDoesNotExist()
    {
        // Arrange
        roleManagerMock.Setup(r => r.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Role)null);

        localizerMock.Setup(l => l.Localize("RoleNotFound")).Returns("Role not found");

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            roleService.DeleteRoleAsync(Guid.NewGuid()));

        Assert.Equal("Role not found", ex.Message);
    }

    public void Dispose()
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}