using Microsoft.Data.SqlClient;
using System.Data;
using UserPanel.Interfaces;
namespace UserPanel.Services.database
{
    public class SqlDataBase : DataBase
    {
        private readonly SqlConnection Connection;
        public SqlDataBase(IConfiguration configuration, string connectionKey = "dev")
        {
            Connection = new SqlConnection(configuration.GetConnectionString(connectionKey));
        }
        public override DataTable query(string command, string name)
        {

            DataTable dataTable = new DataTable(name);
            SqlCommand cmd = new SqlCommand(command, Connection);
            Connection.Open();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                using (da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Dispose();
                cmd = null;
                Connection.Close();
            }
            return dataTable;

        }
    }
}
