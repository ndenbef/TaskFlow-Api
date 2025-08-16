using System.Data;
using TaskFlowWebApi.Models;
using TaskFlowWebApi.Services;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Identity;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<StoreUserDbSettings>(
    builder.Configuration.GetSection("UserDataBase"));

builder.Services.AddIdentity<Users, UserRoles>().
    AddMongoDbStores < Users, UserRoles, Guid>
    (
        "mongodb://host.docker.internal:27017/",
        "TaskFlowUsers"
    )
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
