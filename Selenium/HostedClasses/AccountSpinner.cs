using Microsoft.Extensions.Hosting;
using Selenium.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.HostedClasses
{
    public class AccountSpinner : BackgroundService
    {
        IUsersRepo _usersRepo;

        IAPI api;

        private object _locker = new object();

        public AccountSpinner(IUsersRepo _usersRepo, IAPI api)
        {
            this._usersRepo = _usersRepo;
            this.api = api;
        }

        private void OnTimerRunsOut(object? state)
        {
            lock (_locker)
            {
                Task.WaitAll(Task.Run(OnTimerRunsOutAsync));
            }
        }

        private async Task OnTimerRunsOutAsync()
        {
            var accounts = await _usersRepo.GetAll(account => account.LastSpin < DateTime.Now && account.Balance < account.NeededCash);
            await OnSpin(accounts.ToArray());
            await _usersRepo.Save();
        }

        public async Task OnSpin(params UserData[] accounts)
        {
            foreach (var account in accounts)
            {
                var spinresponce = await api.StartFreeSpin(new SpinRequest() { Token = account.Token });

                account.Balance = (await api.GetBalance(new BalanceRequest() { Token = account.Token })).Balance;

                account.LastSpin = DateTime.Now;
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Timer timer = new Timer(new TimerCallback(OnTimerRunsOut), null, 0, 10000);

            return Task.CompletedTask;
        }
    }
}
