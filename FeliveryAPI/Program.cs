using Feliv_auth.Models;
using FeliveryAPI.Models;
using FeliveryAPI.Data;
using FeliveryAPI.Models;
using FeliveryAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<ElDbContext>(
          op =>
          {
              op.UseSqlServer(builder.Configuration.GetConnectionString("MyConn1"));
          }
      );
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ElDbContext>();
builder.Services.AddScoped<IRepository<MenuItem>, MenuItemRepoService>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepoService>();
builder.Services.AddScoped<IRepository<Order>, OrderRepoService>();
builder.Services.AddScoped<IRepository<Offer>, OfferRepoService>();
builder.Services.AddScoped<IStoreService,StoreService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = false;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
