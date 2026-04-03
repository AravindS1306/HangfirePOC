using Hangfire;
using Hangfire.SqlServer;
using HangfirePOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        PrepareSchemaIfNecessary = true,
    }));

builder.Services.AddHangfireServer();

HangfireTask hangfireTask = new HangfireTask();

var app = builder.Build();

int i = 0;
while (i < 10000)
{
    var client = app.Services.GetRequiredService<IBackgroundJobClient>();
    client.Enqueue(() => hangfireTask.DoWorkAsync());
    i++;
}
app.UseHangfireDashboard(); // /hangfire  or /dashboards to view the hangfire UI

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
