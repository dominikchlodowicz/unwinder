using unwinder.Services.AmadeusApiService.FlightSearch.TypeHelpers;

[TestFixture]
public class DateTimeRangeTypeExtensionTests
{
    [Test]
    public void DateTimeToCorrectIsoFormat_ValidDateTime_NoException()
    {
        Assert.DoesNotThrow(() => DateTimeRangeTypeExtension.DateTimeToCorrectIsoFormat("2024-01-24", "10:30:00"));
    }

    [Test]
    public void DateTimeToCorrectIsoFormat_InvalidDateFormat_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            DateTimeRangeTypeExtension.DateTimeToCorrectIsoFormat("2024/01/24", "10:30:00"));
        Assert.That(ex.Message, Is.EqualTo("Departure date is not in the correct ISO format (YYYY-MM-DD)."));
    }

    [Test]
    public void DateTimeToCorrectIsoFormat_InvalidTimeFormat_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            DateTimeRangeTypeExtension.DateTimeToCorrectIsoFormat("2024-01-24", "10-30-00"));
        Assert.That(ex.Message, Is.EqualTo("Departure time is not in the correct ISO format (HH:MM:SS)."));
    }

    [Test]
    public void DateTimeToCorrectIsoFormat_DateIsInThePast_ThrowsArgumentException()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
            DateTimeRangeTypeExtension.DateTimeToCorrectIsoFormat("2024-01-01", "10:30:00"));
        Assert.That(ex.Message, Is.EqualTo("Departure DateTime is in the past."));
    }
}