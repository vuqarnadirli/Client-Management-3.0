﻿
using Client_Management_2._1.BlackClientPackage;
using Client_Management_2._1.Orders;
using Client_Management_2._1.OrdersPackage;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Text;

namespace Client_Management_2._1
{
    public partial class ClientManagementForm : Form
    {
        public ClientManagementForm()
        {
            InitializeComponent();
        }

        #region Home
        //Get all datas
        public void GetAlls()
        {
            try
            {
                DataSetHomeTableAdapters.DataTableHomeTableAdapter dt = new DataSetHomeTableAdapters.DataTableHomeTableAdapter();

                CDGridView.Columns.Clear();

                CDGridView.DataSource = dt.GetAllData();
                CDGridView.Columns["Id"].Visible = false;
                DesignCDGView();
                lblTotalCount.Text = GetTotal().ToString();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesignCDGView()
        {
            //CDGridView.Columns["Name"].Width = 100;
            //CDGridView.Columns["Phone"].Width = 120;
            //CDGridView.Columns["Vin"].Width = 240;
            //CDGridView.Columns["Model"].Width = 100;
            //CDGridView.Columns["Engine"].Width = 80;
            //CDGridView.Columns["Carplatenumber"].Width = 100;
            CDGridView.Columns["Distance"].Width = 88;
            CDGridView.Columns["Year"].Width = 60;
            CDGridView.Columns["FileName"].Width = 100;
            //CDGridView.Columns["Datetime"].Width = 140;

            //CDGridView.Columns["Carplatenumber"].HeaderText = "Plate";
            //CDGridView.Columns["Distance"].HeaderText = "Distance-km";


            CDGridView.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            CDGridView.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            CDGridView.Columns["Vin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //CDGridView.Columns["Model"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            CDGridView.Columns["Model"].Width = 100;
            //CDGridView.Columns["Engine"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            CDGridView.Columns["Engine"].Width = 80;
            CDGridView.Columns["Carplatenumber"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //CDGridView.Columns["Distance"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            CDGridView.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //CDGridView.Columns["Year"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            //CDGridView.Columns["FileName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            CDGridView.Columns["Datetime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            CDGridView.Columns["Carplatenumber"].HeaderText = "Plate";
            CDGridView.Columns["Distance"].HeaderText = "Distance-km";
        }

        //Add data to database
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txt_Vin.Text != "")
            {
                SqlConnection sqlConnection = AbstractDAO.Connect();
                string addingVin = txt_Vin.Text.ToUpper();
                string existvin = null;
                try
                {
                    string query = "Select Cars_tbl.Vin From Cars_tbl where Cars_tbl.Vin = @vin";

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("vin", addingVin);

                    sqlConnection.Open();
                    existvin = Convert.ToString(sqlCommand.ExecuteScalar());
                    sqlConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sqlConnection.Close();
                }


                if (existvin == addingVin)
                {
                    MessageBox.Show("Already exist vin, see below.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DesignAllColumnsOnCDGView();
                    PrintFord(SearchByVin(txt_Vin.Text));
                }
                else
                {
                    string name = txt_Name.Text.Trim();
                    string surname = "";
                    string phone = txt_Phone.Text.Trim();
                    string vin = txt_Vin.Text.Trim().ToUpper();
                    string model = txt_Model.Text.TrimEnd();
                    string engine = txt_Engine.Text;
                    string carplatenumber = txt_Carplatenumber.Text.ToUpper();
                    string description = txt_Description.Text.TrimEnd();
                    string filePath = txtFilePath.Text;
                    DateTime dateTime = DateTime.Now;
                    string distance = txt_Distance.Text.Trim();
                    string year = txt_Year.Text.Trim();

                    if (DesignYearText(year))
                    {
                        return; 
                    }
                   


                    if (distance == "")
                    {
                        MessageBox.Show("Distance not be empty!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string extn;
                    string fileName;

                    if (filePath == "")
                    {
                        extn = "";
                        fileName = "";
                    }
                    else if (File.Exists(filePath))
                    {
                        var file = new FileInfo(filePath);
                        extn = file.Extension;
                        fileName = file.Name;
                    }
                    else
                    {
                        MessageBox.Show("File do not exist", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }



                    if (vin == "")
                    {
                        MessageBox.Show("Vin not be null!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (vin.Length < 17)
                    {
                        MessageBox.Show("Vin length does not match correctly!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else  
                    {
                        Owner owner = new Owner(name, surname, phone);
                        Description desc = new Description(0, description, dateTime, filePath, fileName, extn, distance);
                        Ford ford = new Ford(0, owner, vin, model, engine, carplatenumber, year, desc);


                        FordDaoInter fordDao = Context.InstanceOfFordDao();
                        fordDao.Add(ford, desc);
                        Clear();
                        GetAlls();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please, Insert information!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Update data from database
        private void btnUpdate_Click(object sender, EventArgs e)
        {


            if (selectedId != 0 && (radioBtnAdd.Checked || radioBtnEdit.Checked || radioBtnDelete.Checked))
            {
                #region Control               

                int control = 0;

                if (radioBtnAdd.Checked)
                {
                    control = 1;
                }
                if (radioBtnEdit.Checked)
                {
                    control = 2;
                }
                if (radioBtnDelete.Checked)
                {
                    SqlConnection connection = AbstractDAO.Connect();
                    int idCount = 0;
                    try
                    {
                        connection.Open();
                        string query = "Select count(*) from Description_tbl  where Description_tbl.CarId = @id";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@id", selectedId);
                        idCount = Convert.ToInt32(command.ExecuteScalar());
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

                    if (idCount > 1)
                    {
                        control = 3;
                    }
                    else if (idCount == 1)
                    {
                        MessageBox.Show("You cant delete it!!!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        radioBtnDelete.Checked = false;
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                #endregion

                string name = txt_Name.Text.TrimEnd();
                string surname = "";
                string phone = txt_Phone.Text.TrimEnd();
                string vin = txt_Vin.Text.TrimEnd();
                string model = txt_Model.Text.TrimEnd();
                string engine = txt_Engine.Text.TrimEnd();
                string carplatenumber = txt_Carplatenumber.Text.TrimEnd().ToUpper();
                string description = txt_Description.Text.TrimEnd();
                string filePath = txtFilePath.Text;
                string distance = txt_Distance.Text;
                string year = txt_Year.Text.Trim();

                string extn = "";
                string fileName = "";

                if (filePath == "")
                {
                    extn = "";
                    fileName = "";
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        var file = new FileInfo(filePath);
                        extn = file.Extension;
                        fileName = file.Name;
                    }
                    else
                    {
                        MessageBox.Show("File doesn't exist!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Clear();              
                Owner owner = new Owner(name, surname, phone);
                Description desc = new Description(selectedId, description, dt, filePath, fileName, extn, distance);
                Ford ford = new Ford(0, owner, vin, model, engine, carplatenumber, year, desc);

                FordDaoInter fordDao = Context.InstanceOfFordDao();
                fordDao.Update(ford, desc, selectedId, control);
                Clear();
                GetAlls();
            }
            else
            {
                string message = "Please, Select exist information or check what do you want to do.";
                MessageBox.Show(message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Delete data from database
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string vin = txt_Vin.Text;

            if (vin != "" && vin.Length == 17 && selectedId != 0)
            {
                if (PreConfirmation())
                {
                    Clear();
                    FordDaoInter fordDao = Context.InstanceOfFordDao();
                    fordDao.Delete(selectedId);

                    GetAlls();
                }
            }
            else
            {
                MessageBox.Show("Please, select car!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Search data by name            
        private List<Ford> SearchByName(string name)
        {
            FordDaoInter fordDao = Context.InstanceOfFordDao();
            List<Ford> fordList = fordDao.GetByName(name);
            return fordList;
        }


        //Search data by Phone
        private List<Ford> SerachByPhone(string phone)
        {
            FordDaoInter fordDao = Context.InstanceOfFordDao();
            List<Ford> fordList = fordDao.GetByPhone(phone);
            return fordList;
        }

        //Search data by vin      
        private List<Ford> SearchByVin(string vin)
        {
            FordDaoInter fordDao = Context.InstanceOfFordDao();
            List<Ford> fordList = fordDao.GetByVin(vin);
            return fordList;
        }

        //Serach by car plate number        
        private List<Ford> SearchByCarPlateNumber(string platenumber)
        {

            FordDaoInter fordDao = Context.InstanceOfFordDao();
            List<Ford> fordList = fordDao.GetByCarPlateNumber(platenumber);
            return fordList;
        }

        //Print data
        private void PrintFord(List<Ford> fords)
        {
            int index = 0;
            for (int i = 0; i < fords.Count; i++)
            {
                index = CDGridView.Rows.Add();
                CDGridView.Columns["Id"].Visible = false;
                CDGridView.Rows[index].Cells["Id"].Value = fords[i].Id;
                CDGridView.Rows[index].Cells["Name"].Value = fords[i].Owner.Name;
                CDGridView.Rows[index].Cells["Phone"].Value = fords[i].Owner.Phone;
                CDGridView.Rows[index].Cells["Vin"].Value = fords[i].Vin;
                CDGridView.Rows[index].Cells["Model"].Value = fords[i].Model;
                CDGridView.Rows[index].Cells["Engine"].Value = fords[i].Engine;
                CDGridView.Rows[index].Cells["Carplatenumber"].Value = fords[i].CarPlateNumber;
                CDGridView.Rows[index].Cells["Distance"].Value = fords[i].Description.Distance;
                CDGridView.Rows[index].Cells["Year"].Value = fords[i].Year;
                CDGridView.Rows[index].Cells["Description"].Value = fords[i].Description.Desc;
                CDGridView.Rows[index].Cells["FileName"].Value = fords[i].Description.FileName;
                CDGridView.Rows[index].Cells["Datetime"].Value = fords[i].Description.DateTime;
            }
        }


        private void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                else
                {
                    ClearAllText(c);
                }
            }
        }

        private void Clear()
        {
            txt_Name.Text = string.Empty;
            txt_Phone.Text = string.Empty;
            txt_Vin.Text = string.Empty;
            txt_Model.Text = string.Empty;
            txt_Engine.Text = string.Empty;
            txt_Carplatenumber.Text = string.Empty;
            txt_Distance.Text = string.Empty;
            txt_Year.Text = string.Empty;
            txt_Description.Text = string.Empty;
            txtFilePath.Text = string.Empty;
            txtSearchingText.Text = string.Empty;

            radioBtnSearchByName.Checked = false;
            radioBtnSearchByPhone.Checked = false;
            radioBtnSerachByVin.Checked = false;
            radioBtnSearchByCarPlateNumber.Checked = false;

            radioBtnAdd.Checked = false;
            radioBtnEdit.Checked = false;
            radioBtnDelete.Checked = false;
        }

        private void NumbersOnly(object sender, KeyPressEventArgs e)
        {
            int asciiCode = Convert.ToInt32(e.KeyChar);

            if (asciiCode != 8)
            {
                if ((asciiCode >= 48 && asciiCode <= 57))
                {
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("Number Only Please!", "Error: Number Only", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    e.Handled = true;
                }
            }
        }

        private bool PreConfirmation()
        {
            DialogResult iExit;
            iExit = MessageBox.Show("Confirm if you want to delete", "Client Database", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (iExit == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void txtSearchingText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioBtnSearchByPhone.Checked)
            {
                NumbersOnly(sender, e);
            }

        }

        private void txt_Phone_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void btnGetAlls_Click(object sender, EventArgs e)
        {
            GetAlls();
        }

        //Get total record from databse
        private int GetTotal()
        {
            SqlConnection conn = AbstractDAO.Connect();
            int total = 0;
            try
            {
                string sql = "Select count(*) from Cars_tbl";

                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                total = Convert.ToInt32(sqlCommand.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return total;
        }







        int selectedId = 0;
        string filePath;
        DateTime dt;
        private void CDGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedId = Convert.ToInt32(CDGridView.CurrentRow.Cells["Id"].Value);
            txt_Name.Text = CDGridView.CurrentRow.Cells["Name"].Value.ToString();
            txt_Phone.Text = CDGridView.CurrentRow.Cells["Phone"].Value.ToString();
            txt_Vin.Text = CDGridView.CurrentRow.Cells["Vin"].Value.ToString();
            txt_Model.Text = CDGridView.CurrentRow.Cells["Model"].Value.ToString();
            txt_Engine.Text = CDGridView.CurrentRow.Cells["Engine"].Value.ToString();
            txt_Carplatenumber.Text = CDGridView.CurrentRow.Cells["Carplatenumber"].Value.ToString();
            txt_Distance.Text = CDGridView.CurrentRow.Cells["Distance"].Value.ToString();
            txt_Year.Text = CDGridView.CurrentRow.Cells["Year"].Value.ToString();
            txt_Description.Text = CDGridView.CurrentRow.Cells["Description"].Value.ToString();
            txtFilePath.Text = CDGridView.CurrentRow.Cells["FileName"].Value.ToString();
            dt = DateTime.Parse(CDGridView.CurrentRow.Cells["Datetime"].Value.ToString());

            filePath = CDGridView.CurrentRow.Cells["FileName"].Value.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchingtext = txtSearchingText.Text;

            if (searchingtext != "")
            {
                if (radioBtnSerachByVin.Checked)
                {
                    List<BlackClient> blackList = BlackClientSearchByVin(searchingtext.ToUpper());
                    List<Ford> fordList = SearchByVin(searchingtext.ToUpper());
                    DesignAllColumnsOnCDGView();

                    if (blackList.Count != 0)
                    {
                        PrintBlackClient(blackList, true);
                        PrintFord(fordList);
                    }
                    else
                    {
                        PrintFord(fordList);
                    }

                }
                else if (radioBtnSearchByName.Checked)
                {
                    List<BlackClient> blackList = BlackClientSearchByName(searchingtext);
                    List<Ford> fordList = SearchByName(searchingtext.ToUpper());
                    DesignAllColumnsOnCDGView();

                    if (blackList.Count != 0)
                    {
                        PrintBlackClient(blackList, true);
                        PrintFord(fordList);

                    }
                    else
                    {
                        PrintFord(fordList);
                    }
                }
                else if (radioBtnSearchByPhone.Checked)
                {

                    List<BlackClient> blackList = BlackSearchByCPNumber(searchingtext.ToUpper());
                    List<Ford> fordList = SerachByPhone(searchingtext);
                    DesignAllColumnsOnCDGView();

                    if (blackList.Count != 0)
                    {
                        PrintBlackClient(blackList, true);
                        PrintFord(fordList);
                    }
                    else
                    {
                        PrintFord(fordList);
                    }
                }
                else if (radioBtnSearchByCarPlateNumber.Checked)
                {
                    List<BlackClient> blackList = BlackSearchByCPNumber(searchingtext.ToUpper());
                    List<Ford> fordList = SearchByCarPlateNumber(searchingtext.ToUpper());
                    DesignAllColumnsOnCDGView();

                    if (blackList.Count != 0)
                    {
                        PrintBlackClient(blackList, true);
                        PrintFord(fordList);

                    }
                    else
                    {
                        PrintFord(fordList);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please check searching cell!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DesignAllColumnsOnCDGView()
        {
            CDGridView.DataSource = null;
            CDGridView.Columns.Clear();

            CDGridView.Columns.Add("Id", "Id");
            CDGridView.Columns.Add("Name", "Name");
            CDGridView.Columns.Add("Phone", "Phone");
            CDGridView.Columns.Add("Vin", "Vin");
            CDGridView.Columns.Add("Model", "Model");
            CDGridView.Columns.Add("Engine", "Engine");
            CDGridView.Columns.Add("Carplatenumber", "Carplatenumber");
            CDGridView.Columns.Add("Distance", "Distance");
            CDGridView.Columns.Add("Year", "Year");
            CDGridView.Columns.Add("Description", "Description");
            CDGridView.Columns.Add("FileName", "FileName");
            CDGridView.Columns.Add("Datetime", "Datetime");

            CDGridView.Columns["Carplatenumber"].HeaderText = "Plate";
            CDGridView.Columns["Distance"].HeaderText = "Distance-km";
        }
        private void ClientManagementForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string pathString = @"C:\ClientManagementDocuments";
                DirectoryInfo dir = new DirectoryInfo(pathString);

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            txtFilePath.Text = dlg.FileName;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (filePath != "")
            {
                OpenFile(selectedId);
            }
            else
            {
                MessageBox.Show("File not found!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void OpenFile(int id)
        {
            SqlConnection cn = AbstractDAO.Connect();
            try
            {
                string query = "Select Data,FileName,Extension from Description_tbl Where CarId = @id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                cn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var name = reader["FileName"].ToString();
                    var data = (byte[])reader["data"];
                    var extn = reader["Extension"].ToString();
                    var newFileName = name.Replace(extn, DateTime.Now.ToString("ddMMyyyyhhmmss")) + extn;

                    string pathString = @"C:\ClientManagementDocuments";
                    Directory.CreateDirectory(pathString);
                    pathString = Path.Combine(pathString, newFileName);

                    File.WriteAllBytes(pathString, data);
                    System.Diagnostics.Process.Start(pathString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        private string GetFilePath(int id)
        {
            SqlConnection cn = AbstractDAO.Connect();
            try
            {
                string query = "Select Data,FileName,Extension from Description_tbl Where CarId = @id";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                cn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var name = reader["FileName"].ToString();
                    var data = (byte[])reader["data"];
                    var extn = reader["Extension"].ToString();
                    var newFileName = name.Replace(extn, DateTime.Now.ToString("ddMMyyyyhhmmss")) + extn;

                    string pathString = @"C:\ClientManagementDocuments";
                    Directory.CreateDirectory(pathString);
                    pathString = Path.Combine(pathString, newFileName);

                    File.WriteAllBytes(pathString, data);
                    return pathString;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
            return null;
        }

        private void ClientManagementForm_Load(object sender, EventArgs e)
        {
            //Data Source=.;Initial Catalog=ClientDatabaseManagementSystem;Integrated Security=True
            try
            {
                AbstractDAO.url = File.ReadAllText(@"C:\Program Files (x86)\Blackwolf Company\DatabaseConnection\config.txt");
                //AbstractDAO.url = @"Data Source=.;Initial Catalog=ClientDatabaseManagementSystem;Integrated Security=True";
                GetAlls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public bool Export<T>(List<T> list, string file, string sheetName)
        {
            bool exported = false;
            using (IXLWorkbook workbook = new XLWorkbook())
            {

                workbook.AddWorksheet(sheetName).FirstCell().InsertTable<T>(list, false);

                workbook.SaveAs(file);
                exported = true;
            }

            return exported;
        }

        private List<Client> GetClientList()
        {
            SqlConnection connection = AbstractDAO.Connect();
            List<Client> clientList = null;
            try
            {
                connection.Open();

                string query = "SELECT Cars_tbl.Name, Cars_tbl.Phone, Cars_tbl.Carplatenumber FROM Cars_tbl";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();

                clientList = new List<Client>();
                string name = "";
                string phone = "";
                string plate = "";


                while (dataReader.Read())
                {
                    name = dataReader["Name"].ToString();
                    phone = dataReader["Phone"].ToString();
                    plate = dataReader["Carplatenumber"].ToString();

                    clientList.Add(new Client(name, phone, plate));
                }
                connection.Close();

                List<Client> temp = new List<Client>();

                foreach (Client client in clientList)
                {
                    if (client.Phone != "")
                    {
                        temp.Add(client);
                    }
                }

                clientList = temp;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return clientList;
        }

        private List<ClientAllInfos> GetClientAllList()
        {
            SqlConnection connection = AbstractDAO.Connect();
            List<ClientAllInfos> clientList = null;
            try
            {
                connection.Open();

                string query = "SELECT Cars_tbl.Name, Cars_tbl.Phone, Cars_tbl.Vin, Cars_tbl.Model, Cars_tbl.Engine, Cars_tbl.Carplatenumber FROM Cars_tbl";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();

                clientList = new List<ClientAllInfos>();
                string name = "";
                string phone = "";
                string vin = "";
                string model = "";
                string engine = "";
                string plate = "";



                while (dataReader.Read())
                {
                    name = dataReader["Name"].ToString();
                    phone = dataReader["Phone"].ToString();
                    vin = dataReader["Vin"].ToString();
                    model = dataReader["Model"].ToString();
                    engine = dataReader["Engine"].ToString();
                    plate = dataReader["Carplatenumber"].ToString();

                    clientList.Add(new ClientAllInfos(name, phone, vin, model, engine, plate));
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
            return clientList;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<Client> clientList = GetClientList();
            if (clientList != null)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Wokbook |*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {

                            if (Export<Client>(clientList, sfd.FileName, "Cars"))
                            {
                                MessageBox.Show("You have successfully exported your data to an excel file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnEportAll_Click(object sender, EventArgs e)
        {
            List<ClientAllInfos> clientList = GetClientAllList();
            if (clientList != null)
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Excel Wokbook |*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {

                            if (Export<ClientAllInfos>(clientList, sfd.FileName, "ALlInfosCars"))
                            {
                                MessageBox.Show("You have successfully exported your data to an excel file.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Error", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion

        #region Orders

        private void BtnOrdersGetAll_Click(object sender, EventArgs e)
        {
            GetAllOrders();
        }

        private void GetAllOrders()
        {
            SqlConnection connection = AbstractDAO.Connect();
            try
            {
                ClearOrdersTab();
                OrdersCDGView.Rows.Clear();

                connection.Open();
                string query = "select * from Orders_tbl";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();


                int index = 0;

                while (dataReader.Read())
                {
                    index = OrdersCDGView.Rows.Add();
                    OrdersCDGView.Rows[index].Cells[0].Value = dataReader["Id"].ToString();
                    OrdersCDGView.Rows[index].Cells[1].Value = dataReader["Name"].ToString();
                    OrdersCDGView.Rows[index].Cells[2].Value = dataReader["Phone"].ToString();
                    OrdersCDGView.Rows[index].Cells[3].Value = dataReader["CPNumber"].ToString();
                    OrdersCDGView.Rows[index].Cells[4].Value = dataReader["Orders"].ToString();
                    OrdersCDGView.Rows[index].Cells[5].Value = dataReader["Datetime"].ToString();
                }

                lblOrdersTotalCount.Text = GetOrdersTotal().ToString();
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


        private int GetOrdersTotal()
        {
            SqlConnection conn = AbstractDAO.Connect();
            int total = 0;
            try
            {
                string sql = "Select count(*) from Orders_tbl";

                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                total = Convert.ToInt32(sqlCommand.ExecuteScalar());
                conn.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            return total;
        }

        private void BtnOrdersAdd_Click(object sender, EventArgs e)
        {
            string name = txtOrderName.Text.Trim();
            string phone = txtOrderPhone.Text.Trim();
            string cpnumber = txtOrderCPNumber.Text.Trim().ToUpper();
            string orders = txtOrders.Text.Trim();

            if (name == string.Empty && phone == "" && cpnumber == "" && orders == "")
            {
                MessageBox.Show("Please, Insert information.");
                return;
            }

            DateTime dateTime = DateTime.Now;

            Order order = new Order(0, name, phone, cpnumber, orders, dateTime);

            ClearOrdersTab();

            if (name != "" || phone != "" || cpnumber != "" || orders != "")
            {
                OrderDaoInter orderDao = Context.InstanceOfOrderDao();
                orderDao.Add(order);

                GetAllOrders();
            }
        }

        private int id;

        private void OrdersCDGView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Convert.ToInt32(OrdersCDGView.CurrentRow.Cells[0].Value);
            txtOrderName.Text = OrdersCDGView.CurrentRow.Cells[1].Value.ToString();
            txtOrderPhone.Text = OrdersCDGView.CurrentRow.Cells[2].Value.ToString();
            txtOrderCPNumber.Text = OrdersCDGView.CurrentRow.Cells[3].Value.ToString();
            txtOrders.Text = OrdersCDGView.CurrentRow.Cells[4].Value.ToString();
        }

        private void txtOrderSearchingText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioBtnOrdersPhone.Checked)
            {
                NumbersOnly(sender, e);
            }
        }

        //Update orders data
        private void BtnOrdersUpdate_Click(object sender, EventArgs e)
        {
            string name = txtOrderName.Text.TrimEnd();
            string phone = txtOrderPhone.Text.TrimEnd();
            string cpnumber = txtOrderCPNumber.Text.TrimEnd();
            string orders = txtOrders.Text.TrimEnd();
            DateTime dateTime = DateTime.Now;

            ClearOrdersTab();

            Order order = new Order(id, name, phone, cpnumber, orders, dateTime);

            OrderDaoInter orderDao = Context.InstanceOfOrderDao();
            orderDao.Update(order);

            GetAllOrders();
        }

        private void BtnOrdersDelete_Click(object sender, EventArgs e)
        {
            string name = txtOrderName.Text;
            string phone = txtOrderPhone.Text;
            string cpnumber = txtOrderCPNumber.Text;
            string orders = txtOrders.Text;

            if (name != "" || phone != "" || cpnumber != "" || orders != "")
            {
                if (PreConfirmation())
                {
                    ClearOrdersTab();
                    OrderDaoInter orderDao = Context.InstanceOfOrderDao();
                    orderDao.Delete(id);
                    GetAllOrders();
                }
            }
            else
            {
                MessageBox.Show("Please, select order!");
            }
        }

        private void ClearOrdersTab()
        {
            //ClearAllText(this);
            txtOrderName.Text = string.Empty;
            txtOrderPhone.Text = string.Empty;
            txtOrderCPNumber.Text = string.Empty;
            txtOrders.Text = string.Empty;
            txtOrderSearchingText.Text = string.Empty;

            radioBtnOrdersName.Checked = false;
            radioBtnOrdersPhone.Checked = false;
            radioBtnOrderCPNumber.Checked = false;
        }

        private void BtnOrdersClear_Click(object sender, EventArgs e)
        {
            ClearOrdersTab();
        }

        private void txtOrderPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void BtnOrdersSearch_Click(object sender, EventArgs e)
        {

            string searchingtext = txtOrderSearchingText.Text.Trim();

            if (radioBtnOrdersName.Checked)
            {
                OrdersSearchByName(searchingtext);
            }
            else if (radioBtnOrdersPhone.Checked)
            {
                OrdersSearchByPhone(searchingtext);
            }
            else if (radioBtnOrderCPNumber.Checked)
            {
                OrdersSearchByCPNumber(searchingtext);
            }
            else if (radioBtnOrder.Checked)
            {
                OrdersSearchByOrders(searchingtext);
            }
            else
            {
                MessageBox.Show("Please check searching cell!");
            }
        }

        private void OrdersSearchByName(string name)
        {
            OrderDaoInter orderDao = Context.InstanceOfOrderDao();

            List<Order> orders = orderDao.GetByName(name);

            PrintOrders(orders);
        }

        private void OrdersSearchByPhone(string phone)
        {
            OrderDaoInter orderDao = Context.InstanceOfOrderDao();

            List<Order> orders = orderDao.GetByName(phone);

            PrintOrders(orders);
        }

        private void OrdersSearchByCPNumber(string cpnumber)
        {
            OrderDaoInter orderDao = Context.InstanceOfOrderDao();

            List<Order> orders = orderDao.GetByCPNumber(cpnumber);

            PrintOrders(orders);
        }

        private void OrdersSearchByOrders(string order)
        {
            OrderDaoInter orderDao = Context.InstanceOfOrderDao();

            List<Order> orders = orderDao.GetByOrders(order);

            PrintOrders(orders);
        }

        private void PrintOrders(List<Order> orders)
        {
            OrdersCDGView.Rows.Clear();

            int index = 0;
            for (int i = 0; i < orders.Count; i++)
            {
                index = OrdersCDGView.Rows.Add();

                OrdersCDGView.Rows[index].Cells[0].Value = orders[i].Id;
                OrdersCDGView.Rows[index].Cells[1].Value = orders[i].Name;
                OrdersCDGView.Rows[index].Cells[2].Value = orders[i].Phone;
                OrdersCDGView.Rows[index].Cells[3].Value = orders[i].Cpnumber;
                OrdersCDGView.Rows[index].Cells[4].Value = orders[i].Orders;
                OrdersCDGView.Rows[index].Cells[5].Value = orders[i].DateTime;
            }
        }

        #endregion

        #region Black

        private void BtnBlackAdd_Click(object sender, EventArgs e)
        {
            string name = txtBlackName.Text.Trim();
            string phone = txtBlackPhone.Text.Trim();
            string vin = txtBlackVin.Text.Trim().ToUpper();
            string cpnumber = txtBlackCPNumber.Text.Trim().ToUpper();
            string description = txtBlackDescription.Text.Trim();
            DateTime dateTime = DateTime.Now;

            if (name == "" && phone == "" && vin == "" && cpnumber == "" && description == "")
            {
                MessageBox.Show("Please, Insert information.");
                return;
            }

            BlackClient blackClient = new BlackClient(0, name, phone, vin, cpnumber, description, dateTime);

            ClearBlackClientTab();

            if (name != "" || phone != "" || cpnumber != "" || description != "" || vin != "")
            {
                BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

                blackClientDao.Add(blackClient);


                GetAllBlackClient();
            }
        }

        private void BtnBlackGetAll_Click(object sender, EventArgs e)
        {
            GetAllBlackClient();
        }

        private void GetAllBlackClient()
        {
            SqlConnection connection = AbstractDAO.Connect();
            try
            {
                ClearBlackClientTab();
                BlackCDGView.Rows.Clear();

                connection.Open();
                string query = "select * from BlackClient_tbl";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();

                int index = 0;

                while (dataReader.Read())
                {
                    index = BlackCDGView.Rows.Add();
                    BlackCDGView.Rows[index].Cells[0].Value = dataReader["Id"].ToString();
                    BlackCDGView.Rows[index].Cells[1].Value = dataReader["Name"].ToString();
                    BlackCDGView.Rows[index].Cells[2].Value = dataReader["Phone"].ToString();
                    BlackCDGView.Rows[index].Cells[3].Value = dataReader["Vin"].ToString();
                    BlackCDGView.Rows[index].Cells[4].Value = dataReader["CPNumber"].ToString();
                    BlackCDGView.Rows[index].Cells[5].Value = dataReader["Description"].ToString();
                    BlackCDGView.Rows[index].Cells[6].Value = dataReader["Datetime"].ToString();
                }

                lblBlackTotalCount.Text = GetBlackClientTotal().ToString();

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

        private void ClearBlackClientTab()
        {
            //ClearAllText(this);
            txtBlackName.Text = string.Empty;
            txtBlackPhone.Text = string.Empty;
            txtBlackVin.Text = string.Empty;
            txtBlackCPNumber.Text = string.Empty;

            radioBtnBlackVin.Checked = false;
            radioBtnBlackName.Checked = false;
            radioBtnBlackPhone.Checked = false;
            radioBtnBlackCPNumber.Checked = false;

        }

        private int GetBlackClientTotal()
        {
            SqlConnection conn = AbstractDAO.Connect();

            int total = 0;
            try
            {
                string sql = "Select count(*) from BlackClient_tbl";

                conn.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, conn);

                total = Convert.ToInt32(sqlCommand.ExecuteScalar());

                conn.Close();
            }
            catch (Exception ex)
            {
                //Logs.CreateLog(ex);
                //MessageBox.Show("Errors, please look at log.txt file");
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return total;
        }

        private int blackId;
        private void BtnBlackUpdate_Click(object sender, EventArgs e)
        {
            string name = txtBlackName.Text.Trim();
            string phone = txtBlackPhone.Text.Trim();
            string vin = txtBlackVin.Text.Trim();
            string cpnumber = txtBlackCPNumber.Text.Trim();
            string description = txtBlackDescription.Text.Trim();
            DateTime dateTime = DateTime.Now;

            ClearBlackClientTab();

            BlackClient blackClient = new BlackClient(blackId, name, phone, vin, cpnumber, description, dateTime);

            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

            blackClientDao.Update(blackClient);
            GetAllBlackClient();

        }

        private void BlackCDGView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            blackId = Convert.ToInt32(BlackCDGView.CurrentRow.Cells[0].Value);
            txtBlackName.Text = BlackCDGView.CurrentRow.Cells[1].Value.ToString();
            txtBlackPhone.Text = BlackCDGView.CurrentRow.Cells[2].Value.ToString();
            txtBlackVin.Text = BlackCDGView.CurrentRow.Cells[3].Value.ToString();
            txtBlackCPNumber.Text = BlackCDGView.CurrentRow.Cells[4].Value.ToString();
            txtBlackDescription.Text = BlackCDGView.CurrentRow.Cells[5].Value.ToString();
        }

        private void BtnBlackDelete_Click(object sender, EventArgs e)
        {
            string name = txtBlackName.Text.Trim();
            string phone = txtBlackPhone.Text.Trim();
            string vin = txtBlackVin.Text.Trim();
            string cpnumber = txtBlackCPNumber.Text.Trim();
            string description = txtBlackDescription.Text.Trim();




            if (name != "" || phone != "" || cpnumber != "" || description != "" || vin != "")
            {
                if (PreConfirmation())
                {
                    ClearOrdersTab();
                    BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

                    blackClientDao.Delete(blackId);
                    GetAllBlackClient();
                }
            }
            else
            {
                MessageBox.Show("Please, select BlackClient!");
            }
        }

        private void BtnBlackClear_Click(object sender, EventArgs e)
        {
            ClearBlackClientTab();
        }

        private void BtnBlackSearch_Click(object sender, EventArgs e)
        {
            string searchingtext = txtBlackClientSearchingText.Text.Trim();

            if (radioBtnBlackName.Checked)
            {
                PrintBlackClient(BlackClientSearchByName(searchingtext));
            }
            else if (radioBtnBlackVin.Checked)
            {
                PrintBlackClient(BlackClientSearchByVin(searchingtext));
            }
            else if (radioBtnBlackPhone.Checked)
            {
                PrintBlackClient(BlackClientSearchByPhone(searchingtext));
            }
            else if (radioBtnBlackCPNumber.Checked)
            {
                PrintBlackClient(BlackClientSearchByCPNumber(searchingtext));
            }
            else
            {
                MessageBox.Show("Please check searching cell!");
            }
        }


        private List<BlackClient> BlackClientSearchByName(string name)
        {
            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

            List<BlackClient> blackClientsList = blackClientDao.GetByName(name);

            return blackClientsList;
        }

        private List<BlackClient> BlackClientSearchByPhone(string phone)
        {
            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();
            List<BlackClient> blackClientsList = blackClientDao.GetByPhone(phone);
            return blackClientsList;
        }

        private List<BlackClient> BlackClientSearchByCPNumber(string cpnumber)
        {
            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

            List<BlackClient> blackClientsList = blackClientDao.GetByCPNumber(cpnumber);

            return blackClientsList;
        }

        private List<BlackClient> BlackSearchByCPNumber(string cpnumber)
        {
            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

            List<BlackClient> blackClientsList = blackClientDao.GetByCPNumber(cpnumber);

            return blackClientsList;
        }

        private List<BlackClient> BlackClientSearchByVin(string vin)
        {
            BlackClientDaoInter blackClientDao = Context.InstanceOfBlackClientDao();

            List<BlackClient> blackClientsList = blackClientDao.GetByVin(vin);

            return blackClientsList;
        }

        private void PrintBlackClient(List<BlackClient> blackClients)
        {
            BlackCDGView.Rows.Clear();

            int index = 0;
            for (int i = 0; i < blackClients.Count; i++)
            {
                index = BlackCDGView.Rows.Add();

                BlackCDGView.Rows[index].Cells[0].Value = blackClients[i].Id;
                BlackCDGView.Rows[index].Cells[1].Value = blackClients[i].Name;
                BlackCDGView.Rows[index].Cells[2].Value = blackClients[i].Phone;
                BlackCDGView.Rows[index].Cells[3].Value = blackClients[i].Vin;
                BlackCDGView.Rows[index].Cells[4].Value = blackClients[i].Cpnumber;
                BlackCDGView.Rows[index].Cells[5].Value = blackClients[i].Description;
                BlackCDGView.Rows[index].Cells[6].Value = blackClients[i].DateTime;
            }
        }

        private void PrintBlackClient(List<BlackClient> blackClients, bool control)
        {
            int index = 0;
            for (int i = 0; i < blackClients.Count; i++)
            {
                index = CDGridView.Rows.Add();

                CDGridView.Columns["Id"].Visible = false;
                CDGridView.Rows[index].Cells["Id"].Value = blackClients[i].Id;
                CDGridView.Rows[index].Cells["Name"].Value = blackClients[i].Name;
                CDGridView.Rows[index].Cells["Phone"].Value = blackClients[i].Phone;
                CDGridView.Rows[index].Cells["Vin"].Value = blackClients[i].Vin;
                CDGridView.Rows[index].Cells["Model"].Value = "";
                CDGridView.Rows[index].Cells["Engine"].Value = "";
                CDGridView.Rows[index].Cells["Carplatenumber"].Value = blackClients[i].Cpnumber;
                CDGridView.Rows[index].Cells["Distance"].Value = "";
                CDGridView.Rows[index].Cells["Year"].Value = "";
                CDGridView.Rows[index].Cells["Description"].Value = blackClients[i].Description;
                CDGridView.Rows[index].Cells["FileName"].Value = "";
                CDGridView.Rows[index].Cells["Datetime"].Value = blackClients[i].DateTime;

                CDGridView.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                CDGridView.Rows[index].DefaultCellStyle.ForeColor = Color.White;
            }
        }

        private void txtBlackPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void txtBlackClientSearchingText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (radioBtnBlackPhone.Checked)
            {
                NumbersOnly(sender, e);
            }
        }

        #endregion


        private string nameStr;
        private string phoneStr;
        private string vinStr;
        private string plateStr;

        private void CDGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            nameStr = CDGridView.CurrentRow.Cells["Name"].Value.ToString();
            phoneStr = CDGridView.CurrentRow.Cells["Phone"].Value.ToString();
            vinStr = CDGridView.CurrentRow.Cells["Vin"].Value.ToString();
            plateStr = CDGridView.CurrentRow.Cells["Carplatenumber"].Value.ToString();
        }

        private void CDGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selectedid = Convert.ToInt32(CDGridView.CurrentRow.Cells["Id"].Value);
            string cellName = CDGridView.CurrentCell.OwningColumn.Name;
            string filename = CDGridView.CurrentCell.Value.ToString();

            if (cellName != "FileName")
            {
                return;
            }

            if (filename != "")
            {
                OpenFile(selectedid);
            }
            else
            {
                MessageBox.Show("File not found!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txt_Distance_KeyPress(object sender, KeyPressEventArgs e)
        {
            NumbersOnly(sender, e);
        }

        private void addToOrdersList_Click(object sender, EventArgs e)
        {
            if (nameStr != "" || phoneStr != "" || plateStr != "")
            {
                tabControl.SelectTab(1);

                txtOrderName.Text = nameStr;
                txtOrderPhone.Text = phoneStr;
                txtOrderCPNumber.Text = plateStr;
            }

        }

        private void addToBlackList_Click(object sender, EventArgs e)
        {
            if (nameStr != "" || phoneStr != "" || plateStr != "" || vinStr != "")
            {
                tabControl.SelectTab(2);

                txtBlackName.Text = nameStr;
                txtBlackPhone.Text = phoneStr;
                txtBlackVin.Text = vinStr;
                txtBlackCPNumber.Text = plateStr;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;

            try
            {
                if (txt_Vin.Text != "" && txt_Description.Text != "")
                {
                    string name;
                    string phone;
                    string model;
                    string engine;
                    string plate;
                    string distance;

                    name = txt_Name.Text;
                    phone = txt_Phone.Text;
                    model = txt_Model.Text;
                    engine = txt_Engine.Text;
                    plate = txt_Carplatenumber.Text;
                    distance = txt_Distance.Text;

                    if (name == "")
                    {
                        name = "Null";
                    }

                    if (phone == "")
                    {
                        phone = "Null";
                    }

                    if (model == "")
                    {
                        model = "Null";
                    }

                    if (engine == "")
                    {
                        engine = "Null";
                    }

                    if (plate == "")
                    {
                        plate = "Null";
                    }

                    if (distance == "")
                    {
                        distance = "Null";
                    }
                    else
                    {
                        distance = distance + " km";
                    }

                    string dtcFilePath;
                    string dtcFileText = null;

                    if (filePath != "" && txtFilePath.Text != "")
                    {
                        dtcFilePath = GetFilePath(selectedId);
                        StringBuilder builder = new StringBuilder();
                        string[] array = File.ReadAllLines(dtcFilePath);

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i].Contains("=") && !array[i].Contains("None") && !array[i].Contains("END") && !array[i].Contains("OBDII"))
                            {
                                builder.Append(array[i]);
                                builder.Append(Environment.NewLine);
                                builder.Append(array[i + 1]);
                                builder.Append(Environment.NewLine);

                                for (int j = i + 2; j < i + 10; j++)
                                {
                                    if (array[j].Contains("Lamp is On") || array[j].Contains("Lamp is Off"))
                                    {
                                        builder.Append(array[j]);
                                        builder.Append(Environment.NewLine);
                                    }
                                }
                            }
                        }

                        dtcFileText = builder.ToString();

                        if (dtcFileText == "")
                        {
                            dtcFileText = "Successful DTC reading, no error codes found!";
                        }
                    }

                    if (dtcFileText != "" && dtcFileText != null)
                    {
                        try
                        {
                            int spaceAfter = 6;
                            //int spaceAfterDTC = 0;

                            object omissing = Missing.Value;

                            object dokumansonu = "\\endofdoc";

                            word.Application oWord;
                            word.Document internalText;

                            oWord = new word.Application();
                            oWord.Visible = false;
                            internalText = oWord.Documents.Add(ref omissing);

                            word.Paragraph paragraphHeading;
                            paragraphHeading = internalText.Content.Paragraphs.Add(ref omissing);
                            paragraphHeading.Range.Text = "FORD Club Azerbaijan";
                            paragraphHeading.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                            paragraphHeading.Range.Font.Bold = 1;
                            paragraphHeading.Range.Font.Size = 20;
                            paragraphHeading.Range.Font.Name = "Times New Roman";
                            paragraphHeading.Format.SpaceAfter = 10;
                            paragraphHeading.Range.InsertParagraphAfter();

                            word.Paragraph paragraphResult;
                            object hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphResult = internalText.Content.Paragraphs.Add(ref hedef);
                            paragraphResult.Range.Text = "Diaqnostika Nəticəsi:";
                            paragraphResult.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphResult.Range.Font.Bold = 1;
                            paragraphResult.Range.Font.Size = 16;
                            paragraphResult.Range.Font.Name = "Times New Roman";
                            paragraphResult.Format.SpaceAfter = 10;
                            paragraphResult.Range.InsertParagraphAfter();


                            word.Paragraph paragraphDateTime;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDateTime = internalText.Content.Paragraphs.Add(ref hedef);
                            string timeText = "Tarix - " + DateTime.Now;
                            paragraphDateTime.Range.Text = timeText;
                            paragraphDateTime.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDateTime.Range.Font.Bold = 0;
                            paragraphDateTime.Range.Font.Size = 12;
                            paragraphDateTime.Range.Font.Name = "Times New Roman";
                            paragraphDateTime.Format.SpaceAfter = spaceAfter;

                            object objTimeStart = paragraphDateTime.Range.Start;
                            object objTimeEnd = paragraphDateTime.Range.Start + timeText.IndexOf("-");
                            word.Range rngTimeBold = internalText.Range(ref objTimeStart, ref objTimeEnd);
                            rngTimeBold.Bold = 1;

                            paragraphDateTime.Range.InsertParagraphAfter();


                            word.Paragraph paragraphName;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphName = internalText.Content.Paragraphs.Add(ref hedef);
                            string nameText = "Ad - " + name;
                            paragraphName.Range.Text = nameText;
                            paragraphName.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphName.Range.Font.Bold = 0;
                            paragraphName.Range.Font.Size = 12;
                            paragraphName.Range.Font.Name = "Times New Roman";
                            paragraphName.Format.SpaceAfter = spaceAfter;

                            object objNameStart = paragraphName.Range.Start;
                            object objNameEnd = paragraphName.Range.Start + nameText.IndexOf("-");
                            word.Range rngNameBold = internalText.Range(ref objNameStart, ref objNameEnd);
                            rngNameBold.Bold = 1;

                            paragraphName.Range.InsertParagraphAfter();


                            word.Paragraph paragraphPhone;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphPhone = internalText.Content.Paragraphs.Add(ref hedef);
                            string phoneText = "Tel - " + phone;
                            paragraphPhone.Range.Text = phoneText;
                            paragraphPhone.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphPhone.Range.Font.Bold = 0;
                            paragraphPhone.Range.Font.Size = 12;
                            paragraphPhone.Range.Font.Name = "Times New Roman";
                            paragraphPhone.Format.SpaceAfter = spaceAfter;

                            object objPhoneStart = paragraphPhone.Range.Start;
                            object objPhoneEnd = paragraphPhone.Range.Start + phoneText.IndexOf("-");
                            word.Range rngPhoneBold = internalText.Range(ref objPhoneStart, ref objPhoneEnd);
                            rngPhoneBold.Bold = 1;

                            paragraphPhone.Range.InsertParagraphAfter();


                            word.Paragraph paragraphVin;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphVin = internalText.Content.Paragraphs.Add(ref hedef);
                            string vinText = "Vin - " + txt_Vin.Text;
                            paragraphVin.Range.Text = vinText;
                            paragraphVin.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphVin.Range.Font.Bold = 0;
                            paragraphVin.Range.Font.Size = 12;
                            paragraphVin.Range.Font.Name = "Times New Roman";
                            paragraphVin.Format.SpaceAfter = spaceAfter;

                            object objVinStart = paragraphVin.Range.Start;
                            object objVinEnd = paragraphVin.Range.Start + vinText.IndexOf("-");
                            word.Range rngVinBold = internalText.Range(ref objVinStart, ref objVinEnd);
                            rngVinBold.Bold = 1;

                            paragraphVin.Range.InsertParagraphAfter();


                            word.Paragraph paragraphModel;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphModel = internalText.Content.Paragraphs.Add(ref hedef);
                            string modelText = "Model - " + model;
                            paragraphModel.Range.Text = modelText;
                            paragraphModel.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphModel.Range.Font.Bold = 0;
                            paragraphModel.Range.Font.Size = 12;
                            paragraphModel.Range.Font.Name = "Times New Roman";
                            paragraphModel.Format.SpaceAfter = spaceAfter;

                            object objModelStart = paragraphModel.Range.Start;
                            object objModelEnd = paragraphModel.Range.Start + modelText.IndexOf("-");
                            word.Range rngModelBold = internalText.Range(ref objModelStart, ref objModelEnd);
                            rngModelBold.Bold = 1;

                            paragraphModel.Range.InsertParagraphAfter();


                            word.Paragraph paragraphEngine;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphEngine = internalText.Content.Paragraphs.Add(ref hedef);
                            string engineText = "Müherrik - " + engine;
                            paragraphEngine.Range.Text = engineText;
                            paragraphEngine.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphEngine.Range.Font.Bold = 0;
                            paragraphEngine.Range.Font.Size = 12;
                            paragraphEngine.Range.Font.Name = "Times New Roman";
                            paragraphEngine.Format.SpaceAfter = spaceAfter;

                            object objEngineStart = paragraphEngine.Range.Start;
                            object objEngineEnd = paragraphEngine.Range.Start + engineText.IndexOf("-");
                            word.Range rngEngineBold = internalText.Range(ref objEngineStart, ref objEngineEnd);
                            rngEngineBold.Bold = 1;

                            paragraphEngine.Range.InsertParagraphAfter();


                            word.Paragraph paragraphPlate;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphPlate = internalText.Content.Paragraphs.Add(ref hedef);
                            string plateText = "Nömrə - " + plate;
                            paragraphPlate.Range.Font.Bold = 0;
                            paragraphPlate.Range.Text = plateText;
                            paragraphPlate.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphPlate.Range.Font.Size = 12;
                            paragraphPlate.Range.Font.Name = "Times New Roman";
                            paragraphPlate.Format.SpaceAfter = spaceAfter;


                            object objPlateStart = paragraphPlate.Range.Start;
                            object objPlateEnd = paragraphPlate.Range.Start + plateText.IndexOf("-");
                            word.Range rngPlateBold = internalText.Range(ref objPlateStart, ref objPlateEnd);
                            rngPlateBold.Bold = 1;

                            paragraphPlate.Range.InsertParagraphAfter();

                            word.Paragraph paragraphDistance;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDistance = internalText.Content.Paragraphs.Add(ref hedef);
                            string distanceText = "Yürüş - " + distance;
                            paragraphDistance.Range.Font.Bold = 0;
                            paragraphDistance.Range.Text = distanceText;
                            paragraphDistance.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDistance.Range.Font.Size = 12;
                            paragraphDistance.Range.Font.Name = "Times New Roman";
                            paragraphDistance.Format.SpaceAfter = spaceAfter;

                            object objDistanceStart = paragraphDistance.Range.Start;
                            object objDistanceEnd = paragraphDistance.Range.Start + distanceText.IndexOf("-");
                            word.Range rngDistanceBold = internalText.Range(ref objDistanceStart, ref objDistanceEnd);
                            rngDistanceBold.Bold = 1;

                            paragraphDistance.Range.InsertParagraphAfter();

                            word.Paragraph paragraphDescription;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDescription = internalText.Content.Paragraphs.Add(ref hedef);
                            string descText = "İzahı - " + txt_Description.Text;
                            paragraphDescription.Range.Font.Bold = 0;
                            paragraphDescription.Range.Text = descText;
                            paragraphDescription.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDescription.Range.Font.Size = 12;
                            paragraphDescription.Range.Font.Name = "Times New Roman";
                            paragraphDescription.Format.SpaceAfter = 10;

                            object objDescStart = paragraphDescription.Range.Start;
                            object objDescEnd = paragraphDescription.Range.Start + descText.IndexOf("-");
                            word.Range rngDescBold = internalText.Range(ref objDescStart, ref objDescEnd);
                            rngDescBold.Bold = 1;

                            paragraphDescription.Range.InsertParagraphAfter();


                            word.Paragraph paragraphDTCHeading;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDTCHeading = internalText.Content.Paragraphs.Add(ref hedef);
                            paragraphDTCHeading.Range.Font.Bold = 1;
                            paragraphDTCHeading.Range.Text = "All DIAGNOSTIC TROUBLE CODE";
                            paragraphDTCHeading.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDTCHeading.Range.Font.Size = 12;
                            paragraphDTCHeading.Range.Font.Name = "Times New Roman";
                            paragraphDTCHeading.Format.SpaceAfter = 0;
                            paragraphDTCHeading.Range.InsertParagraphAfter();

                            word.Paragraph paragraphDTC;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDTC = internalText.Content.Paragraphs.Add(ref hedef);
                            paragraphDTC.Range.Font.Bold = 0;
                            paragraphDTC.Range.Text = dtcFileText;
                            paragraphDTC.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDTC.Range.Font.Size = 12;
                            paragraphDTC.Range.Font.Name = "Times New Roman";
                            paragraphDTC.Format.SpaceAfter = 0;
                            paragraphDTC.Range.InsertParagraphAfter();

                            string savePath = @"C:\ClientManagementDocuments\document" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".docx";

                            internalText.SaveAs2(savePath);

                            oWord.Quit();

                            System.Diagnostics.Process.Start(savePath);
                            btnPrint.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            btnPrint.Enabled = true;
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        int spaceAfter = 6;
                        try
                        {
                            object omissing = Missing.Value;

                            object dokumansonu = "\\endofdoc";

                            word.Application oWord;
                            word.Document internalText;

                            oWord = new word.Application();
                            oWord.Visible = false;
                            internalText = oWord.Documents.Add(ref omissing);

                            word.Paragraph paragraphHeading;
                            paragraphHeading = internalText.Content.Paragraphs.Add(ref omissing);
                            paragraphHeading.Range.Text = "FORD Club Azerbaijan";
                            paragraphHeading.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphCenter;
                            paragraphHeading.Range.Font.Bold = 1;
                            paragraphHeading.Range.Font.Size = 20;
                            paragraphHeading.Range.Font.Name = "Times New Roman";
                            paragraphHeading.Format.SpaceAfter = spaceAfter;
                            paragraphHeading.Range.InsertParagraphAfter();

                            word.Paragraph paragraphResult;
                            object hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphResult = internalText.Content.Paragraphs.Add(ref hedef);
                            paragraphResult.Range.Text = "Diaqnostika Nəticəsi:";
                            paragraphResult.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphResult.Range.Font.Bold = 1;
                            paragraphResult.Range.Font.Size = 16;
                            paragraphResult.Range.Font.Name = "Times New Roman";
                            paragraphResult.Format.SpaceAfter = spaceAfter;
                            paragraphResult.Range.InsertParagraphAfter();


                            word.Paragraph paragraphDateTime;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDateTime = internalText.Content.Paragraphs.Add(ref hedef);
                            string timeText = "Tarix - " + DateTime.Now;
                            paragraphDateTime.Range.Text = timeText;
                            paragraphDateTime.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDateTime.Range.Font.Bold = 0;
                            paragraphDateTime.Range.Font.Size = 12;
                            paragraphDateTime.Range.Font.Name = "Times New Roman";
                            paragraphDateTime.Format.SpaceAfter = spaceAfter;

                            object objTimeStart = paragraphDateTime.Range.Start;
                            object objTimeEnd = paragraphDateTime.Range.Start + timeText.IndexOf("-");
                            word.Range rngTimeBold = internalText.Range(ref objTimeStart, ref objTimeEnd);
                            rngTimeBold.Bold = 1;

                            paragraphDateTime.Range.InsertParagraphAfter();


                            word.Paragraph paragraphName;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphName = internalText.Content.Paragraphs.Add(ref hedef);
                            string nameText = "Ad - " + name;
                            paragraphName.Range.Text = nameText;
                            paragraphName.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphName.Range.Font.Bold = 0;
                            paragraphName.Range.Font.Size = 12;
                            paragraphName.Range.Font.Name = "Times New Roman";
                            paragraphName.Format.SpaceAfter = spaceAfter;

                            object objNameStart = paragraphName.Range.Start;
                            object objNameEnd = paragraphName.Range.Start + nameText.IndexOf("-");
                            word.Range rngNameBold = internalText.Range(ref objNameStart, ref objNameEnd);
                            rngNameBold.Bold = 1;

                            paragraphName.Range.InsertParagraphAfter();


                            word.Paragraph paragraphPhone;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphPhone = internalText.Content.Paragraphs.Add(ref hedef);
                            string phoneText = "Tel - " + phone;
                            paragraphPhone.Range.Text = phoneText;
                            paragraphPhone.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphPhone.Range.Font.Bold = 0;
                            paragraphPhone.Range.Font.Size = 12;
                            paragraphPhone.Range.Font.Name = "Times New Roman";
                            paragraphPhone.Format.SpaceAfter = spaceAfter;

                            object objPhoneStart = paragraphPhone.Range.Start;
                            object objPhoneEnd = paragraphPhone.Range.Start + phoneText.IndexOf("-");
                            word.Range rngPhoneBold = internalText.Range(ref objPhoneStart, ref objPhoneEnd);
                            rngPhoneBold.Bold = 1;

                            paragraphPhone.Range.InsertParagraphAfter();


                            word.Paragraph paragraphVin;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphVin = internalText.Content.Paragraphs.Add(ref hedef);
                            string vinText = "Vin - " + txt_Vin.Text;
                            paragraphVin.Range.Text = vinText;
                            paragraphVin.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphVin.Range.Font.Bold = 0;
                            paragraphVin.Range.Font.Size = 12;
                            paragraphVin.Range.Font.Name = "Times New Roman";
                            paragraphVin.Format.SpaceAfter = spaceAfter;

                            object objVinStart = paragraphVin.Range.Start;
                            object objVinEnd = paragraphVin.Range.Start + vinText.IndexOf("-");
                            word.Range rngVinBold = internalText.Range(ref objVinStart, ref objVinEnd);
                            rngVinBold.Bold = 1;

                            paragraphVin.Range.InsertParagraphAfter();


                            word.Paragraph paragraphModel;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphModel = internalText.Content.Paragraphs.Add(ref hedef);
                            string modelText = "Model - " + model;
                            paragraphModel.Range.Text = modelText;
                            paragraphModel.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphModel.Range.Font.Bold = 0;
                            paragraphModel.Range.Font.Size = 12;
                            paragraphModel.Range.Font.Name = "Times New Roman";
                            paragraphModel.Format.SpaceAfter = spaceAfter;

                            object objModelStart = paragraphModel.Range.Start;
                            object objModelEnd = paragraphModel.Range.Start + modelText.IndexOf("-");
                            word.Range rngModelBold = internalText.Range(ref objModelStart, ref objModelEnd);
                            rngModelBold.Bold = 1;

                            paragraphModel.Range.InsertParagraphAfter();


                            word.Paragraph paragraphEngine;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphEngine = internalText.Content.Paragraphs.Add(ref hedef);
                            string engineText = "Müherrik - " + engine;
                            paragraphEngine.Range.Text = engineText;
                            paragraphEngine.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphEngine.Range.Font.Bold = 0;
                            paragraphEngine.Range.Font.Size = 12;
                            paragraphEngine.Range.Font.Name = "Times New Roman";
                            paragraphEngine.Format.SpaceAfter = spaceAfter;

                            object objEngineStart = paragraphEngine.Range.Start;
                            object objEngineEnd = paragraphEngine.Range.Start + engineText.IndexOf("-");
                            word.Range rngEngineBold = internalText.Range(ref objEngineStart, ref objEngineEnd);
                            rngEngineBold.Bold = 1;

                            paragraphEngine.Range.InsertParagraphAfter();


                            word.Paragraph paragraphPlate;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphPlate = internalText.Content.Paragraphs.Add(ref hedef);
                            string plateText = "Nömrə - " + plate;
                            paragraphPlate.Range.Font.Bold = 0;
                            paragraphPlate.Range.Text = plateText;
                            paragraphPlate.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphPlate.Range.Font.Size = 12;
                            paragraphPlate.Range.Font.Name = "Times New Roman";
                            paragraphPlate.Format.SpaceAfter = spaceAfter;

                            object objPlateStart = paragraphPlate.Range.Start;
                            object objPlateEnd = paragraphPlate.Range.Start + plateText.IndexOf("-");
                            word.Range rngPlateBold = internalText.Range(ref objPlateStart, ref objPlateEnd);
                            rngPlateBold.Bold = 1;

                            paragraphPlate.Range.InsertParagraphAfter();

                            word.Paragraph paragraphDistance;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDistance = internalText.Content.Paragraphs.Add(ref hedef);
                            string distanceText = "Yürüş - " + distance;
                            paragraphDistance.Range.Font.Bold = 0;
                            paragraphDistance.Range.Text = distanceText;
                            paragraphDistance.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDistance.Range.Font.Size = 12;
                            paragraphDistance.Range.Font.Name = "Times New Roman";
                            paragraphDistance.Format.SpaceAfter = spaceAfter;

                            object objDistanceStart = paragraphDistance.Range.Start;
                            object objDistanceEnd = paragraphDistance.Range.Start + distanceText.IndexOf("-");
                            word.Range rngDistanceBold = internalText.Range(ref objDistanceStart, ref objDistanceEnd);
                            rngDistanceBold.Bold = 1;

                            paragraphDistance.Range.InsertParagraphAfter();


                            word.Paragraph paragraphDescription;
                            hedef = internalText.Bookmarks.get_Item(ref dokumansonu).Range;
                            paragraphDescription = internalText.Content.Paragraphs.Add(ref hedef);
                            string descText = "İzahı - " + txt_Description.Text;
                            paragraphDescription.Range.Font.Bold = 0;
                            paragraphDescription.Range.Text = descText;
                            paragraphDescription.Range.ParagraphFormat.Alignment = word.WdParagraphAlignment.wdAlignParagraphLeft;
                            paragraphDescription.Range.Font.Size = 12;
                            paragraphDescription.Range.Font.Name = "Times New Roman";
                            paragraphDescription.Format.SpaceAfter = spaceAfter;

                            object objDescStart = paragraphDescription.Range.Start;
                            object objDescEnd = paragraphDescription.Range.Start + descText.IndexOf("-");
                            word.Range rngDescBold = internalText.Range(ref objDescStart, ref objDescEnd);
                            rngDescBold.Bold = 1;

                            paragraphDescription.Range.InsertParagraphAfter();

                            string savePath = @"C:\ClientManagementDocuments\document" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".docx";

                            internalText.SaveAs2(savePath);

                            oWord.Quit();

                            System.Diagnostics.Process.Start(savePath);
                            btnPrint.Enabled = true;
                        }
                        catch (Exception ex)
                        {
                            btnPrint.Enabled = true;
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    btnPrint.Enabled = true;
                    MessageBox.Show("Please, Select client!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                btnPrint.Enabled = true;
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Importtant sql
        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection connection = AbstractDAO.Connect();
            try
            {
                Clear();
                CDGridView.Rows.Clear();

                connection.Open();
                string query = "select Cars_tbl.Id, " +
                    "Description_tbl.Description, Description_tbl.Datetime from Cars_tbl, Description_tbl " +
                    "where Cars_tbl.Id = Description_tbl.CarId";


                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader dataReader = cmd.ExecuteReader();


                int index = 0;


                while (dataReader.Read())
                {
                    index = CDGridView.Rows.Add();
                    CDGridView.Rows[index].Cells[0].Value = dataReader["Id"].ToString();
                    CDGridView.Rows[index].Cells[8].Value = dataReader["Description"].ToString();
                    CDGridView.Rows[index].Cells[10].Value = dataReader["Datetime"].ToString();
                }
                connection.Close();

                lblTotalCount.Text = GetTotal().ToString();
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
        private void button2_Click(object sender, EventArgs e)
        {
            int id;
            string description = "";
            string distance = "";
            DateTime dateTime;

            int index = default;
            List<TestFord> testFords = new List<TestFord>();
            //int row = 0;
            for (int i = 242; i < CDGridView.RowCount; i++)
            {
                id = Convert.ToInt32(CDGridView.Rows[i].Cells[0].Value);
                description = CDGridView.Rows[i].Cells[8].Value.ToString();
                dateTime = DateTime.Parse(CDGridView.Rows[i].Cells[10].Value.ToString());


                index = description.IndexOf("-km");
                if (index != -1)
                {
                    distance = description.Substring(0, index);
                    description = description.Remove(0, index + 4);
                }
                testFords.Add(new TestFord(id, distance, description, dateTime));
            }

            SqlConnection connection = AbstractDAO.Connect();
            connection.Open();
            SqlCommand cmd3;
            //string query3 = "Update Description_tbl Set Description = @desc, Distance = @distance  Where CarId=@id and Datetime = @datetime";
            string query2 = "Update Description_tbl Set Description = @desc, Distance = @distance Where Datetime = @datetime";
            cmd3 = new SqlCommand(query2, connection);

            for (int i = 0; i < testFords.Count; i++)
            {
                try
                {
                    cmd3.Parameters.AddWithValue("carId", testFords[i].id);
                    cmd3.Parameters.AddWithValue("@datetime", testFords[i].dateTime);
                    cmd3.Parameters.AddWithValue("@distance", testFords[i].distance);
                    cmd3.Parameters.AddWithValue("@desc", testFords[i].description);

                    //cmd3.Parameters.AddWithValue("carId", SqlDbType.Int).Value = testFords[i].id;
                    //cmd3.Parameters.AddWithValue("@datetime", SqlDbType.DateTime).Value = testFords[i].dateTime;
                    //cmd3.Parameters.AddWithValue("@distance", SqlDbType.Text).Value = testFords[i].distance;
                    //cmd3.Parameters.AddWithValue("@desc", SqlDbType.Text).Value = testFords[i].description;


                    cmd3.ExecuteNonQuery();
                    cmd3.Parameters.Clear();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //throw ex;
                }
                //finally
                //{
                //    connection.Close();
                //}
            }
            connection.Close();

            MessageBox.Show("Success");
        }

        #endregion

        private void txt_Year_KeyPress(object sender, KeyPressEventArgs e)
        {
            int asciiCode = Convert.ToInt32(e.KeyChar);

            if (asciiCode != 8)
            {
                if ((asciiCode >= 47 && asciiCode <= 57))
                {                    
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("Year correct variant Year/Month!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Handled = true;
                }
            }
        }

        private bool DesignYearText(string year)
        {
            bool control = false;
            if (txt_Year.Text.Length == 7)
            {
                if (txt_Year.Text[4] != '/')
                {
                    MessageBox.Show("Year correct variant Year/Month!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Year.Text = string.Empty;
                    control = true;
                }                 
            }
            else
            {
                control = false;
            }
            return control;
        }       
    }

}


