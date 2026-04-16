using SwiftPOS.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add Razor Pages with NewtonsoftJson support
builder.Services.AddRazorPages()
    .AddNewtonsoftJson(); // Ab ye error nahi dega package install hone ke baad

// 2. Register MongoDB Context
builder.Services.AddSingleton<MongoDbContext>();

var app = builder.Build();

// 3. Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();