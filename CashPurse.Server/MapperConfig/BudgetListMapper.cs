using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using Riok.Mapperly.Abstractions;

namespace CashPurse.Server;

[Mapper]
public static partial class BudgetListMapper
{
    [MapProperty(nameof(CreateBudgetListModel.Items), nameof(BudgetList.BudgetItems))]
    [MapProperty(nameof(CreateBudgetListModel.Name), nameof(BudgetList.ListName))]
    public static partial BudgetList MapCreateBudgetList(this CreateBudgetListModel budgetList);

    public static partial BudgetList MapUpdateBudgetList(this UpdateBudgetListModel budgetList);
}
