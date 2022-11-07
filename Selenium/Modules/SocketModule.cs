using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;
using Websocket.Client.Models;
using static Selenium.Modules.SocketModule;

namespace Selenium.Modules
{
    public class SocketModule : ISocketModule
    {
        private SocketIO WebSocket;

        public delegate Task AsyncEventHandler<T>(object? sender, T e);

        public event AsyncEventHandler<CrashGame> CrashCreated;

        public event AsyncEventHandler<string> GameStateChaged;

        private ILogger<SocketModule> logger;

        private const string CrashUrl = "wss://crash.getx.pro";

        public SocketModule(ILogger<SocketModule> logger)
        {
            this.logger = logger;
        }

        public async Task Connect(string token)
        {
            WebSocket = new SocketIO(CrashUrl, new SocketIOOptions()
            {
                AutoUpgrade = true,
                EIO = 4,
                Query = new List<KeyValuePair<string, string>>()
                {
                     new("token", token)
                 }
            });

            WebSocket.OnConnected += OnConnected;
            await WebSocket.ConnectAsync();
            AddHandlingEvent("crash.onCreated", CrashCreated);
        }

        private void OnConnected(object? sender, EventArgs e)
        {
            logger.LogInformation("WebSocket connected");
            WebSocket.EmitAsync("crash.join");
        }

        private void AddHandlingEvent<EventArgs>(string eventname, AsyncEventHandler<EventArgs> deleg)
        {
            WebSocket.On(eventname, responce =>
            {
                deleg(this, responce.GetValue<EventArgs>());
            });
        }
    }

    public class CrashGame
    {
        public int gameId { get; set; }

        public string state { get; set; }

    }


    public interface ISocketModule
    {
        public event AsyncEventHandler<string> GameStateChaged;
        public Task Connect(string token);

        public event AsyncEventHandler<CrashGame> CrashCreated;
    }
}
