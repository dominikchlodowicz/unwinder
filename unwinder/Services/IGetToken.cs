namespace unwinder.Services;

public interface IGetToken
{
    Task<string> GetAuthToken();
}