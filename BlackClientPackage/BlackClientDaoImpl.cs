using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Client_Management_2._1.BlackClientPackage
{
    class BlackClientDaoImpl : AbstractDAO, BlackClientDaoInter
    {
        public void Add(BlackClient blackClient)
        {
            SqlConnection connection = Connect();
            try
            {
                string query = "Insert Into BlackClient_tbl(Name, Phone, Vin, CPNumber, Description, Datetime)" +
                    " Values(@name, @phone, @vin, @cpnumber, @description, @datetime)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", blackClient.Name);
                cmd.Parameters.AddWithValue("@phone", blackClient.Phone);
                cmd.Parameters.AddWithValue("@vin", blackClient.Vin);
                cmd.Parameters.AddWithValue("@cpnumber", blackClient.Cpnumber);
                cmd.Parameters.AddWithValue("@description", blackClient.Description);
                cmd.Parameters.AddWithValue("@datetime", blackClient.DateTime);
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

        public void Delete(int id)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();

                string query = "Delete From BlackClient_tbl Where Id = @id";
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

        public List<BlackClient> GetByCPNumber(string cpnumber)
        {
            List<BlackClient> blackClientsList = new List<BlackClient>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  BlackClient_tbl Where  BlackClient_tbl.CPNumber like @cpnumber";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@cpnumber", "%" + cpnumber + "%");
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string vin;
                string phone;
                string cpnumber_;
                string description;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = dataReader["Name"].ToString();
                    phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    cpnumber_ = dataReader["CPNumber"].ToString();
                    description = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];

                    blackClientsList.Add(new BlackClient(id, name, phone, vin, cpnumber_, description, datetime));
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
            return blackClientsList;
        }

        public List<BlackClient> GetByName(string name)
        {
            List<BlackClient> blackClientsList = new List<BlackClient>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  BlackClient_tbl Where  BlackClient_tbl.Name = @name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name_;
                string vin;
                string phone;
                string cpnumber;
                string description;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name_ = dataReader["Name"].ToString();
                    phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    cpnumber = dataReader["CPNumber"].ToString();
                    description = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];

                    blackClientsList.Add(new BlackClient(id, name_, phone, vin, cpnumber, description, datetime));
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
            return blackClientsList;
        }

        public List<BlackClient> GetByPhone(string phone)
        {
            List<BlackClient> blackClientsList = new List<BlackClient>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  BlackClient_tbl Where  BlackClient_tbl.Phone = @phone";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@phone", phone);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string vin;
                string phone_;
                string cpnumber;
                string description;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = dataReader["Name"].ToString();
                    phone_ = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    cpnumber = dataReader["CPNumber"].ToString();
                    description = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];

                    blackClientsList.Add(new BlackClient(id, name, phone_, vin, cpnumber, description, datetime));
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
            return blackClientsList;
        }

        public List<BlackClient> GetByVin(string vin)
        {
            List<BlackClient> blackClientsList = new List<BlackClient>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select * from  BlackClient_tbl Where  BlackClient_tbl.Vin = @vin";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@vin", vin);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                string name;
                string vin_;
                string phone;
                string cpnumber;
                string description;
                DateTime datetime;

                while (dataReader.Read())
                {

                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    name = dataReader["Name"].ToString();
                    phone = dataReader["Phone"].ToString();
                    vin_ = dataReader["Vin"].ToString();
                    cpnumber = dataReader["CPNumber"].ToString();
                    description = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];

                    blackClientsList.Add(new BlackClient(id, name, phone, vin_, cpnumber, description, datetime));
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
            return blackClientsList;
        }

        public void Update(BlackClient blackClient)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();


                string query = "Update BlackClient_tbl Set Name = @name, Phone = @phone," +
               " Vin = @vin, CPNumber = @cpnumber, Description = @description Where Id = @id";

                SqlCommand cmd = new SqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@name", blackClient.Name);
                cmd.Parameters.AddWithValue("@phone", blackClient.Phone);
                cmd.Parameters.AddWithValue("@vin", blackClient.Vin);
                cmd.Parameters.AddWithValue("@cpnumber", blackClient.Cpnumber);
                cmd.Parameters.AddWithValue("@description", blackClient.Description);
                cmd.Parameters.AddWithValue("@id", blackClient.Id);

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
    }
}
