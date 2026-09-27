using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using CowShop.Models;

namespace CowShop.Data
{
    public class VacaData
    {
        public static bool RegistrarVaca(Vaca oVaca)
        {
            // Verificar si el usuario es Vendedor o Ambos
            string tipoUsuario = UsuarioData.ObtenerTipoUsuario(oVaca.ID_Vendedor);

            if (tipoUsuario == null || (tipoUsuario != "Vendedor" && tipoUsuario != "Ambos"))
            {
                return false; // No tiene permiso para registrar vacas
            }
            ConexionBD objEst = new ConexionBD();
            string peso = oVaca.Peso.ToString(CultureInfo.InvariantCulture);
            string precio = oVaca.Precio.ToString(CultureInfo.InvariantCulture);
            string sentencia;
            sentencia = "EXECUTE RegistrarVaca '" + oVaca.Nombre + "','" + oVaca.Raza + "','" + oVaca.Edad + "','"
            + peso + "','" + precio + "','" + oVaca.Estado_Salud + "','" + oVaca.ID_Vendedor + "'";

            bool result = false;

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;

                if (dr.Read())
                {
                    oVaca.ID_Vaca = Convert.ToInt32(dr["ID_Vaca"]);
                    result = true;
                }


                dr.Close();
                dr.Dispose();
            }

            objEst = null;
            return result;
        }

        public static bool ActualizarVaca(int id, Vaca oVaca)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia;
            sentencia = "EXECUTE ActualizarVaca '" + id + "','" + oVaca.Nombre + "','" +
           oVaca.Raza + "','" + oVaca.Edad + "','" + oVaca.Peso + "','" + oVaca.Precio + "','" +
           oVaca.Estado_Salud + "','" + oVaca.ID_Vendedor + "'";
            if (!objEst.EjecutarSentencia(sentencia, false))
            {
                objEst = null;
                return false;
            }
            else
            {
                objEst = null;
                return true;
            }
        }

        public static List<Vaca> ListarVacas()
        {
            List<Vaca> oListaVaca = new List<Vaca>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE SP_LISTAR_Vaca";
            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    oListaVaca.Add(new Vaca()
                    {
                        ID_Vaca = dr.GetInt32(dr.GetOrdinal("ID_Vaca")),
                        Nombre = dr["Nombre"].ToString(),
                        Raza = dr["Raza"].ToString(),
                        Edad = Convert.ToInt32(dr["Edad"]),
                        Peso = Convert.ToDecimal(dr["Peso"]),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado_Salud = dr["Estado_Salud"].ToString(),
                        ID_Vendedor = Convert.ToInt32(dr["ID_Vendedor"])
                    });
                }
                return oListaVaca;
            }
            else
            {
                return oListaVaca;
            }
        }


        public static Vaca ObtenerVaca(int id)
        {
            Vaca vaca = null;
            ConexionBD objEst = new ConexionBD();
            // Llamamos al procedimiento almacenado pasando el parámetro @ID_Vaca
            string sentencia = $"EXECUTE ObtenerVaca @ID_Vaca = {id}";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    vaca = new Vaca()
                    {
                        ID_Vaca = dr.GetInt32(dr.GetOrdinal("ID_Vaca")),
                        Nombre = dr["Nombre"].ToString(),
                        Raza = dr["Raza"].ToString(),
                        Edad = Convert.ToInt32(dr["Edad"]),
                        Peso = Convert.ToDecimal(dr["Peso"]),
                        Precio = Convert.ToDecimal(dr["Precio"]),
                        Estado_Salud = dr["Estado_Salud"].ToString(),
                        ID_Vendedor = Convert.ToInt32(dr["ID_Vendedor"])
                    };
                }
            }
            return vaca;
        }

        public static bool EliminarVaca(string id)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE EliminarVaca '" + id + "'";
            bool success = objEst.EjecutarSentencia(sentencia, false);

            objEst = null;
            return success;
        }

    }
    }