using Hangfire;
using System.Linq.Expressions;

namespace SupportService.Application.Proxy;

internal class BackgroundJobProxy : IBackgroundJobProxy
{
    public string Schedule(Expression<Func<Task>> method, TimeSpan delay)
    {
        return BackgroundJob.Schedule(method, delay);
    }
}
