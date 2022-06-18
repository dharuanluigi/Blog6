using Blog.Data;
using Blog6.Configurations;
using Blog6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

var app = builder.Build();
LoadConfiguration(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void ConfigureAuthentication(WebApplicationBuilder builder)
{
  var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
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
}
void ConfigureMvc(WebApplicationBuilder builder)
{
  builder
  .Services
  .AddControllers()
  .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // avoid cicle dependencies btw classes
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault; // not deserelize when value is null
  });
}
void ConfigureServices(WebApplicationBuilder builder)
{
  builder.Services.AddDbContext<BlogDataContext>();
  builder.Services.AddTransient<TokenService>();
  builder.Services.AddTransient<SendEmailService>();
}
void LoadConfiguration(WebApplication app)
{
  Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
  Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
  Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");

  var smtp = new SmtpConfiguration();
  app.Configuration.GetSection("Smtp").Bind(smtp);
  Configuration.Smtp = smtp;
}