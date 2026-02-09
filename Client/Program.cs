using Client.AuthHandlers;
using Client.Components;
using Client.Extensions;
using Client.Services;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            //Inject Token store
            builder.Services.AddScoped<AuthLocalStorage>();
            builder.Services.AddTransient<AuthMessageHandler>();
            builder.Services.AddSingleton<TokenStore>(); 


            //Add http clients
            builder.Services.AddHttpClient("ApiAuth", client =>
            {
                client.BaseAddress = new Uri( builder.Configuration.GetApiBaseUrl());
            }).AddHttpMessageHandler<AuthMessageHandler>();

            builder.Services.AddHttpClient("Api", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration.GetApiBaseUrl());
            });

            //configure services
            builder.Services.AddScoped<IncidentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
