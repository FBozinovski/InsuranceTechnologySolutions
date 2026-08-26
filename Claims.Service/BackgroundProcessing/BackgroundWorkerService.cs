using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Claims.Service.BackgroundProcessing
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundWorkerService> _logger;

        public BackgroundWorkerService(IBackgroundTaskQueue taskQueue, IServiceScopeFactory scopeFactory, ILogger<BackgroundWorkerService> logger)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Func<IServiceProvider, CancellationToken, Task> workItem;

                try
                {
                    workItem = await _taskQueue.ReadFromQueueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // This fires only when the application is shutting down
                    break;
                }

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    await workItem(scope.ServiceProvider, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing a queued background work item.");
                }
            }
        }
    }
}
