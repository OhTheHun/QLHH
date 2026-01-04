using api_QLHH.Data;
using api_QLHH.Data.Interface;
using api_QLHH.FluentValidation;
using api_QLHH.Handlers.Command;
using api_QLHH.Handlers.Commands;
using api_QLHH.Handlers.Queries;
using api_QLHH.Services;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.DataContext;
using api_QLHH.SqlData.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ================= SERVICES =================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SqlContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// DI
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IValidator<KhachHang>, KhachHangValidator>();
builder.Services.AddScoped<IValidator<NhaCungCap>, NhaCungCapValidator>();

builder.Services.AddScoped<RunGetKhachHangQueryHandler>();
builder.Services.AddScoped<RunGetNhaCungCapQueryHandler>();
builder.Services.AddScoped<RunGetDanhMucSanPhamQueryHandler>();
builder.Services.AddScoped<RunGetSanPhamQueryHandler>();
builder.Services.AddScoped<RunGetKhoQueryHandler>();
builder.Services.AddScoped<RunGetChiTietKhoQueryHandler>();
builder.Services.AddScoped<GetUserByIdQueryHandler>();
builder.Services.AddScoped<LoginUserQueryHandler>();
builder.Services.AddScoped<UpdateUserCommandHandler>();
builder.Services.AddScoped<RegisterCommandHandler>();


var app = builder.Build();

// ================= MIDDLEWARE =================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowWebApp"); 

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();
