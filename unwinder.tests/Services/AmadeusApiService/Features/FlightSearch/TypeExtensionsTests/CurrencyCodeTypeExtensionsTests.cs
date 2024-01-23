using unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

[TestFixture]
public class CurrencyCodeTypeExtensionTests
{
    [Test]
    public void ToCurrencyCodeType_WithInvalidCurrency_ThrowsArgumentException()
    {
        string invalidCurrencyType = "NGN";  // Nigerian currency "Naira"

        var ex = Assert.Throws<ArgumentException>(() =>
        CurrencyCodeTypeExtension.ToCurrencyCodeType(invalidCurrencyType));
        Assert.That(ex.Message, Is.EqualTo($"Invalid currency code type: {invalidCurrencyType}"));
    }
}