using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace webservice
{
    /// <summary>
    /// Summary description for wsDieta
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wsDieta : System.Web.Services.WebService
    {
        static string cadenaconexion = "Data Source=dietauh.database.windows.net;Initial Catalog=dietauh;Persist Security Info=True;User ID=cachacox;Password=cvjgut7tsg!";
        SqlConnection conexion = new SqlConnection(cadenaconexion);
        [WebMethod]
        public DataSet consultaUsuario(string id) 
        {
            DataSet tabla = new DataSet();
            SqlCommand comando = new SqlCommand();
            try
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "select * from usuarios where correo = @iduser";
                comando.Parameters.Add("@iduser", SqlDbType.NVarChar).Value = id;
                SqlDataAdapter adapto = new SqlDataAdapter();
                adapto.SelectCommand = comando;
                adapto.Fill(tabla);
                conexion.Close();
                return tabla;
            }
            catch (Exception ex)
            {
                conexion.Close();
                throw new Exception("Error en la consulta" + ex.Message);
            }
        }

        [WebMethod]
        public DataSet consultaProgreso(int id)
        {
            DataSet tabla = new DataSet();
            SqlCommand comando = new SqlCommand();
            try
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "select * from progreso where iduser = @iduser";
                comando.Parameters.Add("@iduser", SqlDbType.Int).Value = id;
                SqlDataAdapter adapto = new SqlDataAdapter();
                adapto.SelectCommand = comando;
                adapto.Fill(tabla);
                conexion.Close();
                return tabla;
            }
            catch (Exception ex)
            {
                conexion.Close();
                throw new Exception("Error en la consulta" + ex.Message);
            }
        }

        [WebMethod]
        public void insertarUsuario(string correo, string contrasena, string nombreusuario, int peso, int altura, int sexo, int frecuencia, float tmb, float imc, int edad, int kxp, string corporal)
        {
            SqlCommand comando = new SqlCommand();
            try
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "insert into usuarios values(@correo, @contrasena, @nombreusuario, @peso, @altura, @sexo, @frecuencia, @tmb, @imc, @edad, @kxp, @corporal)";
                comando.Parameters.Add("@correo", SqlDbType.NVarChar).Value = correo;
                comando.Parameters.Add("@contrasena", SqlDbType.NVarChar).Value = contrasena;
                comando.Parameters.Add("@nombreusuario", SqlDbType.NVarChar).Value = nombreusuario;
                comando.Parameters.Add("@peso", SqlDbType.Int).Value = peso;
                comando.Parameters.Add("@altura", SqlDbType.Int).Value = altura;
                comando.Parameters.Add("@sexo", SqlDbType.Int).Value = sexo;
                comando.Parameters.Add("@frecuencia", SqlDbType.Int).Value = frecuencia;
                comando.Parameters.Add("@tmb", SqlDbType.Float).Value = tmb;
                comando.Parameters.Add("@imc", SqlDbType.Float).Value = imc;
                comando.Parameters.Add("@edad", SqlDbType.Int).Value = edad;
                comando.Parameters.Add("@kxp", SqlDbType.Int).Value = kxp;
                comando.Parameters.Add("@corporal", SqlDbType.NVarChar).Value = corporal;
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conexion.Close();
                throw new Exception("Error al insertar Usuario" + ex.Message);
            }
        }

        [WebMethod]
        public void insertarProgreso(int id, int progreso_peso, int progreso_imc)
        {
            SqlCommand comando = new SqlCommand();
            DateTime fecha = DateTime.Now;
            
            try
            {
                conexion.Open();
                comando.Connection = conexion;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "insert into progreso values(@id, @progreso_peso, @progreso_imc, @fecha)";
                comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                comando.Parameters.Add("@progreso_peso", SqlDbType.Int).Value = progreso_peso;
                comando.Parameters.Add("@progreso_imc", SqlDbType.Int).Value = progreso_imc;
                comando.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conexion.Close();
                throw new Exception("Error al insertar Usuario" + ex.Message);
            }
        }

        [WebMethod]
        public double calculoIMC(int peso, int altura)
        {
            double indice = 0.0;
            double alturadouble = altura;
            alturadouble = alturadouble / 100;

            if (peso > 0 || altura > 0)
            {
                indice = Math.Round(((peso) / (Math.Pow(alturadouble, 2))), 2);
            }
            return indice;
        }

        [WebMethod]
        public double calculoTMB(int sexo, int altura, int peso, int frecuencia, int edad, int kxp)
        {
            double tmb = 0;
            int calNec = frecuencia;
            double multip = 0.0;
            int kg = 0;

            switch (calNec)
            {
                case 1:
                    multip = 1.2;
                    break;
                case 2:
                    multip = 1.375;
                    break;
                case 3:
                    multip = 1.55;
                    break;
                case 4:
                    multip = 1.725;
                    break;
                case 5:
                    multip = 1.9;
                    break;
            }

            switch (kxp)
            {
                case 1:
                    kg = 1000;
                    break;
                case 2:
                    kg = 500;
                    break;
            }

            if (sexo == 1)   //hombre
            {
                tmb = ((10 * peso) + (6.25 * altura) - (5 * edad) + (5));
                tmb = (tmb * multip) - kg;
            }
            else if (sexo == 2)  //mujer
            {
                tmb = ((10 * peso) + (6.25 * altura) - (5 * edad) - (161));
                tmb = (tmb * multip) - kg;
            }
            return tmb;
        }

        [WebMethod]
        public double calculoIdeal(int altura, int sexo)
        {
            double pesoideal = 0;
            if (sexo == 1)
            {
                pesoideal = (altura - 100) * (0.90);
            }
            else if (sexo == 2)
            {
                pesoideal = (altura - 100) * (0.85);
            }
            return pesoideal;
        }

        [WebMethod]
        public string calculoCompCorporal(double imc)
        {
            string compCorp = "";
            if (imc < 18.5)
            {
                compCorp = "Peso inferior al normal";
            }
            else if (imc >= 18.5 && imc <= 24.9)
            {
                compCorp = "Peso normal";
            }
            else if (imc >= 25.0 && imc <= 29.9)
            {
                compCorp = "Peso superior al normal";
            }
            else if (imc > 30.0)
            {
                compCorp = "Obesidad";
            }
            return compCorp;
        }

        [WebMethod]

        public int calculoDieta_plan(double tmb_dieta)
        {
            int plan = 0;
            if (tmb_dieta < 1200)
            {
                plan = 1;
            }
            else if (tmb_dieta > 1200 && tmb_dieta <= 1500)
            {
                plan = 2;
            }
            else if (tmb_dieta > 1500 && tmb_dieta <= 1800)
            {
                plan = 3;
            }
            else if (tmb_dieta > 1800)
            {
                plan = 4;
            }
            return plan;
        }
    }
}
