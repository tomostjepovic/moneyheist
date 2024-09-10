using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyHeist.Application.Interfaces;

namespace MoneyHeist.Application.Schedulers
{
    public class MemberSkillLevelUpTaskService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private Timer _timer;

        public MemberSkillLevelUpTaskService(IServiceScopeFactory _serviceScopeFactory)
        {
            serviceScopeFactory = _serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();

            // not using DI becouse this service is added as a singleton, and heist service and dbContext are scoped
            var heistService = scope
                .ServiceProvider
                .GetRequiredService<IHeistService>();
            await heistService.HeistMembersLevelUp();
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
