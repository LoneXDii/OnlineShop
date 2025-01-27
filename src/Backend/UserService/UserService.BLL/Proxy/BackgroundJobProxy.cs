using System.Linq.Expressions;
using Hangfire;

namespace UserService.BLL.Proxy;

internal class BackgroundJobProxy : IBackgroundJobProxy
{
    public string Enqueue(Expression<Func<Task>> method)
    {
        return BackgroundJob.Enqueue(method);
    }

    public string Schedule(Expression<Func<Task>> method, TimeSpan delay)
    {
        return BackgroundJob.Schedule(method, delay);
    }
}