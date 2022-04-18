using System.Data.SqlClient;
using System.Windows.Forms;

namespace Client_Management_2._1
{
    abstract class AbstractDAO
    {
        public static string url;
        public static SqlConnection Connect()
        {
            //string url = @"Data Source=192.168.1.110, 1433;Initial Catalog=ClientDatabaseManagementSystem;UID=teacher;PWD=1234";
            //string url = @"Data Source=192.168.2.74, 1433;Initial Catalog=ClientDatabaseManagementSystem;UID=Wassa;PWD=1234";
            //string url= @"Data Source=FDRS2-PC;Initial Catalog=ClientDatabaseManagementSystem;Integrated Security=True";
            //string url = @"Data Source=TOSHIBA\SQLEXPRESS;Initial Catalog=ClientDatabaseManagementSystem;Integrated Security=True";


            SqlConnection connection = null;
            if (url != "")
            {
                connection = new SqlConnection(url);
                return connection;
            }
            else
            {
                return connection = null;
            }
                                                             
        }
    }
}
