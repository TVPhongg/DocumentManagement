using Microsoft.EntityFrameworkCore;
using DocumentManagement.Domain.Context;
using Microsoft.EntityFrameworkCore.Migrations;
using DocumentManagement.Domain.Entities;
using DocumentManagement.Application.Interfaces;
using DocumentManagement.Application.Services;
public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<MyDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
        });
        //C.o.r.s public API 
        builder.Services.AddCors(Option => Option.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        //dki dv auto mapper
        builder.Services.AddAutoMapper(typeof(Program));

        // dang ki life cyce DI
        builder.Services.AddScoped<IApprovalFlow, flow_Repository>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}