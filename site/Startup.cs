namespace MockResponse.Site
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.Run();
        }
    }
}