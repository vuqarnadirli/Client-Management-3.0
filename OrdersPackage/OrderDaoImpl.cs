using Client_Management_2._1.Orders;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Client_Management_2._1.OrdersPackage
{
    class OrderDaoImpl : AbstractDAO, OrderDaoInter
    {
        public void Add(Order order)
        {
            SqlConnection connection = Connect();
            try
            {
                string query = "Insert Into Orders_tbl(Name, Phone,  CPNumber, Orders, Datetime)" +
                    " Values(@name, @phone, @cpnumber, @orders, @datetime)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", order.Name);                
                cmd.Parameters.AddWithValue("@phone", order.Phone);               
                cmd.Parameters.AddWithValue("@cpnumber", order.Cpnumber);
                cmd.Parameters.AddWithValue("@orders", order.Orders);
                cmd.Parameters.AddWithValue("@datetime", order.DateTime);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

               
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public void Update(Order order)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();
                string query = "Update Orders_tbl Set Name = @name, Phone = @phone," +
               " CPNumber = @cpnumber Where Id = @id";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@name", order.Name);
                cmd.Parameters.AddWithValue("@phone", order.Phone);
                cmd.Parameters.AddWithValue("@cpnumber", order.Cpnumber);
                cmd.Parameters.AddWithValue("@id", order.Id);

                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(int id)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();

                string query = "Delete From Orders_tbl Where Id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }         
       
        public List<Order> GetByCPNumber(string cpnumber)
        {
            List<Order> ordersList = new List<Order>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  Orders_tbl Where  Orders_tbl.CPNumber like @cpnumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cpnumber", "%" + cpnumber + "%");
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string phone;
                string cpnumber_;
                string orders;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = (dataReader["Name"].ToString());
                    phone = (dataReader["Phone"].ToString());
                    cpnumber_ = dataReader["CPNumber"].ToString();
                    orders = (dataReader["Orders"].ToString());
                    datetime = (DateTime)dataReader["Datetime"];

                    ordersList.Add(new Order(id, name, phone, cpnumber_, orders, datetime));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return ordersList;
        }

        public List<Order> GetByName(string name)
        {
            List<Order> ordersList = new List<Order>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  Orders_tbl Where  Orders_tbl.Name = @name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name_;               
                string phone;
                string cpnumber;
                string orders;
                DateTime datetime;

                while (dataReader.Read())
                {
                    
                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name_ = (dataReader["Name"].ToString());
                    phone = (dataReader["Phone"].ToString());
                    cpnumber = dataReader["CPNumber"].ToString();
                    orders = (dataReader["Orders"].ToString());
                    datetime = (DateTime)dataReader["Datetime"];

                    ordersList.Add(new Order(id, name, phone, cpnumber, orders, datetime));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return ordersList;
        }

        public List<Order> GetByOrders(string orders)
        {
            List<Order> ordersList = new List<Order>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  Orders_tbl Where  Orders_tbl.Orders like @orders";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@orders", "%" + orders + "%");
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string phone;
                string cpnumber;
                string orders_;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = (dataReader["Name"].ToString());
                    phone = (dataReader["Phone"].ToString());
                    cpnumber = dataReader["CPNumber"].ToString();
                    orders_ = (dataReader["Orders"].ToString());
                    datetime = (DateTime)dataReader["Datetime"];

                    ordersList.Add(new Order(id, name, phone, cpnumber, orders_ , datetime));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return ordersList;
        }

        public List<Order> GetByPhone(string phone)
        {
            List<Order> ordersList = new List<Order>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  Orders_tbl Where  Orders_tbl.Phone = @phone";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@phone",phone );
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string phone_;
                string cpnumber;
                string orders;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = (dataReader["Name"].ToString());
                    phone_ = (dataReader["Phone"].ToString());
                    cpnumber = dataReader["CPNumber"].ToString();
                    orders = (dataReader["Orders"].ToString());
                    datetime = (DateTime)dataReader["Datetime"];

                    ordersList.Add(new Order(id, name, phone, cpnumber, orders, datetime));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return ordersList;
        }      
    }
}
