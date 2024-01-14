using unwinder.Helpers;

[TestFixture]
public class FlightSearchHelpersTests
{
    [Test]
    public void RepeatString_WithValidArguments_RepeatsTheString()
    {
        string validArgument = "Test";

        var ex = FlightSearchHelpers.RepeatString(validArgument, 2);

        var expectedResult = new List<string> { validArgument, validArgument };

        Assert.That(ex, Is.EqualTo(expectedResult));
    }

    // TODO: fix that test because it's not what the TryParse does
    [Test]
    public void ConvertIsoDateStringToDate_WithInValidIsoStringArgument_ThrowsArgumentException()
    {
        var invalidArgument = "test";

        var ex = Assert.Throws<ArgumentException>(() =>
        FlightSearchHelpers.ConvertIsoDateStringToDate(invalidArgument));

        Assert.That(ex.Message, Is.EqualTo("Passed string argument is not a valid ISO date."));
    }

    [Test]
    public void ConvertIsoDateStringToDate_WithValidIsoStringArgument_ReturnsParsedDate()
    {
        throw new NotImplementedException();
    }
}