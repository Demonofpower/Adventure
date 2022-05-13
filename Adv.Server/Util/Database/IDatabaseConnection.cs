using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    interface IDatabaseConnection : IDisposable
    {
        void Init();
        void ExecuteNonQuery(string commandText, params Tuple<string, object>[] parameters);
        IDataReader ExecuteQuery(string commandText, params Tuple<string, object>[] parameters);
    }
}
