namespace CashPurse.Server.ApiModels;

public record CursorPagedRequest(DateTimeOffset Cursor);

public record GetExpenseByCurrencyRequest(int currency) : CursorPagedRequest(DateTimeOffset.Now);
