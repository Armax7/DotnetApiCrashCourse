using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetApiCourse.Services
{
    public class WriteOnFile : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string fileName = "File1.txt";
        private Timer timer;

        public WriteOnFile(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Write("Process initialized");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Write("Process terminated");
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Write($"Executing process: {DateTime.Now:dd/mm/YYYY hh:mm:ss}");
        }

        private void Write(string message)
        {
            var route = $@"{env.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(route, append: true))
            {
                writer.WriteLine(message);
            }
        }
    }
}