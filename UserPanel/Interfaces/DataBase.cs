using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace UserPanel.Interfaces
{
    public abstract class DataBase
    {
        private readonly DbConnection Connection;
        public abstract DataTable query(string command, string name);
    }
}
