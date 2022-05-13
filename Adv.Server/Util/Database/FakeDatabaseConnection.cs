using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Adv.Server.Util.Database
{
    class FakeDatabaseConnection : IDatabaseConnection
    {
        public void Dispose()
        {
            //Nothing to do
        }

        public void Init()
        {
            //Nothing to do
        }

        public void ExecuteNonQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            //Nothing to do
        }

        public IDataReader ExecuteQuery(string commandText, params Tuple<string, object>[] parameters)
        {
            var fakeDataReader = new FakeDataReader();

            return fakeDataReader;
        }
    }
}
