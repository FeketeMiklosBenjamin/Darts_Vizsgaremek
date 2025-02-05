using DartsMobilApp.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Database
{
    public static class LoginDatabase
    {
        static SQLiteAsyncConnection? Database;

        static async Task Init()
        {
            if (Database != null)
            {
                return;
            }
            Database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            var result = await Database.CreateTableAsync<UserLoginData>();
        }

        public static async Task<List<UserLoginData>> GetAllItemsAsync()
        {
            await Init();
            return await Database.Table<UserLoginData>().ToListAsync();
        }

        public static async Task<UserLoginData> GetItemAsync(int id)
        {
            await Init();
            return await Database.Table<UserLoginData>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public static async Task<int> SaveItemAsync(UserLoginData UserLoginData)
        {
            await Init();
            if (UserLoginData.Id != 0)
            {
                return await Database.UpdateAsync(UserLoginData);
            }
            return await Database.InsertAsync(UserLoginData);
        }

        public static async Task<int> DeleteItemAsync(UserLoginData UserLoginData)
        {
            await Init();
            return await Database.DeleteAsync(UserLoginData);
        }
    }
}
