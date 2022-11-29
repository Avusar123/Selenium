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

        Timer timer;

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
                OnTimerRunsOutAsync().Wait();
            }
        }

        private async Task OnTimerRunsOutAsync()
        {
            try
            {
                var accounts = await _usersRepo.GetAll(account => account.LastSpin.AddMinutes(15) < DateTime.Now && account.Balance < account.NeededCash);
                Console.WriteLine($"Accounts found: {accounts.Count}");
                await OnSpin(accounts.Take(15).ToArray());
                await _usersRepo.Save();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
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
            timer = new Timer(new TimerCallback(OnTimerRunsOut), null, 0, 10000);

            return Task.CompletedTask;
        }
    }
}
