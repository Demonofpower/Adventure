using System;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    class FakeDatabaseConnection : IDatabaseConnection
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public void ExecuteNonQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            throw new NotImplementedException();
        }

        public MySqlDataReader ExecuteQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
