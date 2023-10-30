using System.Security.Principal;
using NodaTime;

namespace CashPurse.Server.Models;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly UpdatedAt { get; set; }

    public BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateOnly.FromDateTime(DateTime.Today);
        UpdatedAt = DateOnly.FromDateTime(DateTime.Today);
    }
}

public interface IPublicClaimsPrincipal
{
    // Retrieve the identity object
    IIdentity? Identity { get; }

    // Perform a check for a specific role
    bool IsInRole(string role);
}
