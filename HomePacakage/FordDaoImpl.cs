using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Client_Management_2._1
{
    class FordDaoImpl : AbstractDAO, FordDaoInter
    {

        //Add data
        public void Add(Ford ford, Description description)
        {
            SqlConnection connection = Connect();
            try
            {
                string query = "Insert Into Cars_tbl(Name, Phone, Vin, Model, Engine, Carplatenumber, Year)" +
                    " Values(@name, @phone, @vin, @model, @engine, @carplatenumber, @year)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", ford.Owner.Name);
                cmd.Parameters.AddWithValue("@phone", ford.Owner.Phone);
                cmd.Parameters.AddWithValue("@vin", ford.Vin);
                cmd.Parameters.AddWithValue("@model", ford.Model);
                cmd.Parameters.AddWithValue("@engine", ford.Engine);
                cmd.Parameters.AddWithValue("@carplatenumber", ford.CarPlateNumber);
                cmd.Parameters.AddWithValue("@year", ford.Year);
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                string query2 = "SELECT @@Identity";
                SqlCommand cmd2 = new SqlCommand(query2, connection);

                int id = Convert.ToInt32(cmd2.ExecuteScalar());

                if (description.FilePath == "")
                {

                    string query3 = "Insert into Description_tbl(CarId, Description, Datetime, Distance) Values(@carId,@desc,@datetime,@distance)";
                    SqlCommand cmd3 = new SqlCommand(query3, connection);
                    cmd3.Parameters.AddWithValue("carId", id);
                    cmd3.Parameters.AddWithValue("@desc", description.Desc);
                    cmd3.Parameters.AddWithValue("@datetime", description.DateTime);
                    cmd3.Parameters.AddWithValue("@distance", description.Distance);

                    cmd3.ExecuteNonQuery();
                    cmd3.Parameters.Clear();
                    connection.Close();
                }
                else
                {
                    Stream stream = File.OpenRead(description.FilePath);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    string query3 = "Insert into Description_tbl(CarId, Description, Datetime, Distance, FileName, Data, Extension) Values(@carId,@desc,@datetime, @distance, @fileName, @data, @extn)";
                    SqlCommand cmd3 = new SqlCommand(query3, connection);
                    cmd3.Parameters.AddWithValue("carId", id);
                    cmd3.Parameters.AddWithValue("@desc", description.Desc);
                    cmd3.Parameters.AddWithValue("@datetime", description.DateTime);
                    cmd3.Parameters.AddWithValue("@distance", description.Distance);
                    cmd3.Parameters.AddWithValue("@fileName", SqlDbType.VarChar).Value = description.FileName;
                    cmd3.Parameters.AddWithValue("@data", SqlDbType.VarBinary).Value = buffer;
                    cmd3.Parameters.AddWithValue("@extn", SqlDbType.Char).Value = description.Extension;

                    cmd3.ExecuteNonQuery();
                    cmd3.Parameters.Clear();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        //Delete data
        public void Delete(int id)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();

                string query = "Delete From Cars_tbl Where Id = @id";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }      

        //Update data
        public void Update(Ford ford, Description description, int id, int control)
        {
            SqlConnection connection = Connect();
            try
            {
                connection.Open();

                if (control == 1)
                {
                    string query = "Update Cars_tbl Set Name = @name, Phone = @phone,Vin = @vin, " +
                   "Model = @model, Engine = @engine, Carplatenumber = @carplatenumber, Year = @year Where Id = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlCommand cmd2;
                    SqlCommand cmd3;

                    cmd.Parameters.AddWithValue("@name", ford.Owner.Name);
                    cmd.Parameters.AddWithValue("@phone", ford.Owner.Phone);
                    cmd.Parameters.AddWithValue("@vin", ford.Vin);
                    cmd.Parameters.AddWithValue("@model", ford.Model);
                    cmd.Parameters.AddWithValue("@engine", ford.Engine);
                    cmd.Parameters.AddWithValue("@carplatenumber", ford.CarPlateNumber);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@year", ford.Year);


                    if (description.FilePath == "")
                    {
                        try
                        {
                            string query2 = "Insert into Description_tbl(CarId, Description, Datetime, Distance) Values(@carId,@desc,@datetime,@distance)";
                            cmd2 = new SqlCommand(query2, connection);
                            cmd2.Parameters.AddWithValue("carId", id);
                            cmd2.Parameters.AddWithValue("@desc", description.Desc);
                            cmd2.Parameters.AddWithValue("@datetime", DateTime.Now);
                            cmd2.Parameters.AddWithValue("@distance", description.Distance);

                            cmd.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        try
                        {
                            Stream stream = File.OpenRead(description.FilePath);
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);

                            string query3 = "Insert into Description_tbl(CarId, Description, Datetime, Distance, FileName, Data, Extension) Values(@carId, @desc, @datetime, @distance, @fileName, @data, @extn)";
                            cmd3 = new SqlCommand(query3, connection);
                            cmd3.Parameters.AddWithValue("carId", id);
                            cmd3.Parameters.AddWithValue("@desc", description.Desc);
                            cmd3.Parameters.AddWithValue("@datetime", DateTime.Now);
                            cmd3.Parameters.AddWithValue("@distance", description.Distance);
                            cmd3.Parameters.AddWithValue("@fileName", SqlDbType.VarChar).Value = description.FileName;
                            cmd3.Parameters.AddWithValue("@data", SqlDbType.VarBinary).Value = buffer;
                            cmd3.Parameters.AddWithValue("@extn", SqlDbType.Char).Value = description.Extension;

                            cmd.ExecuteNonQuery();
                            cmd3.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }

                else if (control == 2)
                {
                    string query = "Update Cars_tbl Set Name = @name, Phone = @phone,Vin = @vin, " +
                  "Model = @model, Engine = @engine, Carplatenumber = @carplatenumber, Year = @year Where Id = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlCommand cmd2;
                    SqlCommand cmd3;

                    cmd.Parameters.AddWithValue("@name", ford.Owner.Name);
                    cmd.Parameters.AddWithValue("@phone", ford.Owner.Phone);
                    cmd.Parameters.AddWithValue("@vin", ford.Vin);
                    cmd.Parameters.AddWithValue("@model", ford.Model);
                    cmd.Parameters.AddWithValue("@engine", ford.Engine);
                    cmd.Parameters.AddWithValue("@carplatenumber", ford.CarPlateNumber);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@year", ford.Year);

                    if (description.FilePath == "")
                    {
                        try
                        {
                            string query2 = "Update Description_tbl Set Description = @desc, Distance = @distance Where Datetime = @datetime";
                            cmd2 = new SqlCommand(query2, connection);
                            //cmd2.Parameters.AddWithValue("carId", id);
                            cmd2.Parameters.AddWithValue("@desc", description.Desc);
                            cmd2.Parameters.AddWithValue("@datetime", description.DateTime);
                            cmd2.Parameters.AddWithValue("@distance", description.Distance);

                            cmd.ExecuteNonQuery();
                            cmd2.ExecuteNonQuery();
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        try
                        {
                            Stream stream = File.OpenRead(description.FilePath);
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);

                            string query3 = "Update Description_tbl Set Description = @desc, Distance = @distance, FileName = @fileName, Data = @data, Extension = @extn  Where Datetime = @datetime";
                            cmd3 = new SqlCommand(query3, connection);
                            cmd3.Parameters.AddWithValue("carId", id);
                            cmd3.Parameters.AddWithValue("@desc", description.Desc);
                            cmd3.Parameters.AddWithValue("@datetime", description.DateTime);
                            cmd3.Parameters.AddWithValue("@distance", description.Distance);
                            cmd3.Parameters.AddWithValue("@fileName", SqlDbType.VarChar).Value = description.FileName;
                            cmd3.Parameters.AddWithValue("@data", SqlDbType.VarBinary).Value = buffer;
                            cmd3.Parameters.AddWithValue("@extn", SqlDbType.Char).Value = description.Extension;

                            cmd.ExecuteNonQuery();
                            cmd3.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
                else if (control == 3)
                {
                    try
                    {
                        string query = "Delete From Description_tbl Where Datetime = @datetime";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@datetime", description.DateTime);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        //Get data by id
        public List<Ford> GetById(int id)
        {
            List<Ford> fordList = new List<Ford>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                string query = "Select * From Cars_tbl Where Id=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader dataReader = command.ExecuteReader();
                              
                int id_;
                Owner owner = null;
                Description description = null;
                string vin;
                string model;
                string engine;
                string carPlateNumber;
                DateTime datetime;
                string year;

                while (dataReader.Read())
                {
                    owner = new Owner();
                    description = new Description();
                    datetime = new DateTime();

                    id_ = int.Parse(dataReader["Id"].ToString());
                    owner.Name = dataReader["Name"].ToString();
                    owner.Phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    carPlateNumber = dataReader["Carplatenumber"].ToString();
                    year = dataReader["Year"].ToString();
                    description.Desc = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];
                    description.Distance = dataReader["Distance"].ToString();
                    description.FileName = dataReader["FileName"].ToString();

                    description.DateTime = datetime;

                    fordList.Add(new Ford(id_, owner, vin, model, engine, carPlateNumber, year, description));
                }

                connection.Close();
            }
            catch (Exception ex)
            {                
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return fordList;
        }

        //Get data by name
        public List<Ford> GetByName(string name)
        {
            List<Ford> fordList = new List<Ford>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                string query = "Select Cars_tbl.*, Description_tbl.Description, Description_tbl.Datetime, Description_tbl.Distance, Description_tbl.FileName from Cars_tbl, Description_tbl Where Cars_tbl.Name = @name and Cars_tbl.Id = Description_tbl.CarId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                Owner owner = null;
                Description description = null;
                string vin;
                string model;
                string engine;
                string carPlateNumber;
                DateTime datetime;
                string year;

                while (dataReader.Read())
                {
                    owner = new Owner();
                    description = new Description();
                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    owner.Name = dataReader["Name"].ToString();                    
                    owner.Phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    carPlateNumber = dataReader["Carplatenumber"].ToString();
                    year = dataReader["Year"].ToString();
                    description.Desc = dataReader["Description"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];
                    description.Distance = dataReader["Distance"].ToString();
                    description.FileName = dataReader["FileName"].ToString();

                    description.DateTime = datetime;

                    fordList.Add(new Ford(id, owner, vin, model, engine, carPlateNumber, year, description));
                }

                connection.Close();
            }            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return fordList;
        }

        //Get data by Phone
        public List<Ford> GetByPhone(string phone)
        {
            List<Ford> fordList = new List<Ford>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                string query = "Select Cars_tbl.*, Description_tbl.Description, Description_tbl.Datetime, Description_tbl.Distance, Description_tbl.FileName from Cars_tbl, Description_tbl Where Cars_tbl.Phone = @phone and Cars_tbl.Id = Description_tbl.CarId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@phone", phone);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                Owner owner = null;
                Description description = null;
                string vin;
                string model;
                string engine;
                string carPlateNumber;
                DateTime datetime;
                string year;

                while (dataReader.Read())
                {
                    owner = new Owner();
                    description = new Description();
                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    owner.Name = dataReader["Name"].ToString();
                    owner.Phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    carPlateNumber = dataReader["Carplatenumber"].ToString();
                    year = dataReader["Year"].ToString();
                    description.Desc = dataReader["Description"].ToString();
                    description.FileName = dataReader["FileName"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];
                    description.Distance = dataReader["Distance"].ToString();

                    description.DateTime = datetime;

                    fordList.Add(new Ford(id, owner, vin, model, engine, carPlateNumber, year, description));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return fordList;
        }

        //Get data by vin
        public List<Ford> GetByVin(string vin_)
        {

            List<Ford> fordList = new List<Ford>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                String query = "Select Cars_tbl.*, Description_tbl.Description, Description_tbl.Datetime, Description_tbl.Distance, Description_tbl.FileName from Cars_tbl, Description_tbl Where Cars_tbl.Vin = @vin and Cars_tbl.Id = Description_tbl.CarId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@vin", vin_);
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                Owner owner = null;
                Description description = null;
                string vin;
                string model;
                string engine;
                string carPlateNumber;
                DateTime datetime;
                string year;

                while (dataReader.Read())
                {
                    owner = new Owner();
                    description = new Description();
                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    owner.Name = dataReader["Name"].ToString();                    
                    owner.Phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    carPlateNumber = dataReader["Carplatenumber"].ToString();
                    year = dataReader["Year"].ToString();
                    description.Desc = dataReader["Description"].ToString();
                    description.FileName = dataReader["FileName"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];
                    description.Distance = dataReader["Distance"].ToString();

                    description.DateTime = datetime;

                    fordList.Add(new Ford(id, owner, vin, model, engine, carPlateNumber, year, description));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return fordList;

        }

        //Get data by car plate plate number
        public List<Ford> GetByCarPlateNumber(string carPlateNumber_)
        {
            List<Ford> fordList = new List<Ford>();
            SqlConnection connection = Connect();

            try
            {
                connection.Open();
                string query = "Select Cars_tbl.*, Description_tbl.Description, Description_tbl.Datetime, Description_tbl.Distance, Description_tbl.FileName  from Cars_tbl, Description_tbl Where Cars_tbl.Carplatenumber like @carplatenumber and Cars_tbl.Id = Description_tbl.CarId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@carplatenumber", "%" + carPlateNumber_ + "%");
                SqlDataReader dataReader = command.ExecuteReader();

                int id;
                Owner owner = null;
                Description description = null;
                string vin;
                string model;
                string engine;
                string carPlateNumber;
                DateTime datetime;
                string year;

                while (dataReader.Read())
                {
                    owner = new Owner();
                    description = new Description();
                    datetime = new DateTime();

                    id = int.Parse(dataReader["Id"].ToString());
                    owner.Name = dataReader["Name"].ToString();                    
                    owner.Phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    carPlateNumber = dataReader["Carplatenumber"].ToString();
                    year = dataReader["Year"].ToString();
                    description.Desc = dataReader["Description"].ToString();
                    description.FileName = dataReader["FileName"].ToString();
                    datetime = (DateTime)dataReader["Datetime"];
                    description.Distance = dataReader["Distance"].ToString();

                    description.DateTime = datetime;

                    fordList.Add(new Ford(id, owner, vin, model, engine, carPlateNumber, year, description));
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return fordList;
        }

        //Get data by car plate plate number
        public DataTable GetByCPNumber(string carPlateNumber)
        {
            SqlConnection cn = Connect();
            cn.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("Select Cars_tbl.*, Description_tbl.Description, Description_tbl.Datetime, Description_tbl.Distance, Description_tbl.FileName  from Cars_tbl, Description_tbl Where Cars_tbl.Carplatenumber like @carplatenumber and Cars_tbl.Id = Description_tbl.CarId", cn);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@carplatenumber", "%" + carPlateNumber + "%");
            cn.Close();

            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        
    }
}
