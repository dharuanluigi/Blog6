using Blog.Data;
using Blog6.Configurations;
using Blog6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IO.Compression;
using System.Text;
using System.Text.Json.Serialization;

namespace Blog6.Extensions
{
  public static class AppExtension
  {
    public static WebApplicationBuilder LoadConfiguration(this WebApplicationBuilder builder)
    {
      Configuration.JwtKey = builder.Configuration.GetValue<string>("JwtKey");
      Configuration.ApiKeyName = builder.Configuration.GetValue<string>("ApiKeyName");
      Configuration.ApiKey = builder.Configuration.GetValue<string>("ApiKey");

      var smtp = new SmtpConfiguration();
      builder.Configuration.GetSection("Smtp").Bind(smtp);
      Configuration.Smtp = smtp;

      return builder;
    }

    public static WebApplicationBuilder ConfigureAuthentication(this WebApplicationBuilder builder)
    {
      var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtKey"));
      builder.Services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });

      return builder;
    }

    public static WebApplicationBuilder ConfigureMvc(this WebApplicationBuilder builder)
    {
      builder.Services.AddMemoryCache();
      builder.Services.AddResponseCompression(options =>
      {
        options.EnableForHttps = true;
        options.Providers.Add<GzipCompressionProvider>();
      });
      builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.SmallestSize);

      builder
      .Services
      .AddControllers()
      .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // avoid cicle dependencies btw classes
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault; // not deserelize when value is null
      });

      return builder;
    }

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
      var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");
      builder.Services.AddDbContext<BlogDataContext>(options => options.UseSqlServer(connectionString));
      builder.Services.AddTransient<TokenService>();
      builder.Services.AddTransient<SendEmailService>();

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      return builder;
    }
  }
}