using RMDataManager.Library.Internal.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAccess
{
    public class UserData
    {
        public List<UserModel> GetUserById(string Id)
        {
            // TODO: set up AutoFac for DI, and code against interfaces instead of concrete classes
            SqlDataAccess sql = new SqlDataAccess();

            var p = new { Id };

            List<UserModel> data = sql.LoadData<UserModel, dynamic>("dbo.spUser_Lookup", p, "RMData");

            return data;
        }
    }
}
