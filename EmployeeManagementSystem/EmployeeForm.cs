using System;
using System.Data;
using System.Windows.Forms;
using MISProject;
using MySql.Data.MySqlClient;

namespace EmployeeManagementSystem
{
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
            LoadEmployees();
        }
        private void LoadEmployees()
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT Employees.Id, Employees.Name, Employees.Age, Departments.Name AS Department FROM Employees JOIN Departments ON Employees.DepartmentId = Departments.Id order by Employees.Id asc", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                using (MySqlConnection con = DatabaseHelper.GetConnection())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM Employees WHERE Id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadEmployees();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
                using (MySqlConnection con = DatabaseHelper.GetConnection())
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("Update Employees SET Name = @name, Age = @age, DepartmentId = @dept WHERE Id=@id", con);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                    cmd.Parameters.AddWithValue("@dept", int.Parse(txtDeptId.Text));
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadEmployees();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = DatabaseHelper.GetConnection())
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Employees (Name, Age, DepartmentId) VALUES (@name, @age, @dept)", con);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", int.Parse(txtAge.Text));
                cmd.Parameters.AddWithValue("@dept", int.Parse(txtDeptId.Text));
                cmd.ExecuteNonQuery();
            }
            LoadEmployees();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtSalary_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
