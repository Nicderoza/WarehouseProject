using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Warehouse.Common.DTOs;
using Warehouse.Common.Mappings;
using Warehouse.Common.Security;
using Warehouse.Data.Models;
using Warehouse.Interfaces.IRepositories;
using Warehouse.Interfaces.IServices;
using Warehouse.Repository.Repositories;
using Warehouse.Service.Services;
using Warehouse.Services;
using Warehouse.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// ?? **Connessione al database**
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<WarehouseContext>(options =>
    options.UseSqlServer(connectionString));

// ?? **Configura l'autenticazione JWT**
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
    });

// ?? **Aggiungi autorizzazione**
builder.Services.AddAuthorization();

// ?? **Configura AutoMapper**
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ?? **Configura CORS**
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowFrontend", policy =>
  {
    policy.WithOrigins("http://localhost:4300") // Cambia se necessario
          .AllowAnyHeader()
          .AllowAnyMethod();
  });
});

// ?? **Registra i repository**
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

// ?? **Registra i servizi**
builder.Services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<Warehouse.Common.PasswordServices.PasswordService>();
builder.Services.AddScoped<JWT>();
builder.Services.AddScoped<AuthenticationService>();

// ?? **Registra i logger**
builder.Services.AddScoped<ILogger<ProductService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<ProductService>());
builder.Services.AddScoped<ILogger<CategoryService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CategoryService>());
builder.Services.AddScoped<ILogger<SupplierService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<SupplierService>());
builder.Services.AddScoped<ILogger<CityService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<CityService>());
builder.Services.AddScoped<ILogger<OrderService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<OrderService>());
builder.Services.AddScoped<ILogger<UserService>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<UserService>());
builder.Services.AddScoped<ILogger<GenericService<Products, DTOProduct>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Products, DTOProduct>>());
builder.Services.AddScoped<ILogger<GenericService<Categories, DTOCategory>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Categories, DTOCategory>>());
builder.Services.AddScoped<ILogger<GenericService<Suppliers, DTOSupplier>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Suppliers, DTOSupplier>>());
builder.Services.AddScoped<ILogger<GenericService<Cities, DTOCity>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Cities, DTOCity>>());
builder.Services.AddScoped<ILogger<GenericService<Orders, DTOOrder>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Orders, DTOOrder>>());
builder.Services.AddScoped<ILogger<GenericService<Users, DTOUser>>>(provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger<GenericService<Users, DTOUser>>());

// ?? **Controller e Razor**
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// ?? **Middleware**
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi(); // Se usi Swagger/OpenAPI in dev
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ?? **Attiva la policy CORS**
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
