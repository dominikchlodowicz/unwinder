namespace unwinder.Services.AmadeusApiService.GetCityIataCode;

/// <summary>
/// Defines the contract for a service to retrieve the IATA code for a city based on a search keyword.
/// </summary>
public interface IGetCityIataCodeService
{
    /// <summary>
    /// Retrieves the IATA code for a city based on the provided search keyword.
    /// </summary>
    /// <param name="keyword">The keyword used to search for the city, such as the city name.</param>
    /// <returns>The IATA code of the city if found; otherwise, throws an exception.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the retrieval or processing of location data.</exception>
    public Task<string> GetCityIataCode(string keyword);
}