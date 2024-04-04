using unwinder.Models.AmadeusApiServiceModels.KeyModels;
using unwinder.Controllers;
using unwinder.Services;
using unwinder.Services.AmadeusApiService;
using unwinder.Services.AmadeusApiService.FlightSearch;
using unwinder.Services.AmadeusApiService.GetLocation;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Services.AmadeusApiService.HotelSearch;
using unwinder.Services.AmadeusApiService.GetCityIataCode;
using unwinder.Services.HelperServices;
using Microsoft.VisualBasic;
using unwinder.Services.HelperServices;

var builder = WebApplication.CreateBuilder(args);

//enabling logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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

// register api url v3
builder.Services.AddHttpClient("AmadeusApiV3", httpClient =>
{
    // URI - combination or URL and URN string that represents resource on the web
    httpClient.BaseAddress = new Uri("https://test.api.amadeus.com/v3/");
});

// AMADEUS Api Flight Services DI - START

// Bearer Token
builder.Services.AddSingleton<IGetToken, GetToken>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var logger = sp.GetRequiredService<ILogger<GetToken>>();
    var clientId = flightServicekey.ServiceApiKey;
    var clientSecret = flightServicekey.ServiceSecretApiKey;
    return new GetToken(httpClientFactory, logger, clientId, clientSecret);
});

builder.Services.AddTransient<IFlightSearchService, FlightSearchService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var getToken = sp.GetRequiredService<IGetToken>();
    return new FlightSearchService(httpClientFactory, getToken);
});

builder.Services.AddTransient<IGetLocationService, GetLocationService>(sp =>
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var getToken = sp.GetRequiredService<IGetToken>();
    return new GetLocationService(httpClientFactory, getToken);
});

builder.Services.AddTransient<IGetCityIataCodeService, GetCityIataCodeService>(sp =>
{
    var getLocationService = sp.GetRequiredService<IGetLocationService>();
    return new GetCityIataCodeService(getLocationService);
});

builder.Services.AddSingleton<ICurrencyConversionService, CurrencyConversionService>(sp =>
{
    return new CurrencyConversionService();
});

// AMADEUS Api Flight Services DI - END

// AMADEUS Api Hotel Services DI - START

builder.Services.AddTransient<IHotelSearchListService, HotelSearchListService>(sp =>
{
    var getToken = sp.GetRequiredService<IGetToken>();
    var httpClientV1 = sp.GetRequiredService<IHttpClientFactory>();

    return new HotelSearchListService(httpClientV1, getToken);
});

builder.Services.AddTransient<IHotelSearchService, HotelSearchService>(sp =>
{
    var getToken = sp.GetRequiredService<IGetToken>();
    var httpClientV1 = sp.GetRequiredService<IHttpClientFactory>();
    var currencyConversionService = sp.GetRequiredService<ICurrencyConversionService>();

    return new HotelSearchService(httpClientV1, getToken, currencyConversionService);
});

// AMADEUS Api Hotel Services DI - END

// Controllers - START

// FlightSearchController
builder.Services.AddTransient<FlightSearchController>(sp =>
{
    var flightSearchService = sp.GetRequiredService<IFlightSearchService>();
    var getLocationService = sp.GetRequiredService<IGetLocationService>();
    var getCityIataCodeService = sp.GetRequiredService<IGetCityIataCodeService>();
    var logger = sp.GetRequiredService<ILogger<FlightSearchController>>();
    return new FlightSearchController(flightSearchService, getLocationService, getCityIataCodeService, logger);
});

// HotelSearchController
builder.Services.AddTransient<HotelSearchController>(sp =>
{
    var hotelSearchListService = sp.GetRequiredService<IHotelSearchListService>();
    var hotelSearchService = sp.GetRequiredService<IHotelSearchService>();
    var getCityIataCodeService = sp.GetRequiredService<IGetCityIataCodeService>();
    return new HotelSearchController(hotelSearchListService, hotelSearchService, getCityIataCodeService);
});

// Controllers - END

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
