using System;

namespace NetCore.Infrastructure.Database.Commons;

public abstract class BaseEvent
{
    /// <summary>
    /// TimeStamp when event created
    /// </summary>
    private DateTimeOffset EventRaisedTimeStamp { get; }

    protected BaseEvent()
    {
        EventRaisedTimeStamp = DateTimeOffset.UtcNow;
    }
}
