using CashPurse.Server.Models;
using NodaTime;

namespace CashPurse.Server.ApiModels;
public sealed record CursorPagedResult<T>(DateOnly Cursor, T Data);

public record PagedResult<T>(List<T> Items, int TotalCount, int CurrentPage,
    int TotalPages, int PageSize = 7, int PageNumber = 1,
    bool HasNextPage = false, bool HasPreviousPage = false,
    FilterCriteria CurrentFilter = FilterCriteria.ByDate);
