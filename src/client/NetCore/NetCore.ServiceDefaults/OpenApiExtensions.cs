using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace NetCore.ServiceDefaults;

public static partial class Extensions
{
    public static IHostApplicationBuilder AddDefaultOpenApi(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;
        var modelBuilder = new ODataConventionModelBuilder();

        // Use default OpenAPI/Swagger configuration. No JWT security defined by default.
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers()
            .AddOData(options =>
            {
                options.Filter().Expand()
                    .Select().OrderBy().SetMaxTop(100).SkipToken()
                    .AddRouteComponents("odata", modelBuilder.GetEdmModel());
                options.EnableNoDollarQueryOptions = true;
            });

        return builder;
    }

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}