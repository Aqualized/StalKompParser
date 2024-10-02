namespace StalKompParser.StalKompParser.Common.Helpers
{
    public class ParallelHelper
    {
        public static async Task<List<TResult>> RunInParallelWithLimit<TItem, TResult>(
        IEnumerable<TItem> items,
        Func<TItem, Task<TResult>> taskFactory,
        int maxParallelism,
        CancellationToken token)
        {
            var semaphore = new SemaphoreSlim(maxParallelism);

            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync(token);
                try
                {
                    return await taskFactory(item);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();

            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }
    }
}
