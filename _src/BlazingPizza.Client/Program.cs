using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingPizza.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<OrderState>();

            // Add auth services
            //builder.Services.AddApiAuthorization();
            // t what if we want the user to be redirected back to the home page after they log out? To do that,
            //we can configure in Program.cs which path to direct the user to when they successfully log out.
            builder.Services.AddApiAuthorization<PizzaAuthenticationState>(options =>
            {
                options.AuthenticationPaths.LogOutSucceededPath = "";
            });
            //builder.Services.AddApiAuthorization(options =>
            //{
            //    options.AuthenticationPaths.LogInPath = "authentication/login";
            //    options.AuthenticationPaths.LogInCallbackPath = "authentication/login-callback";
            //    options.AuthenticationPaths.LogInFailedPath = "authentication/login-failed";
            //    options.AuthenticationPaths.LogOutPath = "authentication/logout";
            //    options.AuthenticationPaths.LogOutCallbackPath = "authentication/logout-callback";
            //    options.AuthenticationPaths.LogOutFailedPath = "authentication/logout-failed";
            //    options.AuthenticationPaths.LogOutSucceededPath = "authentication/logged-out";
            //    options.AuthenticationPaths.ProfilePath = "authentication/profile";
            //    options.AuthenticationPaths.RegisterPath = "authentication/register";
            //});

            //Register the OrdersClient as a typed client, with the underlying HttpClient configured with
            //the correct base address and the BaseAddressAuthorizationMessageHandler
            builder.Services.AddHttpClient<OrdersClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


            await builder.Build().RunAsync();
        }
    }
}
