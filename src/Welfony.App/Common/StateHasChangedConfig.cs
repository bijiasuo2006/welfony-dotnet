using System.Globalization;
using Welfony.App.Common.Threading;

namespace Welfony.App.Common
{
    public class StateHasChangedConfig : IDisposable
    {
        public Action Action
        {
            get
            {
                return DelayMode switch
                {
                    StateHasChangedDelayMode.Debounce => () => delayDispatcher.Debounce(DelayInterval, InternalAction),
                    StateHasChangedDelayMode.Throttle => () => delayDispatcher.Throttle(DelayInterval, InternalAction),
                    _ => InternalAction
                };
            }
            set
            {
                stateHasChangedAction = value;
            }
        }

        public int Count { get; set; }

        public StateHasChangedDebugMode DebugMode { get; set; } = StateHasChangedDebugMode.Off;

        public StateHasChangedDelayMode DelayMode { get; set; } = StateHasChangedDelayMode.Off;

        public int DelayInterval { get; set; } = 100;

        public Type? ObservableType { get; set; }

        private Action? stateHasChangedAction;

        private readonly DelayDispatcher delayDispatcher = new();

        public StateHasChangedConfig Clone(StateObservable observable)
        {
            return new()
            {
                Count = Count,

                DebugMode = DebugMode,
                DelayInterval = DelayInterval,
                DelayMode = DelayMode,

                ObservableType = observable?.GetType()
            };
        }

        internal void LogDelay()
        {
            Console.WriteLine($"{ObservableType?.Name}: StateHasChanged #{Count} called @ {DateTime.UtcNow.ToString("hh:mm:ss.fff", CultureInfo.InvariantCulture)} " +
                              $"{(DebugMode != StateHasChangedDebugMode.Off ? $"after {delayDispatcher.DelayCount} dropped calls." : "")}");

            if (DebugMode != StateHasChangedDebugMode.Tuning || DelayMode == StateHasChangedDelayMode.Off)
            {
                return;
            }

            var diffMiliseconds = DateTime.UtcNow.Subtract(delayDispatcher.TimerStarted).TotalMilliseconds;

            var entry = (DelayMode, DelayInterval, delayDispatcher.DelayCount, diffMiliseconds) switch
            {
                (StateHasChangedDelayMode.Debounce, _, _, < 50) => $"Performance: Debounce waited {diffMiliseconds}ms between calls. Delay was imperceptible.",

                (StateHasChangedDelayMode.Debounce, _, _, < 2000) => $"Performance: Debounce waited {diffMiliseconds}ms between calls. Delay was noticeable.",

                (StateHasChangedDelayMode.Throttle, < 50, _, _) => $"Performance: Throttle waited {DelayInterval}ms between calls. Delay was imperceptible.",

                (StateHasChangedDelayMode.Throttle, < 2000, < 10, _) => $"Performance: Throttle waited {DelayInterval}ms between calls," +
                                                                        $" but there were fewer than 10 calls dropped. Delay was imperceptible, but consider using Debounce instead.",

                (StateHasChangedDelayMode.Throttle, < 2000, _, _) => $"Performance: Throttle waited {DelayInterval}ms between calls." +
                                                                     $" If your goal is to reduce the number of repaints but fire them consistently, this is the right setting.",

                _ => $"Performance: {DelayMode} waited {(DelayMode == StateHasChangedDelayMode.Debounce ? diffMiliseconds : DelayInterval)}ms " +
                     $"between calls. Delay was unacceptable. Consider adding a visual 'waiting' indicator for the end user."
            };

            Console.WriteLine(entry);
        }

        internal void InternalAction()
        {
            ++Count;

            stateHasChangedAction?.Invoke();

            if (DebugMode == StateHasChangedDebugMode.Off)
            {
                return;
            }

            LogDelay();
        }

        public void Dispose()
        {
            delayDispatcher.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
