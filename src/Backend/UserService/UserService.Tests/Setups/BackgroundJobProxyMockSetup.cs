using System.Linq.Expressions;
using Moq;
using UserService.BLL.Proxy;

namespace UserService.Tests.Setups;

public static class BackgroundJobProxyMockSetup
{
    public static void SetupBackgroundJobProxy(this Mock<IBackgroundJobProxy> mock)
    {
        mock.Setup(job => job.Enqueue(It.IsAny<Expression<Func<Task>>>()))
            .Callback<Expression<Func<Task>>>(expression =>
            {
                var func = expression.Compile();
                func.Invoke();
            });
    }
}