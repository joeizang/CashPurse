using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using Riok.Mapperly.Abstractions;

namespace CashPurse.Server.MapperConfiguration;

[Mapper]
public static partial class BudgetListItemMapper
{
    public static partial BudgetListItem MapCreateBudgetListItemRequest(this CreateBudgetListItemRequest item);
    
    public static partial List<BudgetListItem> MapCreateBudgetListItemsRequest(
        this List<CreateBudgetListItemRequest> items);

    public static partial UpdateBudgetListItemRequest MapUpdateBudgetListItemRequest(
        this UpdateBudgetListItemRequest item);
}
