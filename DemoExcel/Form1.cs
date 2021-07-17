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
        public Form1()
        {
            InitializeComponent();
        }

        private void btnObtenerDatos_Click(object sender, EventArgs e)
        {
            try
            {
                Excel excel = new Excel("Database.xlsx");
                excel.ProbarConexion();

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
    }
}
