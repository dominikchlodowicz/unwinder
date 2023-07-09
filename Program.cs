using unwinder.Services;
using unwinder.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

AmadeusFlightKeys flightServicekey = builder.Configuration.GetSection("AmadeusFlight").Get<AmadeusFlightKeys>();

builder.Services.AddControllersWithViews();

// register api url v1
builder.Services.AddHttpClient("AmadeusApiV1", httpClient =>
{
    // URI - combination or URL and URN string that represents resource on the web
    httpClient.BaseAddress = new Uri("https://test.api.amadeus.com/v1/");
});

// register api url v2
builder.Services.AddHttpClient("AmadeusApiV2", httpClient =>
{
    // URI - combination or URL and URN string that represents resource on the web
    httpClient.BaseAddress = new Uri("https://test.api.amadeus.com/v2/");
});

// Bearer Token
builder.Services.AddSingleton<IGetToken, GetToken>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>(); 
    var logger = sp.GetRequiredService<ILogger<GetToken>>();
    var clientId = flightServicekey.ServiceApiKey;
    var clientSecret = flightServicekey.ServiceApiSecretKey;
    return new GetToken(httpClientFactory, logger, clientId, clientSecret);
});

// Api Service
builder.Services.AddTransient<IAmadeusApiService, AmadeusApiService>(sp =>
{   
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var getToken = sp.GetRequiredService<IGetToken>();
    var logger = sp.GetRequiredService<ILogger<IAmadeusApiService>>();
    return new AmadeusApiService(httpClientFactory, logger, getToken);
});

// Amadeus API key service
// builder.Services.AddSingleton<AmadeusFlightSettings>(sp =>
// {

// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");



app.Run();

