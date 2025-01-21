using System.Linq.Expressions;

namespace UserService.BLL.Proxy;

public interface IBackgroundJobProxy
{
    string Enqueue(Expression<Func<Task>> method);
    string Schedule(Expression<Func<Task>> method, TimeSpan delay);
}