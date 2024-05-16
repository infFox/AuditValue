using System.Reflection;
using System.Text;

namespace WebApplication3;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IFreeSql fsql =
            new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.MySql,
                    "Server=127.0.0.1;Port=3306;User=root;Password=123456;Database=test_auditvalue;Charset=utf8mb4;SslMode=none;AllowPublicKeyRetrieval=True")
                .UseMonitorCommand(cmd => Console.WriteLine($"{cmd.CommandText}"))
                .UseNoneCommandParameter(true)
                .Build();

        fsql.Aop.AuditValue += (o, e) =>
        {
            if (e.Column.CsType == typeof(string))
            {
                var attr = e.Property.GetCustomAttribute<AesAttribute>(false);
                if (attr is not null)
                {
                    var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(e.Value.ToString()));
                    Console.WriteLine(
                        $"进行加密 \tauditType:{e.AuditValueType.ToString()} \tbefore:{e.Value} \tafter:{base64}");
                    e.Value = base64;
                }
            }
        };

        builder.Services.AddSingleton(fsql);
        builder.Services.AddFreeRepository(assemblies: AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapGet("/test", async (Repo repo) =>
        {
            await repo.InsertAsync(new Table
            {
                Password = "123456"
            });
        });

        app.MapGet("/test2", async (Repo repo) =>
        {
            using (var uow = repo.Orm.CreateUnitOfWork())
            {
                repo.UnitOfWork = uow;
                await repo.InsertAsync(new Table
                {
                    Password = "654321"
                });
            }
        });

        app.Run();
    }
}