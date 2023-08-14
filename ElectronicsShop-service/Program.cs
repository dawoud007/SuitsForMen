using System.Text;
using ElectronicsShop_service;
using ElectronicsShop_service.BusinessLogic;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.Models;
using ElectronicsShop_service.Repositories;
using ElectronicsShop_service.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ElectronicsShop_service.Options;
using Authentication.Infrastructure.Models;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();

//configure swagger gen options

builder.Services.ConfigureOptions<SwaggerGenOptionsSetup>();
builder.Services.AddSwaggerGen();

/*string connectionString = builder.Configuration.GetConnectionString("Default");*/
/*builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));*/

builder.Services.AddDbContext<ApplicationDbContext>(Options =>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});





builder.Services.AddValidatorsFromAssembly(typeof(BillValidations).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
/*builder.Services.AddScoped<ITokenGenerator, GenerateJwtToken>();*/




//////////////////////////
builder.Services.AddScoped<IClothRepository, ClothRepository>();
builder.Services.AddScoped<IClothUnitOfWork, ClothBusiness>();

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillUnitOfWork, BillBusiness>();


builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IShopUnitOfWork, ShopBusiness>();


/*builder.Services.AddScoped<IUserService, UserService>();
*/

Jwt jwt = new();
builder.Configuration.GetSection("Jwt").Bind(jwt);
builder.Services.AddSwaggerGen(options =>
{
    // Other Swagger configurations...
    options.OperationFilter<AuthorizeOperationFilter>();
});



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        ValidateIssuerSigningKey = true,
    };

});

/*builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});*/

//configure cors policy 



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder.WithOrigins("https://localhost:7040","https://richmanshops.azurewebsites.net")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("Content-Disposition") // Add this line
        .WithExposedHeaders("X-Custom-Header");   // Add any additional headers here

    });
});

builder.Services.AddCoreAdmin();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.UseCors("AllowAnyOrigin"); // Enable CORS before routing
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Map your library's controllers to the appropriate routes
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseStaticFiles();
app.MapControllers();
app.Run();