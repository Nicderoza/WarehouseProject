using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Common.DTOs;
using Warehouse.Common.Mappings;
using Warehouse.Common.Security;
using Warehouse.Data;
using Warehouse.Data.Interfaces;
using Warehouse.Data.Models;
using Warehouse.Data.Repositories;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Repository.Repositories;
using Warehouse.Service.Services;
using Warehouse.Services.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<WarehouseContext>(options =>
    options.UseSqlServer(connectionString));

var jwtSettingSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSetting>(jwtSettingSection);

var jwtSetting = jwtSettingSection.Get<JwtSetting>();
var key = Encoding.ASCII.GetBytes(jwtSetting.Key);

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    ClockSkew = TimeSpan.FromMinutes(10)
  };
  options.MapInboundClaims = false;
});

builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
  });

});

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>(); 
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUsersSuppliersRepository, UsersSuppliersRepository>();
builder.Services.AddScoped<IFormStructureRepository, FormStructureRepository>();

builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartItemService, CartItemService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<IFormStructureRepository, FormStructureRepository>();
builder.Services.AddScoped<IFormStructureService, FormStructureService>();
builder.Services.AddScoped<ILogger<ProductService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<ProductService>());
builder.Services.AddScoped<ILogger<CategoryService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CategoryService>());
builder.Services.AddScoped<ILogger<SupplierService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<SupplierService>());
builder.Services.AddScoped<ILogger<CityService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CityService>());
builder.Services.AddScoped<ILogger<OrderService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<OrderService>());
builder.Services.AddScoped<ILogger<UserService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<UserService>());
builder.Services.AddScoped<ILogger<CartItemService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CartItemService>());
builder.Services.AddScoped<ILogger<RegistrationService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<RegistrationService>());
builder.Services.AddScoped<ILogger<GenericService<Products, DTOProduct>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Products, DTOProduct>>());
builder.Services.AddScoped<ILogger<GenericService<Categories, DTOCategory>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Categories, DTOCategory>>());
builder.Services.AddScoped<ILogger<GenericService<Suppliers, DTOSupplier>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Suppliers, DTOSupplier>>());
builder.Services.AddScoped<ILogger<GenericService<Cities, DTOCity>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Cities, DTOCity>>());
builder.Services.AddScoped<ILogger<GenericService<Orders, DTOOrder>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Orders, DTOOrder>>());
builder.Services.AddScoped<ILogger<GenericService<Users, DTOUser>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Users, DTOUser>>());
builder.Services.AddScoped<ILogger<GenericService<Cart, DTOCartResponse>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Cart, DTOCartResponse>>());
builder.Services.AddScoped<ILogger<CartService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CartService>());



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
