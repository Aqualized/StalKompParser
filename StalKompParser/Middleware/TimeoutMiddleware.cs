namespace StalKompParser.StalKompParser.Middleware
{
    public class TimeoutMiddleware
    {
        private readonly RequestDelegate _next;

        public TimeoutMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Query.TryGetValue("WaitTimeout", out var timeoutValue)
                && int.TryParse(timeoutValue, out int waitTimeoutSeconds))
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(waitTimeoutSeconds));
                context.RequestAborted = cts.Token;

                try
                {
                    context.Items["CancellationToken"] = cts.Token;
                    await _next(context);
                }
                catch (OperationCanceledException)
                {
                    context.Response.StatusCode = StatusCodes.Status408RequestTimeout;
                    await context.Response.WriteAsync("Request timed out");
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
