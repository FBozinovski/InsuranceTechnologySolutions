namespace Claims.Service.BackgroundProcessing
{
    public interface IBackgroundTaskQueue
    {
        ValueTask WriteToQueueAsync(Func<IServiceProvider, CancellationToken, Task> workItem);

        ValueTask<Func<IServiceProvider, CancellationToken, Task>> ReadFromQueueAsync(CancellationToken cancellationToken);
    }
}
