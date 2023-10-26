using Humanizer;
using Microsoft.JSInterop;
using System.Text.Json;

namespace Welfony.App.Common.Navigation
{
    public class NavigationHistory
    {
        private readonly IJSRuntime jsRuntime;

        public NavigationHistory(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public ValueTask Back() => jsRuntime.InvokeVoidAsync("window.history.back");

        public ValueTask<int> Count() => jsRuntime.InvokeAsync<int>("eval", "window.history.length");

        public async ValueTask<T?> CurrentState<T>() => JsonSerializer.Deserialize<T>(await jsRuntime.InvokeAsync<string>("eval", "window.history.state"));

        public ValueTask Forward() => jsRuntime.InvokeVoidAsync("window.history.forward");

        public async ValueTask<ScrollRestorationType> GetScrollRestoration()
        {
            return Enum.Parse<ScrollRestorationType>((await jsRuntime.InvokeAsync<string>("eval", $"window.history.scrollRestoration").ConfigureAwait(false)).Humanize(LetterCasing.Title));
        }

        public ValueTask Go(int index) => jsRuntime.InvokeVoidAsync("window.history.go", index);

        public ValueTask PushState<T>(T state, string url)
        {
            return jsRuntime.InvokeVoidAsync("window.history.pushState", JsonSerializer.Serialize(state), string.Empty, url);
        }

        public ValueTask ReplaceState<T>(T state, string url)
        {
            return jsRuntime.InvokeVoidAsync("window.history.replaceState", JsonSerializer.Serialize(state), string.Empty, url);
        }

        public ValueTask SetScrollRestoration(ScrollRestorationType scrollRestorationType)
        {
            return jsRuntime.InvokeVoidAsync("eval", $"window.history.scrollRestoration = '{Enum.GetName(scrollRestorationType)?.ToLower()}'");
        }
    }
}
