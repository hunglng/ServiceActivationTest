

namespace ServiceActivationTest.Services
{
    public class TimerService(ILogger<TimerService> logger, IWeatherForecastService weatherForecastService) 
        : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private int _executionCount = 0;
        private Timer? _timer;

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("{Service} {ServiceId} is running.", nameof(TimerService), weatherForecastService.GetWeatherForecastServiceId());
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return _completedTask;
        }

        private void DoWork(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);

            logger.LogInformation(
                "{Service} {ServiceId} is working, execution count: {Count:#,0}",
                nameof(TimerService), weatherForecastService.GetWeatherForecastServiceId(),
                count);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "{Service} {ServiceId} is stopping.", nameof(TimerService), weatherForecastService.GetWeatherForecastServiceId());

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }
    }

    public class TimerServiceV2(ILogger<TimerServiceV2> logger, IWeatherForecastService weatherForecastService)
        : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private int _executionCount = 0;
        private Timer? _timer;

        public async ValueTask DisposeAsync()
        {
            if (_timer is IAsyncDisposable timer)
            {
                await timer.DisposeAsync();
            }

            _timer = null;

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("{Service} {ServiceId} is running.", nameof(TimerServiceV2), weatherForecastService.GetWeatherForecastServiceId());
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return _completedTask;
        }

        private void DoWork(object? state)
        {
            int count = Interlocked.Increment(ref _executionCount);

            logger.LogInformation(
                "{Service} {ServiceId} is working, execution count: {Count:#,0}",
                nameof(TimerServiceV2), weatherForecastService.GetWeatherForecastServiceId(),
                count);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
            "{Service} {ServiceId} is stopping.", nameof(TimerServiceV2), weatherForecastService.GetWeatherForecastServiceId());

            _timer?.Change(Timeout.Infinite, 0);

            return _completedTask;
        }
    }
}
