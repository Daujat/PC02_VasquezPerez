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
    public partial class registroOrdenes : Form
    {
        public registroOrdenes()
        {
            InitializeComponent();
        }

        // METODO CARGAR REGISTROS POR FECHAS
        void cargarRegistrosPorRangoFechas()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("ObtenerPedidosPorRangoFechas", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros del DateTimePicker
                    cmd.Parameters.AddWithValue("@FechaInicio", dtpFechaInicial.Value.ToString("dd/MM/yyyy"));
                    cmd.Parameters.AddWithValue("@FechaFin", dtpFechaFinal.Value.ToString("dd/MM/yyyy"));

                    // Obtener el Id del Empleado seleccionado en el ComboBox
                    int idEmpleado = (int)comboxEmpleados.SelectedValue;
                    cmd.Parameters.AddWithValue("@IdEmpleado", idEmpleado);

                    SqlDataReader dr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    int totalRegistros = 0;

                    while (dr.Read())
                    {
                        object[] rowValues = new object[dr.FieldCount];
                        dr.GetValues(rowValues);
                        dataGridView1.Rows.Add(rowValues);
                        totalRegistros++;
                    }

                    // Actualizar el TextBox con el total de registros
                    txtTotalRegistros.Text = $"{totalRegistros} registros";

                    // Mostrar mensaje si no hay registros
                    if (totalRegistros == 0)
                    {
                        MessageBox.Show("No existen registros en ese intervalo de tiempo para el empleado seleccionado.");
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los registros: " + ex.Message);
            }
        }

        //METODO CARGAS TODOS LOS REGISTROS
        void cargarTodosLosRegistros()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();

                    SqlCommand cmd = new SqlCommand("ObtenerTodosLosPedidos", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dr = cmd.ExecuteReader();
                    dataGridView1.Rows.Clear();
                    int totalRegistros = 0;

                    while (dr.Read())
                    {
                        object[] rowValues = new object[dr.FieldCount];
                        dr.GetValues(rowValues);
                        dataGridView1.Rows.Add(rowValues);
                        totalRegistros++;
                    }

                    // Actualizar el TextBox con el total de registros
                    txtTotalRegistros.Text = $"{totalRegistros} registros";

                    // Mostrar mensaje si no hay registros
                    if (totalRegistros == 0)
                    {
                        MessageBox.Show("No existen registros en la base de datos.");
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar todos los registros: " + ex.Message);
            }
        }

        // CARGAR LOS EMPLEADOS EN EL COMBOBOX
        void cargarEmpleados()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(Properties.Settings.Default.cnx))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT IdEmpleado, CONCAT(NomEmpleado, ' ', ApeEmpleado) AS Empleado FROM EMPLEADO", cn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    Dictionary<int, string> empleados = new Dictionary<int, string>();

                    while (dr.Read())
                    {
                        empleados.Add((int)dr["IdEmpleado"], dr["Empleado"].ToString());
                    }

                    // Asignar los empleados al ComboBox
                    comboxEmpleados.DataSource = new BindingSource(empleados, null);
                    comboxEmpleados.DisplayMember = "Value";
                    comboxEmpleados.ValueMember = "Key";
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los empleados: " + ex.Message);
            }
        }


        private void btnMostrar_Click(object sender, EventArgs e)
        {
            cargarRegistrosPorRangoFechas();
        }

        private void btnMostrarTodo_Click(object sender, EventArgs e)
        {
            cargarTodosLosRegistros();
        }

        private void comboxEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void registroOrdenes_Load(object sender, EventArgs e)
        {
            cargarEmpleados();
        }
    }
}
