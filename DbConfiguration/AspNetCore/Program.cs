using System;

namespace AspCoreDbConfig
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            //*** add config ***
            builder.AddDbConfiguration(options =>
            {
                //
                //SELECT ID,Value FROM Configurations WHERE App = 'App1'
                //INSERT INTO Configurations(App,ID,Value) VALUES('App1',?,?)
                //UPDATE Configurations SET Value=? WHERE App = 'App1' AND ID=?
                //DELETE FROM Configurations WHERE App = 'App1' AND ID=?
                //
                options.Table = "Configurations";
                options.KeyColumn = "ID";
                options.ValueColumn = "Value";
                options.AppColumn = "App"; //optional
                options.AppId =  "App1"; //optional

                //options.KeyProtector = encrypt,decrypt
                //options.ValueProtector =  encrypt,decrypt

                //auto re-load configs
                options.TimestampId = "Monitor:Timestamp";
                options.WatchdogPeriod = TimeSpan.FromSeconds(30);
                options.WatchdogPeriodId = "Monitor:WatchdogPeriod";

                //options.SetConnection<Microsoft.Data.Sqlite.SqliteConnection>("");
            });
            //builder.Configuration.UseDbConfiguration<Microsoft.Data.Sqlite.SqliteConnection>("Data Source=MySampleDb.sqlite3;");

            var app = builder.Build();

            //*** load config ***
            app.Services.UseDbConfiguration<Microsoft.Data.Sqlite.SqliteConnection>(app.Configuration.GetConnectionString("DefaultConnection")!);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
