using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CowShop.Models;

namespace CowShop.Data
{
    public class MembresiaData
    {
        public static bool RegistrarMembresia(Membresia oMembresia)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE RegistrarMembresia '" +
                               oMembresia.Nombre_Membresia + "','" +
                               oMembresia.Valor_Membresia + "'";

            bool success = objEst.EjecutarSentencia(sentencia, false);
            objEst = null;
            return success;
        }

        public static bool ActualizarMembresia(string id, Membresia oMembresia)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE ActualizarMembresia '" + id + "','" +
                               oMembresia.Nombre_Membresia + "','" +
                               oMembresia.Valor_Membresia + "'";

            bool success = objEst.EjecutarSentencia(sentencia, false);
            objEst = null;
            return success;
        }

        public static List<Membresia> ListarMembresia()
        {
            List<Membresia> lista = new List<Membresia>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE SP_ListarMembresias";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Membresia()
                    {
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        Nombre_Membresia = dr["Nombre_Membresia"].ToString(),
                        Valor_Membresia = Convert.ToInt32(dr["Valor_Membresia"]),
                        // Campos de auditoría - con verificación de nulos
                        FechaCreacion = dr["FechaCreacion"] != DBNull.Value ?
                                       Convert.ToDateTime(dr["FechaCreacion"]) : DateTime.Now,
                        FechaModificacion = dr["FechaModificacion"] != DBNull.Value ?
                                          Convert.ToDateTime(dr["FechaModificacion"]) : DateTime.Now,
                        Estado = dr["Estado"] != DBNull.Value ?
                               Convert.ToBoolean(dr["Estado"]) : true
                    });
                }
                // Cerrar el DataReader después de usarlo
                objEst.Reader.Close();
            }
            objEst = null;
            return lista;
        }

        public static List<Membresia> ObtenerMembresia(string id)
        {
            List<Membresia> lista = new List<Membresia>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = $"EXECUTE ObtenerMembresia '{id}'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Membresia()
                    {
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        Nombre_Membresia = dr["Nombre_Membresia"].ToString(),
                        Valor_Membresia = Convert.ToInt32(dr["Valor_Membresia"]),
                        // Campos de auditoría - con verificación de nulos
                        FechaCreacion = dr["FechaCreacion"] != DBNull.Value ?
                                       Convert.ToDateTime(dr["FechaCreacion"]) : DateTime.Now,
                        FechaModificacion = dr["FechaModificacion"] != DBNull.Value ?
                                          Convert.ToDateTime(dr["FechaModificacion"]) : DateTime.Now,
                        Estado = dr["Estado"] != DBNull.Value ?
                               Convert.ToBoolean(dr["Estado"]) : true
                    });
                }
                // Cerrar el DataReader después de usarlo
                objEst.Reader.Close();
            }
            objEst = null;
            return lista;
        }

        public static bool EliminarMembresia(string id)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE EliminarMembresia '" + id + "'";
            bool success = objEst.EjecutarSentencia(sentencia, false);
            objEst = null;
            return success;
        }

        // Método adicional para listar solo membresías activas
        public static List<Membresia> ListarMembresiasActivas()
        {
            List<Membresia> lista = new List<Membresia>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "SELECT * FROM Membresia WHERE Estado = 1 ORDER BY Nombre_Membresia";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    lista.Add(new Membresia()
                    {
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        Nombre_Membresia = dr["Nombre_Membresia"].ToString(),
                        Valor_Membresia = Convert.ToInt32(dr["Valor_Membresia"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    });
                }
                // Cerrar el DataReader después de usarlo
                objEst.Reader.Close();
            }
            objEst = null;
            return lista;
        }

        // Método para obtener membresía por ID (versión mejorada)
        public static Membresia ObtenerMembresiaPorId(int id)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = $"SELECT * FROM Membresia WHERE ID_Membresia = {id}";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    Membresia membresia = new Membresia()
                    {
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        Nombre_Membresia = dr["Nombre_Membresia"].ToString(),
                        Valor_Membresia = Convert.ToInt32(dr["Valor_Membresia"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                        FechaModificacion = Convert.ToDateTime(dr["FechaModificacion"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                    objEst.Reader.Close();
                    objEst = null;
                    return membresia;
                }
                objEst.Reader.Close();
            }
            objEst = null;
            return null;
        }
    }
}