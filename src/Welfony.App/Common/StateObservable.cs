namespace Welfony.App.Common
{
    public class StateObservable : ObservableObject
    {
        private bool disposedValue;
        private LoadingStatus loadingStatus;

        public LoadingStatus LoadingStatus
        {
            get => loadingStatus;
            set => Set(() => LoadingStatus, ref loadingStatus, value);
        }

        public StateHasChangedConfig StateHasChanged { get; set; }

        public StateObservable(StateHasChangedConfig? stateHasChangedConfig = null)
        {
            StateHasChanged = stateHasChangedConfig?.Clone(this) ?? new() { ObservableType = GetType() };
            StateHasChanged.Action = () =>
            {
                if (StateHasChanged.DebugMode != StateHasChangedDebugMode.Off)
                {
                    Console.WriteLine($"WARNING: {GetType().Name} called the empty StateHasChanged.Action method. Make sure to set `[YourViewModel].StateHasChanged.Action = StateHasChanged;` in OnInitializedAsync()");
                }
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StateHasChanged?.Dispose();
                }
                disposedValue = true;

                Dispose();
            }
        }
    }
}
