using unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

[TestFixture]
class TravelerTypeExtensionTests
{
    [Test]
    public void ToTravelerType_ArgumentIsNotValidType_ThrowsNewArgumentException()
    {
        string invalidTravelerType = "TEEN";

        var ex = Assert.Throws<ArgumentException>(() =>
        TravelerTypeExtension.ToTravelerType(invalidTravelerType));

        Assert.That(ex.Message, Is.EqualTo($"Invalid traveler type: {invalidTravelerType}"));
    }

}