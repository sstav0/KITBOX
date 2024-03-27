using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using UraniumUI;
using Microsoft.Maui.LifecycleEvents;
using CommunityToolkit.Maui;

namespace Kitbox_project
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            #if WINDOWS
                    builder.ConfigureLifecycleEvents(events =>
                    {
                        events.AddWindows(windowsLifecycleBuilder =>
                        {
                            windowsLifecycleBuilder.OnWindowCreated(window =>
                            {
                                window.ExtendsContentIntoTitleBar = false;
                                var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                                var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                                var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                                switch (appWindow.Presenter)
                                {
                                    case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                        overlappedPresenter.SetBorderAndTitleBar(true, true); //set both false for "kiosk mode"
                                        overlappedPresenter.Maximize();
                                        break;
                                }
                            });
                        });
                    });
            #endif

#if DEBUG
            builder.Logging.AddDebug();
#endif


            return builder.Build();
        }
    }
}
