using System.Linq.Expressions;
using ProductsService.Application.Specifications;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Tests.UnitTests.Specifications;

public class AndSpecificationTests
{
    private class TestEntity : IEntity
    {
        public int Id { get; set; }
    }

    private class MockSpecification : Specification<TestEntity>
    {
        private readonly Func<TestEntity, bool> _predicate;

        public MockSpecification(Func<TestEntity, bool> predicate)
        {
            _predicate = predicate;
        }

        public override Expression<Func<TestEntity, bool>> ToExpression()
        {
            return entity => _predicate(entity);
        }
    }

    [Fact]
    public void ToExpression_WhenBothSpecificationsAreTrue_ShouldReturnTrue()
    {
        //Arrange
        var spec1 = new MockSpecification(e => e.Id > 0);
        var spec2 = new MockSpecification(e => e.Id < 10);
        var andSpecification = new AndSpecification<TestEntity>(spec1, spec2);

        //Act
        var expression = andSpecification.ToExpression();
        var func = expression.Compile();
        var result = func(new TestEntity { Id = 5 });

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void ToExpression_WhenFirstSpecificationIsFalse_ShouldReturnFalse()
    {
        //Arrange
        var spec1 = new MockSpecification(e => e.Id > 10);
        var spec2 = new MockSpecification(e => e.Id < 10);
        var andSpecification = new AndSpecification<TestEntity>(spec1, spec2);

        //Act
        var expression = andSpecification.ToExpression();
        var func = expression.Compile();
        var result = func(new TestEntity { Id = 5 });

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenSecondSpecificationIsFalse_ShouldReturnFalse()
    {
        //Arrange
        var spec1 = new MockSpecification(e => e.Id > 0);
        var spec2 = new MockSpecification(e => e.Id < 0);
        var andSpecification = new AndSpecification<TestEntity>(spec1, spec2);

        //Act
        var expression = andSpecification.ToExpression();
        var func = expression.Compile();
        var result = func(new TestEntity { Id = 5 });

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void ToExpression_WhenBothSpecificationsAreFalse_ShouldReturnFalse()
    {
        //Arrange
        var spec1 = new MockSpecification(e => e.Id < 0);
        var spec2 = new MockSpecification(e => e.Id < 0);
        var andSpecification = new AndSpecification<TestEntity>(spec1, spec2);

        //Act
        var expression = andSpecification.ToExpression();
        var func = expression.Compile();
        var result = func(new TestEntity { Id = 5 });

        //Assert
        Assert.False(result);
    }
}