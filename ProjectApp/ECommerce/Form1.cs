using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ECommerce
{
    public partial class Form1 : Form
    {
        SqlCommand cmd = null;
        SqlDataAdapter adapt = null;
        public static int productId = 0;
        public Form1()
        {
            InitializeComponent();
            AddButtonColumn();
            AddButtonOrder();
            LoadEmp();
            ResetProduct();
            LoadOrder();
            ResetOrder();
        }
        SqlConnection con = new SqlConnection("data source=DESKTOP-03VU7SV; database =AdoProject; integrated security = true");
        int CategoryId = 0;
        public DataTable GetCategory()
        {
            DataSet dsData = new DataSet();
            {
                con.Open();
                string query = "SELECT [categoryId],[categoryName] FROM [categories]";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsData);
                con.Close();
                return dsData.Tables[0];
            }
        }
        private void btnBSave_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = null;
            con.Open();
            string insertQ = "INSERT INTO [categories]([categoryName]) VALUES('" + txtCatName.Text.Trim() + "')";
            cmd = new SqlCommand(insertQ, con);
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                MessageBox.Show("Data Inserted Succesfully!!!");
            }
            else
            {
                MessageBox.Show("Operation failed");
            }
            con.Close();

            ResetCategory();
            FillCmbCategory();
        }

        private void btnBUpdate_Click(object sender, EventArgs e)
        {
            string selectQ = "SELECT [categoryId],[categoryName] FROM [categories] WHERE [categoryId] = " + CategoryId;
            SqlCommand cmd = new SqlCommand(selectQ, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CategoryId = Convert.ToInt32(reader["categoryId"]);
                }
                reader.Close();
                string updateQ = "UPDATE [categories] SET [categoryName] = '" + txtCatName.Text.Trim() + "' WHERE [categoryId] = " + CategoryId;
                cmd = new SqlCommand(updateQ, con);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("UPDATE Succesfully...!!");
                }
                else
                {
                    MessageBox.Show("Operation failed.");
                }
            }
            else
            {
                MessageBox.Show("No data found.");
            }
            con.Close();
            ResetCategory();
            FillCmbCategory();
        }
        private void ResetCategory()
        {
            btnBSave.Show();
            btnBUpdate.Hide();
            txtCatName.Text = "";
            dataGridView1.DataSource = GetCategory();
            CategoryId = 0;
            string q = "SELECT e.[categoryId],e.[categoryName] FROM [categories] e ";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, "categories");
        }
        private void FillCmbCategory()
        {
            var cat = GetCategory();
            cmbCategory.DataSource = cat;
            cmbCategory.DisplayMember = "categoryName";
            cmbCategory.ValueMember = "categoryId";
        }
        private void FillCmbCustomer()
        {
            var customers = GetCustomer();
            cmbCustomer.DataSource = customers;
            cmbCustomer.DisplayMember = "customerName";
            cmbCustomer.ValueMember = "customerId";
        }
        private void FillCmbProduct()
        {
            var products = GetProduct();
            cmbProduct.DataSource = products;
            cmbProduct.DisplayMember = "productName";
            cmbProduct.ValueMember = "productId";
        }


        private void btnBDelete_Click(object sender, EventArgs e)
        {
            ResetCategory();
            FillCmbCategory();
        }
        private void AddButtonColumn()
        {
            
            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.HeaderText = "#";
            btnEdit.Text = "Edit";
            btnEdit.Name = "btnEdit";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "#";
            btnDelete.Text = "Delete";
            btnDelete.Name = "btnDelete";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(btnDelete);
        }
        private void LoadEmp()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [productId], [productName], [unitprice], [quantity] FROM [product]";
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            dataGridView3.DataSource = dt;
            con.Close();
        }
        public DataTable GetProduct()
        {
            DataSet dsData = new DataSet();
            {
                con.Open();
                string query = "SELECT [productId],[productName],[unitPrice],[quantity] FROM [product]";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsData);
                con.Close();
                return dsData.Tables[0];
            }
        }

        private void AddNew()
        {
            // save image
            
            string query = @"INSERT INTO [dbo].[product]
                ([productName]
               ,[unitPrice]
               ,[quantity]
               )
                VALUES
               (@productName
               ,@unitPrice
               ,@quantity
               )";
            cmd = new SqlCommand(query, con);
            con.Open();
            //cmd.Parameters.AddWithValue("@productId", txtID.Text.Trim());
            cmd.Parameters.AddWithValue("@productName", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@unitPrice", txtPrice.Text.Trim());
            cmd.Parameters.AddWithValue("@quantity", txtProQuantity.Text.Trim());           
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data added successfully.");
        }
        private void Updates()
        {
            cmd = new SqlCommand(@"UPDATE [product]
               SET [productName] = @productName
                  ,[unitPrice] = @unitPrice
                  ,[quantity] = @quantity
                  
             WHERE [productId] = @productId", con);
            con.Open();
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@productName", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@unitPrice", txtPrice.Text.Trim());
            cmd.Parameters.AddWithValue("@quantity", txtProQuantity.Text.Trim());           
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data updated successfully.");
        }
        private void Edit()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [productId]
                  ,[productName]
                  ,[unitPrice]
                  ,[quantity]                  
              FROM [product] WHERE [productId] = " + productId;
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                btnAdd.Text = "Update";

                productId = Convert.ToInt32(dt.Rows[0]["productId"].ToString());
                txtName.Text = dt.Rows[0]["productName"].ToString();
                txtPrice.Text = dt.Rows[0]["unitPrice"].ToString();
                txtProQuantity.Text = dt.Rows[0]["quantity"].ToString();                
            }
        }
        private void Delete()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to remove?", "Confirm Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                con.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT [productId]
                      ,[productName]
                      ,[unitPrice]
                      ,[quantity]
                     
                  FROM [product] WHERE [productId] = " + productId;
                adapt = new SqlDataAdapter(query, con);
                adapt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    
                    string q = @"DELETE FROM [product]
                    WHERE productId = @productId";
                    cmd = new SqlCommand(q, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@productId", productId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ResetProduct();
                    LoadEmp();
                    MessageBox.Show("Data removed successfully.");

                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (productId > 0)
            {
                Updates();
            }
            else
            {
                AddNew();
            }
            ResetProduct();
            LoadEmp();
            FillCmbProduct();
        }
        private void ResetProduct()
        {
            productId = 0;
            btnAdd.Text = "Add";
            txtName.Text = "";
            txtPrice.Text = "";
            txtProQuantity.Text = "";
            
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetProduct();
            FillCmbProduct();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetCategory();
            ResetProduct();
            ResetCustomer();
            ResetOrder();
            FillCmbCategory();
            FillCmbCustomer();
            FillCmbProduct();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // edit button
            {
                btnBSave.Hide();
                btnBUpdate.Show();
                string categoryId = dataGridView1.Rows[e.RowIndex].Cells["categoryId"].Value.ToString();
                string categoryName = dataGridView1.Rows[e.RowIndex].Cells["categoryName"].Value.ToString();
                Int32.TryParse(categoryId, out CategoryId);
                txtCatName.Text = categoryName;
            }

            if (e.ColumnIndex == 1) // delete button
            {
                string categoryId = dataGridView1.Rows[e.RowIndex].Cells["categoryId"].Value.ToString();
                string categoryName = dataGridView1.Rows[e.RowIndex].Cells["categoryName"].Value.ToString();
                Int32.TryParse(categoryId, out CategoryId);

                // example of connected architechture
                if (MessageBox.Show("Are you sure to delete '" + categoryName + "'", "Delete confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string selectQ = "SELECT [categoryId],[categoryName] FROM [categories] WHERE [categoryId] = " + CategoryId;
                    SqlCommand cmd = new SqlCommand(selectQ, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CategoryId = Convert.ToInt32(reader["categoryId"]);
                        }
                        reader.Close();

                        string deleteQ = "DELETE FROM [categories] WHERE [categoryId] = " + CategoryId;
                        cmd = new SqlCommand(deleteQ, con);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("DELETE Perform Succesfully.");
                        }
                        else
                        {
                            MessageBox.Show("Operation failed.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found.");
                    }
                    con.Close();
                    ResetCategory();
                }
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && dataGridView3.Rows.Count > e.RowIndex + 1)
            {
                var v = dataGridView3.Rows[e.RowIndex].Cells["productId"].Value;
                productId = dataGridView3.Rows[e.RowIndex].Cells["productId"].Value == null ? 0 : Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["productId"].Value);
                if ("Edit" == dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Edit();
                }
                if ("Delete" == dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Delete();
                }
            }
        }
        int CustomerId = 0;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0) // edit button
            {
                csSave.Hide();
                csUpdate.Show();
                string customerId = dataGridView2.Rows[e.RowIndex].Cells["customerId"].Value.ToString();
                string customerName = dataGridView2.Rows[e.RowIndex].Cells["customerName"].Value.ToString();
                string address = dataGridView2.Rows[e.RowIndex].Cells["address"].Value.ToString();
                string phone = dataGridView2.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                string email = dataGridView2.Rows[e.RowIndex].Cells["email"].Value.ToString();
                Int32.TryParse(customerId, out CustomerId);
                txtCustomerName.Text = customerName;
                txtCsAddress.Text = address;
                txtCsPhone.Text = phone;
                txtCsEmail.Text = email;
            }

            if (e.ColumnIndex == 1) // delete button
            {
                string customerId = dataGridView2.Rows[e.RowIndex].Cells["customerId"].Value.ToString();
                string customerName = dataGridView2.Rows[e.RowIndex].Cells["customerName"].Value.ToString();
                string address = dataGridView2.Rows[e.RowIndex].Cells["address"].Value.ToString();
                string phone = dataGridView2.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                string email = dataGridView2.Rows[e.RowIndex].Cells["email"].Value.ToString();
                Int32.TryParse(customerId, out CustomerId);

                // example of connected architechture
                if (MessageBox.Show("Are you sure to delete '" + customerName + "'", "Delete confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string selectQ = "SELECT [customerId],[customerName],[address],[phone],[email] FROM [customer] WHERE [customerId] = " + CustomerId;
                    SqlCommand cmd = new SqlCommand(selectQ, con);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            CustomerId = Convert.ToInt32(reader["customerId"]);
                        }
                        reader.Close();

                        string deleteQ = "DELETE FROM [customer] WHERE [customerId] = " + CustomerId;
                        cmd = new SqlCommand(deleteQ, con);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("DELETE Perform Succesfully.");
                        }
                        else
                        {
                            MessageBox.Show("Operation failed.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found.");
                    }
                    con.Close();
                    ResetCustomer();
                }
            }
        }

        private void csSave_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = null;
            con.Open();
            string csinsertQ = "INSERT INTO [customer]([customerName],[address],[phone],[email]) VALUES(@customerName,@address,@phone,@email)";
            cmd = new SqlCommand(csinsertQ, con);            
          //cmd.Parameters.AddWithValue("@proID", txtID.Text.Trim());
            cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtCsAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@phone", txtCsPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtCsEmail.Text.Trim());            
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data added successfully.");

            ResetCustomer();
            FillCmbCustomer();
        }
        public DataTable GetCustomer()
        {
            DataSet dsData = new DataSet();
            {
                con.Open();
                string query = "SELECT [customerId],[customerName],[address],[phone],[email] FROM [customer]";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dsData);
                con.Close();
                return dsData.Tables[0];
            }
        }
        private void ResetCustomer()
        {
            csSave.Show();
            csUpdate.Hide();
            txtCustomerName.Text = "";
            txtCsAddress.Text = "";
            txtCsEmail.Text = "";
            txtCsPhone.Text = "";
            dataGridView2.DataSource = GetCustomer();
            CustomerId = 0;
            string q = "SELECT e.[customerId],e.[customerName],e.[address],e.[phone],e.[email] FROM [customer] e";
            SqlCommand cmd = new SqlCommand(q, con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, "customer");
        }

        private void csReset_Click(object sender, EventArgs e)
        {
            ResetCustomer();
            FillCmbCustomer();
        }

        private void csUpdate_Click(object sender, EventArgs e)
        {
            string selectQ = "SELECT [customerId],[customerName],[address],[phone],[email] FROM [customer] WHERE [customerId] = " + CustomerId;
            SqlCommand cmd = new SqlCommand(selectQ, con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CustomerId = Convert.ToInt32(reader["customerId"]);
                }
                reader.Close();
                cmd = new SqlCommand(@"UPDATE [customer]
               SET [customerName] = @customerName
                  ,[address] = @address
                  ,[phone] = @phone
                  ,[email] = @email                  
             WHERE [customerId] = @customerId", con);                
                cmd.Parameters.AddWithValue("@customerId", CustomerId);
                cmd.Parameters.AddWithValue("@customerName", txtCustomerName.Text.Trim());
                cmd.Parameters.AddWithValue("@address", txtCsAddress.Text.Trim());               
                cmd.Parameters.AddWithValue("@phone", txtCsPhone.Text.Trim());               
                cmd.Parameters.AddWithValue("@email", txtCsEmail.Text.Trim());               
                cmd.ExecuteNonQuery();                

                MessageBox.Show("Data updated successfully.");

            }
            else
            {
                MessageBox.Show("No data found.");
            }
            con.Close();
            ResetCustomer();
            FillCmbCustomer();
        }
        public static int orderId = 0;

        private void orderSave_Click(object sender, EventArgs e)
        {
            if (orderId > 0)
            {
                OrderUpdates();
            }
            else
            {
                OrderAddNew();
            }
            ResetOrder();
            LoadOrder();
        }
        private void ResetOrder()
        {
            orderId = 0;
            orderSave.Text = "Add";
            newFilePath = "";
            pictureBox1.Image = null;
            using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.jpg"))
            {
                pictureBox1.Image = new Bitmap(img);
                lblFile.Text = "\\images\\default_img.jpg";
            }
            cmbCategory.Text = "";
            cmbProduct.Text = "";
            cmbCustomer.Text = "";
            txtorderQuantity.Text = "";            
            pickDoB.Text = "";
            radioIn.Checked = true;
            
        }
        private void LoadOrder()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT e.[orderId], c.[categoryName],d.[productName], f.[customerName],e.[quantity],e.[stock],e.[orderDate],e.[productPic] FROM [orderDetails] e
                JOIN [product] d ON e.productId = d.productId 
                Join [customer] f ON e.customerId=f.customerId
                Join [categories] c ON e.categoryId=c.categoryId";
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            dataGridView4.DataSource = dt;
            con.Close();
        }
        private void OrderAddNew()
        {
            // save image
            string strFilePath = AddFile();
            string query = @"INSERT INTO [dbo].[orderDetails]
                ([categoryId]
               ,[productId]
               ,[customerId]
               ,[quantity]
               ,[stock]
               ,[orderDate]
               ,[productPic])
                VALUES
               (@categoryId
               ,@productId
               ,@customerId
               ,@quantity
               ,@stock
               ,@orderDate
               ,@productPic)";
            cmd = new SqlCommand(query, con);
            con.Open();
            cmd.Parameters.AddWithValue("@categoryId", cmbCategory.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@productId", cmbProduct.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@customerId", cmbCustomer.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@quantity", txtorderQuantity.Text);
            cmd.Parameters.AddWithValue("@stock", radioIn.Checked == true ? "StockIn" : "StockOut");
            cmd.Parameters.AddWithValue("@orderDate", pickDoB.Value);
            cmd.Parameters.AddWithValue("@productPic", strFilePath);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data added successfully.");
        }
        private void OrderUpdates()
        {
            // save image
            string strFilePath = UpdateFile();

            cmd = new SqlCommand(@"UPDATE [orderDetails]
               SET [categoryId] = @categoryId
                  ,[productId] = @productId
                  ,[customerId] = @customerId
                  ,[stock] = @stock
                  ,[quantity] = @quantity
                  ,[orderDate] = @orderDate
             WHERE [orderId] = @orderId", con);
            con.Open();
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@categoryId", cmbCategory.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@productId", cmbProduct.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@customerId", cmbCustomer.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@quantity", txtorderQuantity.Text);
            cmd.Parameters.AddWithValue("@orderDate", pickDoB.Value);
            cmd.Parameters.AddWithValue("@stock", radioIn.Checked == true ? "StockIn" : "StockOut");
            cmd.Parameters.AddWithValue("@productPic", strFilePath);
            
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Data updated successfully.");
        }
        private void SelectFile()
        {
            open.Filter = "JPG (*.JPG)|*.jpg";
            if (open.ShowDialog() == DialogResult.OK)
            {
                using (var img = new Bitmap(open.FileName))
                {
                    pictureBox1.Image = new Bitmap(img);
                }
                // image file path
                newFilePath = open.FileName;
                isNewFile = true;
            }
        }
        string newFilePath = string.Empty;
        string oldFilePath = string.Empty;
        bool isNewFile = true;
        OpenFileDialog open = new OpenFileDialog();

        private string AddFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                File.Copy(newFilePath, Application.StartupPath + strFilePath);
            }

            return strFilePath;
        }
        private string UpdateFile()
        {
            string strFilePath = string.Empty;
            if (isNewFile)
            {
                strFilePath = "\\images\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                File.Copy(newFilePath, Application.StartupPath + strFilePath);

                //remove old file
                RemoveFile(Application.StartupPath + oldFilePath);
            }
            else
            {
                strFilePath = oldFilePath;
            }

            return strFilePath;
        }
        private void RemoveFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (!filePath.Contains("default"))
                {
                    File.Delete(filePath);
                }
                pictureBox1.Image = null;
            }
        }
        private void OrderEdit()
        {
            con.Open();
            DataTable dt = new DataTable();
            string query = @"SELECT [orderId]
                  ,[categoryId]
                  ,[productId]
                  ,[customerId]
                  ,[quantity]
                  ,[stock]
                  ,[orderDate]
                  ,[productPic]
              FROM [orderDetails] WHERE [orderId] = " + orderId;
            adapt = new SqlDataAdapter(query, con);
            adapt.Fill(dt);
            con.Close();

            if (dt.Rows.Count > 0)
            {
                orderSave.Text = "Update";

                orderId = Convert.ToInt32(dt.Rows[0]["orderId"].ToString());
                txtorderQuantity.Text = dt.Rows[0]["quantity"].ToString();                
                pickDoB.Value = Convert.ToDateTime(dt.Rows[0]["orderDate"].ToString());
                radioIn.Checked = dt.Rows[0]["stock"].ToString() == "StockIn" ? true : false;
                radioOut.Checked = dt.Rows[0]["stock"].ToString() == "StockIn" ? false : true;
                // set image
                if (dt.Rows[0]["productPic"].ToString() != null)
                {
                    if (File.Exists(Application.StartupPath + dt.Rows[0]["productPic"].ToString()))
                    {
                        using (var img = new Bitmap(Application.StartupPath + dt.Rows[0]["productPic"].ToString()))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = dt.Rows[0]["productPic"].ToString();
                            isNewFile = false;
                            oldFilePath = dt.Rows[0]["productPic"].ToString();
                        }
                    }
                    else
                    {
                        using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.jpg"))
                        {
                            pictureBox1.Image = new Bitmap(img);
                            lblFile.Text = "\\images\\default_img.jpg";
                        }
                    }
                }
                else
                {
                    using (var img = new Bitmap(Application.StartupPath + "\\images\\default_img.jpg"))
                    {
                        pictureBox1.Image = new Bitmap(img);
                        lblFile.Text = "\\images\\default_img.jpg";
                    }
                }
                cmbCategory .SelectedValue = dt.Rows[0]["categoryId"].ToString();
                cmbProduct.SelectedValue = dt.Rows[0]["productId"].ToString();
                cmbCustomer.SelectedValue = dt.Rows[0]["customerId"].ToString();
            }
        }
        private void OrderDelete()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to remove?", "Confirm Message", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                con.Open();
                DataTable dt = new DataTable();
                string query = @"SELECT [orderId]
                  ,[categoryId]
                  ,[productId]
                  ,[customerId]
                  ,[quantity]
                  ,[stock]
                  ,[orderDate]
                  ,[productPic]
              FROM [orderDetails] WHERE [orderId] = " + orderId;
                adapt = new SqlDataAdapter(query, con);
                adapt.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["productPic"] != null)
                    {
                        // remove old file
                        RemoveFile(Application.StartupPath + dt.Rows[0]["productPic"].ToString());
                    }

                    string q = @"DELETE FROM [orderDetails]
                    WHERE orderId = @orderId";
                    cmd = new SqlCommand(q, con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ResetProduct();
                    LoadOrder();
                    MessageBox.Show("Data removed successfully.");

                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void orderReset_Click(object sender, EventArgs e)
        {
            ResetOrder();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SelectFile();
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1 && dataGridView4.Rows.Count > e.RowIndex + 1)
            {
                var v = dataGridView4.Rows[e.RowIndex].Cells["orderId"].Value;
                orderId = dataGridView4.Rows[e.RowIndex].Cells["orderId"].Value == null ? 0 : Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells["orderId"].Value);
                if ("Report" == dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    Form2 f2 = new Form2();
                    f2.Show();
                }
                if ("Edit" == dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    OrderEdit();
                }
                if ("Delete" == dataGridView4.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString())
                {
                    OrderDelete();
                }
            }
        }
        private void AddButtonOrder()
        {
            DataGridViewButtonColumn btnReport = new DataGridViewButtonColumn();
            btnReport.HeaderText = "#";
            btnReport.Text = "Report";
            btnReport.Name = "btnReport";
            btnReport.UseColumnTextForButtonValue = true;
            dataGridView4.Columns.Add(btnReport);

            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
            btnEdit.HeaderText = "#";
            btnEdit.Text = "Edit";
            btnEdit.Name = "btnEdit";
            btnEdit.UseColumnTextForButtonValue = true;
            dataGridView4.Columns.Add(btnEdit);

            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
            btnDelete.HeaderText = "#";
            btnDelete.Text = "Delete";
            btnDelete.Name = "btnDelete";
            btnDelete.UseColumnTextForButtonValue = true;
            dataGridView4.Columns.Add(btnDelete);
        }


    }

}
