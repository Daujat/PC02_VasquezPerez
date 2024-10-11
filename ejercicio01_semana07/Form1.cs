using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ejercicio01_semana07
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        void cargarDetallesOrden()
        {
            // Validación de que se haya ingresado un número de orden válido
            if (string.IsNullOrEmpty(txtboxOrden.Text))
            {
                MessageBox.Show("Por favor, ingresa un número de orden.");
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("ObtenerDetallesPedido", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumeroOrden", int.Parse(txtboxOrden.Text));
                    SqlDataReader dr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    decimal totalOrden = 0;

                    //Cargar datos
                    while (dr.Read())
                    {
                        object[] rowValues = new object[dr.FieldCount];
                        dr.GetValues(rowValues);
                        dataGridView1.Rows.Add(rowValues);

                        //Suma del Importe (última columna) al total de la orden
                        totalOrden += Convert.ToDecimal(dr["Importe"]);
                    }

                    // Mostrar el total de la orden en el TextBox
                    txtboxTotalOrden.Text = totalOrden.ToString("0.0000");
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los detalles de la orden: " + ex.Message);
            }
        }

        private void btnDetalleOrden_Click(object sender, EventArgs e)
        {
            cargarDetallesOrden();
        }
    }
}
