using System.Linq.Expressions;
using Hangfire;

namespace ProductsService.Application.Proxy;

internal class BackgroundJobProxy : IBackgroundJobProxy
{
    public string Schedule(Expression<Func<Task>> method, DateTime enqueuedAt)
    {
        return BackgroundJob.Schedule(method, enqueuedAt);
    }
}