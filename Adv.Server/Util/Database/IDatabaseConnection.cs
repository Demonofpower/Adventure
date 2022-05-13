using System;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    interface IDatabaseConnection : IDisposable
    {
        void Init();
        void ExecuteNonQuery(string commandText, params Tuple<string, object>[] parameters);
        MySqlDataReader ExecuteQuery(string commandText, params Tuple<string, object>[] parameters);
    }
}
