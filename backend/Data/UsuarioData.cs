using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CowShop.Models;

namespace CowShop.Data
{
    public class UsuarioData
    {
        public static bool RegistrarUsuario(Usuario oUsuario)
        {
            ConexionBD objEst = new ConexionBD();

            try
            {
                if (string.IsNullOrWhiteSpace(oUsuario.Contrasena))
                    return false;

                oUsuario.Correo = oUsuario.Correo?.Trim();
                oUsuario.Contrasena = oUsuario.Contrasena.Trim();

                if (!EsHashBCrypt(oUsuario.Contrasena))
                    oUsuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(oUsuario.Contrasena);

                using (SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario"))
                {
                    cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.NVarChar, 80) { Value = oUsuario.Nombre });
                    cmd.Parameters.Add(new SqlParameter("@Cedula", SqlDbType.NVarChar, 20) { Value = oUsuario.Cedula });
                    cmd.Parameters.Add(new SqlParameter("@Fecha_Nacimiento", SqlDbType.DateTime) { Value = oUsuario.Fecha_Nacimiento });
                    cmd.Parameters.Add(new SqlParameter("@Correo", SqlDbType.NVarChar, 320) { Value = oUsuario.Correo });
                    cmd.Parameters.Add(new SqlParameter("@Celular", SqlDbType.NVarChar, 10) { Value = oUsuario.Celular });
                    cmd.Parameters.Add(new SqlParameter("@Tipo_Usuario", SqlDbType.NVarChar, 20) { Value = oUsuario.Tipo_Usuario });
                    cmd.Parameters.Add(new SqlParameter("@Contrasena", SqlDbType.NVarChar, 255) { Value = oUsuario.Contrasena });
                    cmd.Parameters.Add(new SqlParameter("@ID_Membresia", SqlDbType.Int) { Value = oUsuario.ID_Membresia });
                    cmd.Parameters.Add(new SqlParameter("@ID_Rol", SqlDbType.Int) { Value = oUsuario.ID_Rol });

                    SqlParameter idUsuarioParam = new SqlParameter("@ID_Usuario", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(idUsuarioParam);

                    bool success = objEst.EjecutarComando(cmd);

                    if (!success)
                        System.Diagnostics.Debug.WriteLine("Error SQL RegistrarUsuario: " + objEst.Error);

                    if (success && idUsuarioParam.Value != DBNull.Value)
                        oUsuario.ID_Usuario = Convert.ToInt32(idUsuarioParam.Value);

                    return success;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error RegistrarUsuario: " + ex.Message);
                return false;
            }
        }

        private static bool EsHashBCrypt(string valor)
        {
            return !string.IsNullOrWhiteSpace(valor) &&
                   (valor.StartsWith("$2a$") ||
                    valor.StartsWith("$2b$") ||
                    valor.StartsWith("$2y$"));
        }

        // Los demás métodos se mantienen igual...
        public static bool ActualizarUsuario(int id, Usuario oUsuario)
        {
            ConexionBD objEst = new ConexionBD();

            string contrasenaSql = string.IsNullOrWhiteSpace(oUsuario.Contrasena)
                ? "NULL"
                : $"'{oUsuario.Contrasena}'";

            bool valor = oUsuario.Estado;
            int estadoUsuario = valor ? 1 : 0;

            string sentencia = "EXECUTE ActualizarUsuario '" + id + "','" +
    oUsuario.Nombre + "','" +
    oUsuario.Cedula + "','" +
    oUsuario.Fecha_Nacimiento.ToString("yyyy-MM-dd") + "','" +
    oUsuario.Correo + "','" +
    oUsuario.Celular + "','" +
    oUsuario.Tipo_Usuario + "','" +
    oUsuario.ID_Membresia + "'," +
    contrasenaSql + "," +
    oUsuario.ID_Rol + "," +
    estadoUsuario;


            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    int filas = Convert.ToInt32(dr["FilasActualizadas"]);
                    objEst = null;
                    return filas > 0;
                }
            }

            objEst = null;
            return false;
        }

        public static List<Usuario> ListarUsuarios()
        {
            List<Usuario> oListaUsuario = new List<Usuario>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE sp_ListarUsuariosActivos";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    oListaUsuario.Add(new Usuario()
                    {
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Cedula = dr["Cedula"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(dr["Fecha_Nacimiento"].ToString()),
                        Correo = dr["Correo"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        Tipo_Usuario = dr["Tipo_Usuario"].ToString(),
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        ID_Rol = Convert.ToInt32(dr["ID_Rol"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    });
                }
            }

            objEst = null;
            return oListaUsuario;
        }

        public static List<Usuario> ObtenerUsuarioporId(string id)
        {
            List<Usuario> oListaUsuario = new List<Usuario>();
            ConexionBD objEst = new ConexionBD();
            string sentencia = $"EXECUTE ObtenerUsuario '{id}'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                while (dr.Read())
                {
                    oListaUsuario.Add(new Usuario()
                    {
                        ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                        Nombre = dr["Nombre"].ToString(),
                        Cedula = dr["Cedula"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(dr["Fecha_Nacimiento"].ToString()),
                        Correo = dr["Correo"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        Tipo_Usuario = dr["Tipo_Usuario"].ToString(),
                        ID_Membresia = Convert.ToInt32(dr["ID_Membresia"]),
                        ID_Rol = Convert.ToInt32(dr["ID_Rol"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    });
                }
            }

            objEst = null;
            return oListaUsuario;
        }

        public static bool EliminarUsuario(string id)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = "EXECUTE EliminarUsuario '" + id + "'";
            if (objEst.Consultar(sentencia, false))
            {
                var dr = objEst.Reader;
                if (dr.Read())
                {
                    int resultado = Convert.ToInt32(dr["Resultado"]);
                    objEst = null;
                    return resultado == 1;
                }
            }
            objEst = null;
            return false;
        }

        public static Usuario Login(string correo, string contrasena)
        {
            ConexionBD objEst = new ConexionBD();

            try
            {
                correo = correo?.Trim();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT ID_Usuario, Nombre, Tipo_Usuario, Contrasena
                    FROM Usuarios
                    WHERE Correo = @Correo AND Estado = 1"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@Correo", SqlDbType.NVarChar, 320) { Value = correo });

                    if (objEst.Consultar(cmd, false))
                    {
                        SqlDataReader dr = objEst.Reader;
                        if (dr.Read())
                        {
                            string hashDb = dr["Contrasena"].ToString();

                            if (string.IsNullOrWhiteSpace(hashDb) ||
                                !BCrypt.Net.BCrypt.Verify(contrasena, hashDb))
                            {
                                return null;
                            }

                            return new Usuario
                            {
                                ID_Usuario = Convert.ToInt32(dr["ID_Usuario"]),
                                Nombre = dr["Nombre"].ToString(),
                                Tipo_Usuario = dr["Tipo_Usuario"].ToString()
                            };
                        }
                    }
                }
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                System.Diagnostics.Debug.WriteLine("Hash BCrypt invalido: " + ex.Message);
                return null;
            }
            finally
            {
                objEst.CerrarConexion();
                objEst = null;
            }

            return null;
        }

        public static string ObtenerTipoUsuario(int idUsuario)
        {
            ConexionBD objEst = new ConexionBD();
            string sentencia = $"EXECUTE ObtenerTipoUsuario '{idUsuario}'";

            if (objEst.Consultar(sentencia, false))
            {
                SqlDataReader dr = objEst.Reader;
                if (dr.Read())
                {
                    string tipo = dr["Tipo_Usuario"].ToString();
                    objEst = null;
                    return tipo;
                }
            }

            objEst = null;
            return null;
        }

        // Método diagnóstico para depuración: verifica si el correo existe y si la contraseña coincide
        public enum VerificacionLoginResultado { NoExisteCorreo, ContrasenaIncorrecta, Ok }

        public static VerificacionLoginResultado VerificarLogin(string correo, string contrasena)
        {
            ConexionBD objEst = new ConexionBD();

            try
            {
                correo = correo?.Trim();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT Contrasena
                    FROM Usuarios
                    WHERE Correo = @Correo AND Estado = 1"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@Correo", SqlDbType.NVarChar, 320) { Value = correo });

                    if (!objEst.Consultar(cmd, false))
                        return VerificacionLoginResultado.NoExisteCorreo;

                    SqlDataReader dr = objEst.Reader;

                    if (!dr.Read())
                        return VerificacionLoginResultado.NoExisteCorreo;

                    string hashDb = dr["Contrasena"].ToString();

                    if (string.IsNullOrWhiteSpace(hashDb))
                        return VerificacionLoginResultado.ContrasenaIncorrecta;

                    bool passwordValida = BCrypt.Net.BCrypt.Verify(contrasena, hashDb);

                    return passwordValida
                        ? VerificacionLoginResultado.Ok
                        : VerificacionLoginResultado.ContrasenaIncorrecta;
                }
            }
            catch (BCrypt.Net.SaltParseException ex)
            {
                System.Diagnostics.Debug.WriteLine("Hash BCrypt invalido: " + ex.Message);
                return VerificacionLoginResultado.ContrasenaIncorrecta;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error VerificarLogin: " + ex.Message);
                return VerificacionLoginResultado.NoExisteCorreo;
            }
            finally
            {
                objEst.CerrarConexion();
                objEst = null;
            }
        }
    }
}
