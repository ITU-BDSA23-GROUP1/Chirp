using Chirp.CLI;

public class UnitTest1
{
    [Fact]
    public void getUnixTimeTest()
    {
        DateTime dateTime = new DateTime(2023, 9, 10, 12, 30, 0);

        long expectedUnixTime = 1694349000;

        Assert.Equal(expectedUnixTime, getUnixTime(dateTime));

    }
}