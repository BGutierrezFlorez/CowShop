using CowShop.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CowShop.Data
{
    public class RolData
    {
        private ConexionBD conexion = new ConexionBD();

        // Crear un nuevo rol
        public bool CrearRol(Rol rol)
        {
            using (SqlCommand cmd = new SqlCommand("sp_CrearRol"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreRol", rol.NombreRol);
                cmd.Parameters.AddWithValue("@DescripcionRol", rol.DescripcionRol);
                cmd.Parameters.AddWithValue("@FechaCreacion", rol.FechaCreacion);
                cmd.Parameters.AddWithValue("@Estado", rol.Estado);

                return conexion.EjecutarComando(cmd);
            }
        }

        // Obtener todos los roles
        public List<Rol> ObtenerRol()
        {
            List<Rol> roles = new List<Rol>();
            using (SqlCommand cmd = new SqlCommand("sp_ObtenerRoles"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (conexion.Consultar(cmd, false))
                {
                    SqlDataReader reader = conexion.Reader;
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            ID_Rol = Convert.ToInt32(reader["ID_Rol"]),
                            NombreRol = reader["NombreRol"].ToString(),
                            DescripcionRol = reader["DescripcionRol"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                        });
                    }
                    reader.Close();
                    conexion.CerrarConexion();
                }
            }
            return roles;
        }

        // Obtener un rol por ID
        public static List<Rol> ObtenerRolPorId(int id)
        {
            List<Rol> listaRoles = new List<Rol>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = $"EXECUTE sp_ObtenerRolPorId '{id}'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    listaRoles.Add(new Rol()
                    {
                        ID_Rol = Convert.ToInt32(dr["ID_Rol"]),
                        NombreRol = dr["NombreRol"].ToString(),
                        DescripcionRol = dr["DescripcionRol"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"]),
                        FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"])
                    });
                }
            }

            objEst = null;
            return listaRoles;
        }


        // Actualizar un rol
        public bool ActualizarRol(Rol rol)
        {
            using (SqlCommand cmd = new SqlCommand("sp_ActualizarRol"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Rol", rol.ID_Rol);
                cmd.Parameters.AddWithValue("@NombreRol", rol.NombreRol);
                cmd.Parameters.AddWithValue("@DescripcionRol", rol.DescripcionRol);
                cmd.Parameters.AddWithValue("@Estado", rol.Estado);
                cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);

                return conexion.EjecutarComando(cmd);
            }
        }

        // Eliminar un rol
        public bool EliminarRol(int id)
        {
            using (SqlCommand cmd = new SqlCommand("sp_EliminarRol"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_Rol", id);
                return conexion.EjecutarComando(cmd);
            }
        }
    }
}
