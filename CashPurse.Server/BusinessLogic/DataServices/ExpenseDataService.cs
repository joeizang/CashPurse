﻿using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.DataServices;

public static class ExpenseDataService
{

    public static async Task AddNewExpense(CashPurseDbContext context, Expense entity,
        CancellationToken token = default)
    {
        await context.Database.BeginTransactionAsync(token);
        try
        {
            context.Expenses.Add(entity);
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            await context.Database.CommitTransactionAsync(token);
        }
        catch (Exception e)
        {
            await context.Database.RollbackTransactionAsync(token);
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<bool> UpdateExpense(CashPurseDbContext context, Expense entity, UpdateExpenseRequest req,
        CancellationToken token = default)
    {
        await context.Database.BeginTransactionAsync(token);
        try
        {
            entity.ExpenseDate = req.ExpenseDate;
            entity.Name = req.Name;
            entity.Amount = req.Amount;
            // entity.ExpenseOwnerId = req.ExpenseOwnerId;
            entity.CurrencyUsed = req.CurrencyUsed;
            entity.Description = req.Description;
            entity.Notes = req.Notes;
            entity.ExpenseType = req.ExpenseType;
            entity.Id = req.ExpenseId;
            entity.ListId = req.BudgetListId;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            await context.Database.CommitTransactionAsync(token);
            return true;
        }
        catch (Exception e)
        {
            await context.Database.RollbackTransactionAsync(token);
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task DeleteExpense(CashPurseDbContext context, Expense entity)
    {
        entity.ExpenseDate = entity.ExpenseDate;
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
    public static int TotalExpenses { get; set; }

    public static int TotalCount(CashPurseDbContext context, string? userId = "")
    {
        return CompiledQueries.GetNumberOfUserExpensesAsync(context);
    }

    public static async Task<PagedResult<ExpenseIndexModel>> UserExpenses(CashPurseDbContext context, int pageNumber = 1)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(context);
        await foreach (var each in CompiledQueries.GetUserExpensesAsync(context, ""))
        {
            expenses.Add(each);
        }

        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, 
            pageNumber, (int)Math.Ceiling(TotalExpenses / (double)7), 1, 
                pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public static async Task<CursorPagedResult<List<ExpenseIndexModel>>> CursorPagedUserExpenses(CashPurseDbContext 
            context, string userId, DateOnly cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        await foreach (var each in CompiledQueries.GetCursorPagedUserExpensesAsync(context, 
                           userId, cursor))
        {
            expenses.Add(each);
        }

        if(expenses.Count == 0) return new CursorPagedResult<List<ExpenseIndexModel>>(DateOnly.FromDateTime(DateTime.Today), 
            expenses);
        return new CursorPagedResult<List<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public static ExpenseIndexModel UserExpenseById(CashPurseDbContext context, string userId, Guid expenseId)
    {
        var result = CompiledQueries.GetExpenseById(context, userId, expenseId);
        return result;
    }

    public static async Task<PagedResult<ExpenseIndexModel>> ExpenseTypeFilteredExpenses(CashPurseDbContext context,
        string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(context, userId);
        await foreach (var each in CompiledQueries.GetExpensesFilteredByExpenseTypeAsync(context,
                           pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, 
            pageNumber, (int)Math.Ceiling(TotalExpenses / (double)7), 7, 
                pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public static async Task<CursorPagedResult<IEnumerable<ExpenseIndexModel>>> CursorPagedTypeFilteredExpenses(
        CashPurseDbContext context,
        string userId, CursorPagedRequest cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(context, userId);
        await foreach (var each in CorrectQuery())
        {
            expenses.Add(each);
        }
        return new CursorPagedResult<IEnumerable<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);

        IAsyncEnumerable<ExpenseIndexModel> CorrectQuery()
        {
            return cursor.Cursor is null
                ? CompiledQueries.GetCursorPagedExpensesFilteredByExpenseTypeWithoutCursorAsync(
                    context, userId)
                : CompiledQueries.GetCursorPagedExpensesFilteredByExpenseTypeAsync(context,
                    cursor.Cursor.Value, userId);
        }
    }

    public static async Task<PagedResult<ExpenseIndexModel>> CurrencyUsedFilteredExpenses(CashPurseDbContext context,
        string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(context, userId);
        await foreach (var each in CompiledQueries.GetUserExpensesFilteredByCurrencyUsedAsync(context,
                           pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, 
            pageNumber, (int)Math.Ceiling(TotalExpenses / (double)7), 
            7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), 
            pageNumber > 1);
    }

    public static async Task<CursorPagedResult<IEnumerable<ExpenseIndexModel>>> CurrencyUsedCursorPagedFilteredExpenses(
        CashPurseDbContext context, string userId, DateOnly cursor
        )
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(context, userId);
        await foreach (var each in CompiledQueries
                           .GetCursorPagedUserExpensesFilteredByCurrencyUsedAsync(context, cursor, userId))
        {
            expenses.Add(each);
        }
        return new CursorPagedResult<IEnumerable<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public static decimal ExpenseTotalForLastMonth(CashPurseDbContext context, string userId)
    {
        var result = CompiledQueries.GetExpenseTotalForLast30DaysAsync(context, userId);
        var actualTotal = 0m;
        foreach (var each in result)
        {
            actualTotal += each.CurrencyUsed switch
            {
                Currency.USD => each.Amount * 1100,
                Currency.EUR => each.Amount * 1046,
                Currency.GBP => each.Amount * 1222,
                _ => each.Amount
            };
        }
        return actualTotal;
    }

    public static async Task<decimal> MeanSpendByDays(CashPurseDbContext context, string userId, int days)
    {
        var result = 0m;
        await foreach (var item in CompiledQueries.GetMeanSpendOverSpecifiedPeriod(
                           context, userId, days))
        {
            var temp = item.CurrencyUsed switch
            {
                Currency.USD => item.Amount * 995,
                Currency.EUR => item.Amount * 1046,
                Currency.GBP => item.Amount * 1222,
                _ => item.Amount
            };
            result += temp;
        }
        return result;
    }
}
