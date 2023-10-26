using Microsoft.AspNetCore.Components;
using System;
using Timer = System.Timers.Timer;

namespace Welfony.App.Common.Threading
{
    public class DelayDispatcher : IDisposable
    {
        private bool disposedValue;

        private Timer? timer;

        private readonly Dispatcher dispatcher = Dispatcher.CreateDefault();

        public int DelayCount { get; internal set; }

        public DateTime TimerStarted { get; internal set; }

        public void Debounce(int interval, Action action)
        {
            DelayCount++;

            if (timer is not null)
            {
                timer.Stop();
            }
            else
            {
                TimerStarted = DateTime.UtcNow;
            }

            timer = new Timer(interval);
            timer.Elapsed += (s, e) =>
            {
                if (timer is null)
                {
                    return;
                }

                timer?.Stop();
                timer = null;

                dispatcher.InvokeAsync(() => action.Invoke());

                DelayCount = 0;
            };

            timer.Start();
        }

        public void Throttle(int interval, Action action)
        {
            DelayCount++;

            if (timer is null)
            {
                timer = new Timer(interval);
                timer.Elapsed += (s, e) =>
                {
                    if (timer is null)
                    {
                        return;
                    }

                    timer?.Stop();
                    timer = null;

                    dispatcher.InvokeAsync(() => action.Invoke());

                    DelayCount = 0;
                };

                timer.Start();
                TimerStarted = DateTime.UtcNow;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    timer?.Dispose();
                    timer = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
