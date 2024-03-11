namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services)
        {
            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(
            this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}