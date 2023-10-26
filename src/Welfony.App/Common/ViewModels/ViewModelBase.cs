using Microsoft.AspNetCore.Components;
using Welfony.App.Common.Configuration;
using Welfony.App.Common.Models;

namespace Welfony.App.Common.ViewModels
{
    public class ViewModelBase<TConfig, TAppState> : StateObservable
        where TConfig : ConfigurationBase
        where TAppState : AppStateBase
    {
        public TConfig? Configuration { get; internal set; }
        public TAppState? AppState { get; internal set; }

        public ViewModelBase(TConfig? configuration = null, TAppState? appState = null, StateHasChangedConfig? stateHasChangedConfig = null)
            : base(stateHasChangedConfig)
        {
            Configuration = configuration;
            AppState = appState;
        }
    }
}
