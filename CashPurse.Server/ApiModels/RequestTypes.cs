using System.Diagnostics.CodeAnalysis;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using NodaTime;

namespace CashPurse.Server.ApiModels;

public class CursorPagedRequest : IParsable<CursorPagedRequest>
{
    public DateOnly? Cursor { get; set; }
    public int? PageSize { get; set; }

    public static CursorPagedRequest Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CursorPagedRequest result)
    {
        result = null;

        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        var parts = s.Split(',');
        if (parts.Length != 2)
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var pageSize))
        {
            return false;
        }

        if (!DateOnly.TryParse(parts[0], out var cursor))
        {
            return false;
        }

        result = new CursorPagedRequest
        {
            PageSize = pageSize,
            Cursor = cursor
        };

        return true;
    }
}

public record CreateExpenseRequest(string Name, string Description, decimal Amount, DateOnly ExpenseDate,
    ExpenseType ExpenseType, Currency CurrencyUsed, string ExpenseOwnerId, string Notes);

public record UpdateExpenseRequest(Guid ExpenseId, string Name, string Description, decimal Amount, DateOnly ExpenseDate,
    ExpenseType ExpenseType, Currency CurrencyUsed, string ExpenseOwnerId, string Notes);
