using System.Collections.Generic;
using System;

public class Venta
{
    public int ID_Venta { get; set; }
    public int ID_Comprador { get; set; }
    public DateTime Fecha_Venta { get; set; }
    public decimal Total { get; set; }

    public List<DetalleVenta> Detalles { get; set; }
}
