using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;

namespace AzureAdAuthentication
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApp(configRoot.GetSection("AzureAd"));
            services.AddInMemoryTokenCaches();

            services.AddAuthorization(
                options =>
                {
                    // By default, all incoming requests will be authorized according to the default policy.
                    options.FallbackPolicy = options.DefaultPolicy;
                }
            );

            services.AddRazorPages()
                    .AddRazorPagesOptions(
                         options =>
                         {
                             options.Conventions.AddPageRoute("/Inbox", "");
                         }
                     )
                    .AddMicrosoftIdentityUI();
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                }
            );
            app.Run();
        }
    }
}
