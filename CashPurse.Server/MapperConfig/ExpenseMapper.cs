﻿using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.Models;
using Riok.Mapperly.Abstractions;

namespace CashPurse.Server;

[Mapper]
public static partial class ExpenseMapper
{
    public static ExpenseIndexModel MapToExpenseIndexModel(this Expense expense)
    {
        return new(expense.Name, expense.Description, expense.Amount, expense.ExpenseDate, expense.Id,
            expense.CurrencyUsed, expense.ExpenseType, expense.Notes!, expense.ExpenseOwnerId);
    }

    internal static partial Expense MapToCreateExpense(this CreateExpenseRequest request);

    internal static partial Expense MapToUpdateExpense(this UpdateExpenseRequest request);
}
