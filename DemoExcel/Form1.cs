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

                int afectados = excel.NonQuery($"INSERT INTO [Datos$] " +
                    $"(Nombre, Telefono, Email) VALUES " +
                    $"('{txtNombre.Text}', '{txtTelefono.Text}','{txtEmail.Text}')");


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
    }
}