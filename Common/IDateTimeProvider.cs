using System;

namespace Common
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow();
    }
}
