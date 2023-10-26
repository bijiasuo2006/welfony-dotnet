using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.ComponentModel;
using Welfony.App.Common;
using Welfony.App.Common.Configuration;
using Welfony.App.Common.Models;

namespace Welfony.App.Models
{
    public class AppState : AppStateBase
    {
        public int Count
        {
            get => count;
            set => Set(() => Count, ref count, value);
        }

        private int count;

        private readonly ConfigurationBase _config;

        public AppState(NavigationManager navigationManager, IJSRuntime jsRuntime, IWebAssemblyHostEnvironment environment, ConfigurationBase config)
            : base(navigationManager, jsRuntime, environment)
        {
            if (!Environment.IsProduction())
            {
                StateHasChanged.DebugMode = StateHasChangedDebugMode.Info;
            }
            StateHasChanged.DelayMode = StateHasChangedDelayMode.Debounce;
            StateHasChanged.DelayInterval = 100;

            _config = config;

            PropertyChanged += AppState_PropertyChanged;
        }

        private void AppState_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Console.WriteLine($"AppState.{e.PropertyName} changed.");
            StateHasChanged.Action();
        }
    }
}
