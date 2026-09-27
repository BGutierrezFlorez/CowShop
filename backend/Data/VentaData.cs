using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CowShop.Models;
using Newtonsoft.Json;

namespace CowShop.Data
{
    public class VentaData
    {
        //public static int RegistrarVentaMultiple(VentaRequest venta)
        //{
        //    ConexionBD objEst = new ConexionBD();
        //    int idVenta = 0;

        //    string ventaJson = JsonConvert.SerializeObject(venta);

        //    SqlCommand cmd = new SqlCommand("RegistrarVentaMultiple");
        //    cmd.Parameters.AddWithValue("@VentaJson", ventaJson);

        //    if (objEst.Consultar(cmd, true))
        //    {
        //        SqlDataReader dr = objEst.Reader;

        //        if (dr.Read())
        //        {
        //            idVenta = Convert.ToInt32(dr["ID_Venta"]);
        //        }

        //        dr.Close();
        //        dr.Dispose();
        //    }

        //    objEst.CerrarConexion();
        //    return idVenta;
        //}

        public static int RegistrarVentaMultiple(VentaRequest venta)
        {
            ConexionBD objEst = new ConexionBD();
            int idVenta = 0;
            string ventaJson = JsonConvert.SerializeObject(venta);

            SqlCommand cmd = new SqlCommand("RegistrarVentaMultiple");
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VentaJson", ventaJson);

            try
            {
                if (objEst.Consultar(cmd, true))
                {
                    using (SqlDataReader dr = objEst.Reader)
                    {
                        if (dr != null && dr.Read())
                        {
                            if (!dr.IsDBNull(dr.GetOrdinal("ID_Venta")))
                                idVenta = dr.GetInt32(dr.GetOrdinal("ID_Venta")); // si el tipo es int en BD
                                                                                  // ó Convert.ToInt32(dr["ID_Venta"]);
                        }
                    }
                }
            }
            finally
            {
                objEst.CerrarConexion();
            }

            return idVenta;
        }


        public static Venta ObtenerVentaPorId(int id)
        {
            ConexionBD objEst = new ConexionBD();
            Venta venta = null;

            SqlCommand cmd = new SqlCommand("ObtenerVentaPorId");
            cmd.Parameters.Add("@ID_Venta", SqlDbType.Int).Value = id;

            if (objEst.Consultar(cmd, true))
            {
                SqlDataReader dr = objEst.Reader;

                if (dr.Read())
                {
                    venta = new Venta
                    {
                        ID_Venta = (int)dr["ID_Venta"],
                        ID_Comprador = (int)dr["ID_Comprador"],
                        Fecha_Venta = (DateTime)dr["Fecha"],
                        Total = (decimal)dr["Total"],
                        Detalles = new List<DetalleVenta>()
                    };
                }

                dr.Close();
            }

            
            if (venta != null)
            {
                cmd = new SqlCommand("ObtenerDetallesVenta");
                cmd.Parameters.Add("@ID_Venta", SqlDbType.Int).Value = id;

                if (objEst.Consultar(cmd, true))
                {
                    SqlDataReader dr = objEst.Reader;
                    while (dr.Read())
                    {
                        venta.Detalles.Add(new DetalleVenta
                        {
                            ID_DetalleVenta = (int)dr["ID_Detalle_Venta"],
                            ID_Venta = (int)dr["ID_Venta"],
                            ID_Vaca = (int)dr["ID_Vaca"]
                        });
                    }
                    dr.Close();
                }
            }

            objEst.CerrarConexion();
            return venta;
        }


        public static List<Venta> ListarVentas()
        {
            ConexionBD objEst = new ConexionBD();
            List<Venta> lista = new List<Venta>();

            SqlCommand cmd = new SqlCommand("ListarVentas");

            if (objEst.Consultar(cmd, true))
            {
                SqlDataReader dr = objEst.Reader;

                while (dr.Read())
                {
                    lista.Add(new Venta
                    {
                        ID_Venta = (int)dr["ID_Venta"],
                        ID_Comprador = (int)dr["ID_Comprador"],
                        Fecha_Venta = (DateTime)dr["Fecha_Venta"],
                        Total = (decimal)dr["Total"],
                        Detalles = new List<DetalleVenta>() 
                    });
                }

                dr.Close();
            }

            objEst.CerrarConexion();
            return lista;
        }

        public static bool EliminarVenta(int id)
        {
            ConexionBD objEst = new ConexionBD();
            SqlCommand cmd = new SqlCommand("EliminarVenta");
            cmd.Parameters.Add("@ID_Venta", SqlDbType.Int).Value = id;

            bool exito = objEst.EjecutarComando(cmd);
            return exito;
        }

    }
}
