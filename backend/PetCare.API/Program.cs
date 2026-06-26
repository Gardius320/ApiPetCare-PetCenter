using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetCare.Application.Appointments.Commands.CreateAppointment;
using PetCare.Application.Common.Behaviors;
using PetCare.Domain.Identity;
using PetCare.Domain.Interfaces;
using PetCare.Infrastructure.Data;
using PetCare.Infrastructure.Hubs;
using PetCare.Infrastructure.Persistence;
using PetCare.API.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    
    options.Password.RequireDigit = true;           
    options.Password.RequireLowercase = true;      
    options.Password.RequireUppercase = true;       
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequiredLength = 8;            

    
    options.User.RequireUniqueEmail = true;       
})
.AddEntityFrameworkStores<PetsDbContext>() 
.AddDefaultTokenProviders();              


var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

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
        ValidIssuer = jwtIssuer,        
        ValidAudience = jwtAudience,     
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)) 
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe: Bearer {tu token aquí}"
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
            new string[] {}
        }
    });
});

builder.Services.AddValidatorsFromAssembly(
    typeof(PetCare.Application.Appointments.Commands.CreateAppointment.CreateAppointmentCommand).Assembly
);
builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>)
);

builder.Services.AddDbContext<PetsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPetRepository, PetsRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ISpeciesRepository, SpeciesRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<CreateAppointmentCommand>());

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowFrontend"); 

app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<PetsHub>("/petsHub");
app.MapControllers();

app.Run();