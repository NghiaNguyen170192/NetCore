using NetCore.Donation.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedConfiguration();

var apiBaseAddress = builder.Configuration.GetValue<string>("ApiBaseAddress") ?? "https://localhost:6001";

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped(sp => new HttpClient
{
	BaseAddress = new Uri(apiBaseAddress),
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();