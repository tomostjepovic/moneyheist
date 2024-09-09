
using Microsoft.EntityFrameworkCore;
using MoneyHeist.Application.Interfaces;
using MoneyHeist.Application.Schedulers;
using MoneyHeist.Application.Services;
using MoneyHeist.Data.AppSettings;
using MoneyHeist.DataAccess;

namespace MoneyHeist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<ISkillService, SkillService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IHeistService, HeistService>();

            // Register the hosted service
            builder.Services.AddHostedService<HeistStartAndFinishTaskService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var Configuration = builder.Configuration;
            builder.Services.AddDbContext<RepoContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                    .UseSnakeCaseNamingConvention()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            builder.Services.Configure<HeistSettings>(builder.Configuration.GetSection("HeistSettings"));

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
}
