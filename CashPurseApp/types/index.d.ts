type CreateExpenseModel = {
  expenseName: string
  expenseAmount: number
  expenseDescription: string
  expenseDate: string
  currencyUsed: Currency
  expenseTypeSelected: number
}

type Currency = 'USD' | 'EUR' | 'NGN' | 'GBP'

type ExpenseIndexModel = {
  expenseName: string
  expenseAmount: number
  expenseDescription: string
  expenseDate: string
  currencyUsed: Currency
  expenseId: string
  expenseType: ExpenseType
  notes?: string
  expenseOwnerEmail?: string
}

type ExpenseUpdateModel = {
  expenseName: string
  amount: number
  expenseDescription: string
  expenseDate: string
  currencyUsed: Currency
  expenseId: string
  expenseType: ExpenseType
  notes?: string
  expenseOwnerEmail?: string
}

type ExpenseDashboardSummary = {
  amount: number
  currencyUsed: Currency
}

type ExpenseType =
  | 'Tuition'
  | 'Rent'
  | 'Food'
  | 'Entertainment'
  | 'Other'
  | 'Utilities'
  | 'Transportation'
  | 'Clothing'
  | 'Medical'
  | 'Insurance'
  | 'Personal'
  | 'Debt'
  | 'Savings'
  | 'Gifts'
  | 'Donations'
  | 'Taxes'
  | 'Investments'
  | 'Salary'
  | 'Rental'
  | 'Subscriptions'
  | 'Groceries'
  | 'KidsExpenses'
  | 'CareerInvestment'
