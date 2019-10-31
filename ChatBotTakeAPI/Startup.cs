using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol.Server;
using Serilog;
using Serilog.Core;
using Take.Blip.Client;

namespace ChatBotTakeAPI
{

    public class Startup : IStartable
    {
        private readonly ISender _sender;

        public Startup(ISender sender)
        {
            _sender = sender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Trace().CreateLogger();

            return Task.CompletedTask;
        }
    }
}
