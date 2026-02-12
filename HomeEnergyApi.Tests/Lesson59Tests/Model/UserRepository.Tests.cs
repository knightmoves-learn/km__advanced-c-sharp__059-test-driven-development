using Microsoft.EntityFrameworkCore;
using HomeEnergyApi.Models;

public class UserRepositoryTest : IAsyncLifetime
{
    private readonly User _testUser;
    private UserRepository repository;
    private HomeDbContext _context;

    public UserRepositoryTest()
    {
        _testUser = new User
        {
            Id = 1,
            Username = "testUsername",
            Role = "testRole",
            HashedPassword = "testHashedPassword",
            EncryptedAddress = "testEncryptedAddress"
        };
    }

    public async Task InitializeAsync()
    {
        _context = new MockDb().CreateDbContext();
        repository = new UserRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task ShouldSaveUser_WhenValidUserProvided()
    {
        var saveUser = repository.Save(_testUser);

        Assert.NotNull(saveUser);
        Assert.Equal(1, saveUser.Id);
    }

    [Fact]
    public async Task ShouldNotSaveUser_WhenInvalidUserProvided()
    {
        _testUser.Username = null;

        var exception = Assert.Throws<DbUpdateException>(() => repository.Save(_testUser));

        Assert.Contains("Required properties '{'Username'}' are missing for the instance of entity type 'User'.", exception.Message);
    }

    [Fact]
    public async Task ShouldFindUserByUsername_WhenUserExists()
    {
        var saveUser = repository.Save(_testUser);

        var foundUser = repository.FindByUsername(_testUser.Username);

        Assert.NotNull(foundUser);
        Assert.Equal(foundUser.Id, saveUser.Id);
    }

    [Fact]
    public async Task ShouldNotFindUserByUsername_WhenUserDoesNotExists()
    {
        var saveUser = repository.Save(_testUser);

        var foundUser = repository.FindByUsername("badName");

        Assert.Null(foundUser);
    }

    [Fact]
    public async Task ShouldRemoveUserById_WhenUserExists()
    {
        var saveUser = repository.Save(_testUser);
        var countBefore = repository.Count();

        var foundUser = repository.RemoveById(saveUser.Id);

        var countAfter = repository.Count();
        Assert.NotNull(foundUser);
        Assert.Equal(foundUser.Id, saveUser.Id);
        Assert.Equal(1, countBefore);
        Assert.Equal(0, countAfter);
    }

    [Fact]
    public async Task ShouldReturnNull_WhenRemovingUserWithAnIdThatDoesNotExist()
    {
        var foundUser = repository.RemoveById(1);

        Assert.Null(foundUser);
    }

    [Fact]
    public async Task ShouldUpdateUser_WhenUserExists()
    {
        var saveUser = repository.Save(_testUser);

        saveUser.Username = "updatedTestUsername";

        repository.Update(saveUser.Id, saveUser);

        var foundUser = repository.FindByUsername(_testUser.Username);
        Assert.NotNull(foundUser);
        Assert.Equal(foundUser.Id, saveUser.Id);
        Assert.Equal("updatedTestUsername", foundUser.Username);
    }

    [Fact]
    public async Task ShouldNotUpdateUser_WhenUserIsNull()
    {
        Assert.Throws<NullReferenceException>(() => repository.Update(1, null));
    }

    [Fact]
    public async Task ShouldCountUser_WhenUserExists()
    {
        var saveUser = repository.Save(_testUser);

        var count = repository.Count();

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task ShouldCountUser_WhenNoUsersExists()
    {
        var count = repository.Count();

        Assert.Equal(0, count);
    }
}