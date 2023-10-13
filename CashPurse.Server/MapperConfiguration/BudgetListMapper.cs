using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using Riok.Mapperly.Abstractions;

namespace CashPurse.Server.MapperConfiguration;

[Mapper]
public static partial class BudgetListMapper
{
    [MapProperty(nameof(CreateBudgetListRequest.Name), nameof(BudgetList.ListName))]
    [MapProperty(nameof(CreateBudgetListRequest.BudgetListOwnerId), nameof(BudgetList.OwnerId))]
    public static partial BudgetList MapCreateBudgetList(this CreateBudgetListRequest budgetList);

    [MapProperty(nameof(UpdateBudgetListRequest.BudgetListOwnerId), nameof(BudgetList.OwnerId))]
    public static partial BudgetList MapUpdateBudgetList(this UpdateBudgetListRequest budgetList);
}
