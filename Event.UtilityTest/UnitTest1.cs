using EventBus.Abstractions;
using EventBus.Extensions;

namespace Event.UtilityTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var typeName = typeof(IEventHandle<>).GetGenericTypeName();
        Assert.Equal("IEventHandle<TEvent>", typeName);
    }
}