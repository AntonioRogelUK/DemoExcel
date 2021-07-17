using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoExcel
{
    public partial class Form1 : Form
    {
        private Excel excel;
        public Form1()
        {
            excel = new Excel("Database.xlsx");
            InitializeComponent();
            txtId.ReadOnly = true;
        }

        private void btnObtenerDatos_Click(object sender, EventArgs e)
        {
            try
            {
                if (excel.ProbarConexion()) 
                {
                    dgvDatos.DataSource = excel.ObtenerDataTable("SELECT * FROM [Datos$]");
                    int.TryParse(excel.Scalar("SELECT Count(*) FROM [Datos$]").ToString(), out int CuentaContactos);
                    lblContactos.Text = $"{CuentaContactos} Contactos";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNuevoRegistro_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNombre.Text)) 
                {
                    txtNombre.Focus();
                    throw new Exception("El nombre es requerido"); 
                }

                if (string.IsNullOrWhiteSpace(txtTelefono.Text)) 
                {
                    txtTelefono.Focus();
                    throw new Exception("El teléfono es requerido"); 
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text)) 
                {
                    txtEmail.Focus();
                    throw new Exception("El Email es requerido"); 
                }

                int.TryParse(excel.Scalar("SELECT Count(*) FROM [Datos$]").ToString(), out int CuentaContactos);

                int afectados = excel.NonQuery($"INSERT INTO [Datos$] " +
                    $"(Id, Nombre, Telefono, Email) VALUES " +
                    $"({CuentaContactos + 1},'{txtNombre.Text}', '{txtTelefono.Text}','{txtEmail.Text}')");


                foreach(var control in this.Controls) 
                {
                    if (control is TextBox) 
                    {
                        ((TextBox)control).Clear();
                    }
                }

                btnObtenerDatos.PerformClick();

                lblContactos.Text += $"   '{afectados}' registros afectados";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void dgvDatos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int renglon = e.RowIndex;
                if (renglon < 0) { throw new Exception("No hay registros"); }

                txtId.Text = dgvDatos.Rows[renglon].Cells["Id"].Value.ToString();
                txtNombre.Text = dgvDatos.Rows[renglon].Cells["Nombre"].Value.ToString();
                txtTelefono.Text = dgvDatos.Rows[renglon].Cells["Telefono"].Value.ToString();
                txtEmail.Text = dgvDatos.Rows[renglon].Cells["Email"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    throw new Exception("El id es requerido");
                }

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    txtNombre.Focus();
                    throw new Exception("El nombre es requerido");
                }

                if (string.IsNullOrWhiteSpace(txtTelefono.Text))
                {
                    txtTelefono.Focus();
                    throw new Exception("El teléfono es requerido");
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    txtEmail.Focus();
                    throw new Exception("El Email es requerido");
                }

                int afectados = excel.NonQuery($"UPDATE [Datos$] " +
                    $" SET " +
                    $"Nombre = '{txtNombre.Text}', " +
                    $"Telefono = '{txtTelefono.Text}', " +
                    $"Email = '{txtEmail.Text}' " +
                    $"WHERE Id = {txtId.Text}");


                foreach (var control in this.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Clear();
                    }
                }

                btnObtenerDatos.PerformClick();

                lblContactos.Text += $"   '{afectados}' registros afectados";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    throw new Exception("El id es requerido");
                }

                int afectados = excel.NonQuery($"DELETE * FROM [Datos$] " +
                    $"WHERE Id = {txtId.Text}");


                foreach (var control in this.Controls)
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).Clear();
                    }
                }

                btnObtenerDatos.PerformClick();

                lblContactos.Text += $"   '{afectados}' registros afectados";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnObtenerDatos.PerformClick();
        }
    }
}