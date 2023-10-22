using Microsoft.AspNetCore.Server.IIS.Core;

namespace unwinder.Helpers;

public static class ErrorMessages
{
    public const string ApiResponseEmpty = "Api response is empty.";
    public const string UnexpectedJsonStructure = "Unexpected JSON structure: {0}";
    public const string DeserializeError = "Failed to deserialize API response.";
    public const string RecievedErrorCode = "Received {0} from the server.";
}
