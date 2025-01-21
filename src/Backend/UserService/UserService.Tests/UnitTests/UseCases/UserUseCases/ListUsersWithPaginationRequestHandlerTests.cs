using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using UserService.BLL.DTO;
using UserService.BLL.UseCases.UserUseCases.ListUsersWithPaginationUseCase;
using UserService.DAL.Entities;
using UserService.Tests.Factories;

namespace UserService.Tests.UnitTests.UseCases.UserUseCases;

public class ListUsersWithPaginationRequestHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ListUsersWithPaginationRequestHandler _handler;
    private readonly Fixture _fixture = new();

    public ListUsersWithPaginationRequestHandlerTests()
    {
        _userManagerMock = MocksFactory.CreateUserManager();
        _handler = new ListUsersWithPaginationRequestHandler(
            _userManagerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WhenNoUsersExist_ShouldReturnEmptyPaginatedList()
    {
        //Arrange
        var request = _fixture.Create<ListUsersWithPaginationRequest>();
        var users = new List<AppUser>(); 

        var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

        _userManagerMock.Setup(manager => manager.Users).Returns(asyncUsers);

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Handle_WhenUsersExist_ShouldReturnPaginatedListOfUsers()
    {
        //Arrange
        var request = new ListUsersWithPaginationRequest(new PaginationDTO {PageNo = 1, PageSize = 5});
        var users = _fixture.CreateMany<AppUser>(5).ToList();
        var userWithRolesDTO = _fixture.Create<UserWithRolesDTO>();

        var asyncUsers = new TestAsyncEnumerable<AppUser>(users);
        
        _userManagerMock.Setup(manager => manager.Users)
            .Returns(asyncUsers);

        _mapperMock.Setup(mapper => mapper.Map<UserWithRolesDTO>(It.IsAny<AppUser>()))
            .Returns(userWithRolesDTO);

        _userManagerMock.Setup(manager => manager.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(new List<string> { "User" });

        //Act
        var result = await _handler.Handle(request, CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(users.Count, result.Items.Count);
        Assert.Equal(request.pagination.PageNo, result.CurrentPage);
        _mapperMock.Verify(mapper => mapper.Map<UserWithRolesDTO>(It.IsAny<AppUser>()), Times.Exactly(users.Count));
    }
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
    {
    }

    public TestAsyncEnumerable(Expression expression) : base(expression)
    {
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());

        public T Current => _inner.Current;
    }

    private class TestAsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var executionResult = Execute(expression);

            return Task.FromResult((dynamic)executionResult).Result;
        }
    }
}