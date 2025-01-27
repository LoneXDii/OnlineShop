using System.Linq.Expressions;

namespace ProductsService.Application.Proxy;

internal interface IBackgroundJobProxy
{
    string Schedule(Expression<Func<Task>> method, DateTime enqueuedAt);
}