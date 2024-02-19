namespace unwinder.Services;

public interface IGetToken
{
    /// <summary>
    /// Asynchronously retrieves a new authentication token.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{String}"/> that represents the asynchronous operation and contains the authentication token as its result.
    /// </returns>
    /// <remarks>
    /// This method ensures the token is valid and refreshes it if necessary.
    /// It uses a thread-safe mechanism to avoid concurrent refreshes.
    /// </remarks>
    Task<string> GetAuthToken();
}