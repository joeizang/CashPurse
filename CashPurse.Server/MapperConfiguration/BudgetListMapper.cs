using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using Riok.Mapperly.Abstractions;

namespace CashPurse.Server.MapperConfiguration;

[Mapper]
public static partial class BudgetListMapper
{
    [MapProperty(nameof(CreateBudgetListRequest.Items), nameof(BudgetList.BudgetItems))]
    [MapProperty(nameof(CreateBudgetListRequest.Name), nameof(BudgetList.ListName))]
    public static partial BudgetList MapCreateBudgetList(this CreateBudgetListRequest budgetList);

    public static partial BudgetList MapUpdateBudgetList(this UpdateBudgetListRequest budgetList);
}
