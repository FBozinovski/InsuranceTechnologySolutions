using System.Threading.Channels;

namespace Claims.Service.BackgroundProcessing
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _queue =
            Channel.CreateUnbounded<Func<IServiceProvider, CancellationToken, Task>>();

        public async ValueTask WriteToQueueAsync(Func<IServiceProvider, CancellationToken, Task> workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem);

            await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<IServiceProvider, CancellationToken, Task>> ReadFromQueueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
