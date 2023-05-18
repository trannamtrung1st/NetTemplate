namespace NetTemplate.Common.Web.Middlewares
{
    public static class RequestBufferingMiddleware
    {
        public static IApplicationBuilder UseRequestBuffering(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next();
            });
        }
    }

}
