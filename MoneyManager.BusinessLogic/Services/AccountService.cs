using System;
using MoneyManager.Data;
using MoneyManager.DTO;
using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Services
{
    public class AccountService
    {
        public readonly AppDBContext _db;

        public AccountService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<APIResponse<List<Account>>> GetAllAccounts(int userId)
        {
            var result = await _db.Accounts.Where(x=> x.UserId == userId).ToListAsync();

            if (result == null)
            {
                return new APIResponse<List<Account>> { Data = null, Message = "No Accounts found", StatusCode = 200 };
            }

            return new APIResponse<List<Account>> { Data = result, Message = "Accounts fetched successfully", StatusCode = 200 };
        }

        public async Task<APIResponse<Account>> PostAccount(AccountRequest req)
        {
            if (req == null)
            {
                return new APIResponse<Account> { StatusCode = 400, Message = "You must provide the details", Data = null };
            }

            var account = new Account()
            {
                AccountName = req.AccountName,
                UserId = req.UserId,
                Description = req.Description
            };

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            return new APIResponse<Account>
            {
                StatusCode = 201,
                Message = "Account added successfully",
                Data = account
            };
        }

        public async Task<APIResponse<Account>> UpdateAccount(int id, AccountRequest req)
        {
            if (req == null)
            {
                return new APIResponse<Account> { StatusCode = 400, Message = "You must provide the details", Data = null };
            }

            var account = await _db.Accounts.FindAsync(id);

            if (account == null)
            {
                return new APIResponse<Account> { StatusCode = 404, Message = "Account not found", Data = null };
            }

            account.AccountName = req.AccountName;
            account.Description = req.Description;

            await _db.SaveChangesAsync();

            return new APIResponse<Account>
            {
                StatusCode = 200,
                Message = "Account updated successfully",
                Data = account
            };
        }

        public async Task<APIResponse<Account>> DeleteAccount(int id)
        {
            var account = await _db.Accounts.FindAsync(id);

            if (account == null)
            {
                return new APIResponse<Account> { StatusCode = 404, Message = "Account not found", Data = null };
            }

            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync();

            return new APIResponse<Account>
            {
                StatusCode = 200,
                Message = "Account deleted successfully",
                Data = account
            };
        }
    }
}