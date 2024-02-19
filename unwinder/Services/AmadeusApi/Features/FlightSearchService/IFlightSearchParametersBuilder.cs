using unwinder.Models.AmadeusApiServiceModels.FlightSearchModels;
using System.Collections.Generic;

namespace unwinder.Services.AmadeusApiService.FlightSearch;

/// <summary>
/// Defines a contract for building flight search parameters using the builder pattern.
/// </summary>
public interface IFlightSearchParametersBuilder
{
    /// <summary>
    /// Specifies the number of travelers for the flight search.
    /// </summary>
    /// <param name="numberOfTravelers">A list representing the number and type of travelers.</param>
    /// <returns>The builder instance for chaining additional configuration.</returns>
    public FlightSearchParametersBuilder BuildNumberOfTravelers(List<string> numberOfTravelers);

    /// <summary>
    /// Sets the departure date and time range for the flight search.
    /// </summary>
    /// <param name="departureDate">The departure date in ISO 8601 format (YYYY-MM-DD).</param>
    /// <param name="departureTime">The departure time in 24-hour format (HH:MM).</param>
    /// <returns>The builder instance for chaining additional configuration.</returns>
    public FlightSearchParametersBuilder BuildDateTimeRange(string departureDate, string departureTime);

    /// <summary>
    /// Defines the origin and destination airports for the flight search.
    /// </summary>
    /// <param name="originLocationCode">The IATA code of the origin airport.</param>
    /// <param name="destinationLocationCode">The IATA code of the destination airport.</param>
    /// <returns>The builder instance for chaining additional configuration.</returns>
    public FlightSearchParametersBuilder BuildOriginDestinations(string originLocationCode, string destinationLocationCode);

    /// <summary>
    /// Sets the currency code for pricing information in the flight search.
    /// </summary>
    /// <param name="currencyCode">The ISO currency code for the search results.</param>
    /// <returns>The builder instance for chaining additional configuration.</returns>
    public FlightSearchParametersBuilder BuildCurrencyCode(string currencyCode);

    /// <summary>
    /// Applies default values for various search parameters.
    /// </summary>
    /// <returns>The builder instance for chaining additional configuration.</returns>
    public FlightSearchParametersBuilder BuildDefaultValues();

    /// <summary>
    /// Finalizes the construction of the <see cref="FlightSearchParameters"/> object and returns it.
    /// </summary>
    /// <returns>The fully constructed <see cref="FlightSearchParameters"/> instance.</returns>
    public FlightSearchParameters Build();
}