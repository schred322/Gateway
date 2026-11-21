using Gateway.interfaces;
using Gateway.Providers;
using Gateway.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// 2. Register your services and your main application class
builder.Services.AddTransient<IJobServices, JobServices>();
builder.Services.AddTransient<ILogService, LogServices>(); 
builder.Services.AddTransient<DataServiceProvider>();
builder.Services.AddTransient<Gateway.Gateway>();
//builder.Services.AddTransient<IConfiguration>();

// 3. Build the host
using IHost host = builder.Build();

// 4. Resolve your target application class and run it
var run = host.Services.GetRequiredService<Gateway.Gateway>();
await run.RunAsync();