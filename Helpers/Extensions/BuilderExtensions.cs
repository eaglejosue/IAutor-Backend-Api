using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using QuestPDF.Infrastructure;

namespace IAutor.Api.Helpers.Extensions;

public static class BuilderExtensions
{
    public static void AddArchitectures(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration as IConfiguration;
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCors();
        builder.Services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            o.JsonSerializerOptions.MaxDepth = 3;
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            o.JsonSerializerOptions.WriteIndented = true;
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        
        builder.Services.AddDbContext<IAutorDb>(o => o.UseNpgsql(config.GetConnectionString("IAutorDb")));
        //builder.Services.AddDbContext<IAutorDb>(o => o.UseSqlite("DataSource=IAutor.db;Cache=Shared", b => b.MigrationsAssembly("IAutor.Api")));

        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        var azureBlobStorageConnString = "DefaultEndpointsProtocol=https;AccountName=blobfilesiautor;AccountKey=Pe0MHLxUbwGlyxLGaZ0yKsevjBcwMCIw0kywS3LX7m4g0PAWvTs2NOhsIK1BqeGQhxFlOEWlYpny+ASt2cnsxg==;EndpointSuffix=core.windows.net";
        builder.Services.AddSingleton<IAzureBlobServiceClient>(new AzureBlobServiceClient(new BlobServiceClient(config.GetSection("AzureBlobStorageConnString")?.Value ?? azureBlobStorageConnString)));

        var awsRegion = config.GetSection("Aws:Region")?.Value;
        var awsRegionEndpoint = RegionEndpoint.USEast1;
        if (!string.IsNullOrWhiteSpace(awsRegion))
            awsRegionEndpoint = RegionEndpoint.GetBySystemName(awsRegion);

        builder.Services.AddSingleton<IAmazonS3StorageManager>(new AmazonS3StorageManager(
            new AmazonS3Client(config.GetSection("Aws:AccessKey").Value, config.GetSection("Aws:SecretKey").Value, awsRegionEndpoint),
            config.GetSection("Aws:BucketName")?.Value ?? "iautor-assets-dev",
            config)
        );

        builder.AddSwagger();
        builder.AddSecurity(config);
        builder.AddCors(config);
        builder.Logging.SetMinimumLevel(LogLevel.Error).AddConsole();
        builder.InjectServiceDependencies(config);

        QuestPDF.Settings.License = LicenseType.Community;
    }

    private static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "IAutor.Api",
            });
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private static void AddSecurity(this WebApplicationBuilder builder, IConfiguration config)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var token = Encoding.UTF8.GetBytes(config.GetSection("Security:Token").Value!);
        var key = new SymmetricSecurityKey(token);
        var issuer = config.GetSection("Security:Issuer").Value!;
        var audience = config.GetSection("Security:Audience").Value!;

        builder.Services.Configure<Security>(config.GetSection("Security"))
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = config.GetSection("Google:ClientId").Value!;
                options.ClientSecret = config.GetSection("Google:ClientSecret").Value!;
            });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("Get", p => p.RequireRole("Get"))
            .AddPolicy("Create", p => p.RequireRole("Create"))
            .AddPolicy("Update", p => p.RequireRole("Update"))
            .AddPolicy("Delete", p => p.RequireRole("Delete"))
            .AddPolicy("GetIncomes", p => p.RequireRole("GetIncomes"))
            .AddPolicy("Admin", p => p.RequireRole("Admin"))
            .AddPolicy("Influencer", p => p.RequireRole("Influencer"))
            .AddPolicy("Agent", p => p.RequireRole("Agent"));
    }

    private static void AddCors(this WebApplicationBuilder builder, IConfiguration config)
    {
        var myAllowSpecificOrigins = "_MyAllowSubdomainPolicy";
        var domainsAllowed = config.GetSection("AllowedDomain");
        var origins = domainsAllowed.AsEnumerable().Where(p => p.Value is not null).Select(p => p.Value!).ToArray();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: myAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins(origins)
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
                          .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
                });
        });
    }

    private static void InjectServiceDependencies(this WebApplicationBuilder builder, IConfiguration config)
    {
        builder.Services.AddScoped<INotificationService, NotificationService>();
        builder.Services.AddScoped<ILoginService, LoginService>();
        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOwnerService, OwnerService>();
        builder.Services.AddScoped<IParamService, ParamService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IQuestionService, QuestionService>();
        builder.Services.AddScoped<IChapterService, ChapterService>();
        builder.Services.AddScoped<IPlanService, PlanService>();
        builder.Services.AddScoped<IIAService, IAService>();
        builder.Services.AddScoped<IPDFService, PDFService>();

        builder.Services.Configure<IuguConfig>(config.GetSection("IuguConfig"));
        builder.Services.AddScoped<IIuguIntegrationService, IuguIntegrationService>();

        builder.Services.Configure<Smtp>(config.GetSection("Smtp"));
        builder.Services.Configure<Config>(config.GetSection("Config"));
        builder.Services.AddScoped<IEmailSender, EmailSender>();
    }
}