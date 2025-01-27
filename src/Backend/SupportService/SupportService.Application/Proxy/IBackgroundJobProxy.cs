using System.Linq.Expressions;

namespace SupportService.Application.Proxy;

internal interface IBackgroundJobProxy
{
    string Schedule(Expression<Func<Task>> method, TimeSpan delay);
}
