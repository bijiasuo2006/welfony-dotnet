using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace Welfony.App.Common.Models
{
    public class AppStateBase : StateObservable
    {
        public NavigationManager NavigationManager { get; private set; }

        public IJSRuntime JSRuntime { get; set; }

        public IWebAssemblyHostEnvironment Environment { get; set; }

        public AppStateBase(NavigationManager navigationManager, IJSRuntime jsRuntime, IWebAssemblyHostEnvironment environment, StateHasChangedConfig? stateHasChangedConfig = null)
            : base(stateHasChangedConfig)
        {
            NavigationManager = navigationManager;
            JSRuntime = jsRuntime;
            Environment = environment;
        }
    }
}
