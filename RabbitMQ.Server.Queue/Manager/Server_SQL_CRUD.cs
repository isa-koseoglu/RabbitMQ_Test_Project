
using RabbitMQ.Common.Model;
using System.Data;
using System.Data.SqlClient;


namespace RabbitMQ.Server.Queue.Manager
{
    
    public class Server_SQL_CRUD
    {
        private  SqlConnection connection;
        private readonly string connecitonString = "Server=DESKTOP-NF9CO7S;Database=RabbitMQ_TestProject_DB;User Id=sa;Password=1;MultipleActiveResultSets=true";
        
        
        public Server_SQL_CRUD()
        {
            CheckConneciton();
        }

        private static Server_SQL_CRUD obj;
        
        public static Server_SQL_CRUD Obj()
        {
            if (obj == null)
            {
                obj = new Server_SQL_CRUD();
            }
            return obj;
        }
        private void CheckConneciton()
        {
            if (connection == null)
            {
                connection = new  SqlConnection(connecitonString);
            }
            connection.Open();
        }
        public GetVersionModel GetById(int branchCODE)
        {
            string query = "SELECT top 1 *FROM Test_Table where BranchCode=@p1";
            using (SqlCommand cmd = new SqlCommand(query,connection))
            {
                cmd.Parameters.AddWithValue("@p1", branchCODE);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new GetVersionModel
                        {
                            BranchCode = (int)reader["BranchCode"],
                            StationNo = (int)reader["StationNo"],
                            ApiVers = reader["ApiVers"].ToString(),
                            HtmlVers = reader["HtmlVers"].ToString(),
                            GetMmeesage = reader["ClientMessage"].ToString()
                        };
                    }
                }
            }

            return null;
        }



    }
}
