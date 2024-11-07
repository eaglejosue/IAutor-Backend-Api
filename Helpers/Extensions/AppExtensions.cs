namespace IAutor.Api.Helpers.Extensions;

public static class AppExtensions
{
    public static void UseArchitectures(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseExceptionHandleMiddleware();

        app.MapEmailEndpoints();
        app.MapLoginEndpoints();
        app.MapOrderEndpoints();
        app.MapParamEndpoints();
        app.MapPaymentEndpoints();
        app.MapUserEndpoints();
        app.MapBookEndpoints();
        app.MapChapterEndpoints();
        app.MapQuestionEndpoints();
        app.MapThemeEndpoints();
        app.MapPlanEndpoints();
        app.MapIAEndpoints();
    }

    public static IApplicationBuilder UseExceptionHandleMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<ExceptionHandleMiddleware>();
}