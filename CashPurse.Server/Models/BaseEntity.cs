using NodaTime;

namespace CashPurse.Server.Models;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow.ToLocalTime();
        UpdatedAt = DateTime.UtcNow.ToLocalTime();
    }
}
