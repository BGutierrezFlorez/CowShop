using System;
using System.Collections.Generic;

public class VentaRequest
{
    public int ID_Comprador { get; set; }
    public DateTime Fecha_Venta { get; set; } 

    public decimal Total { get; set; }
    public List<DetalleVenta> VacasDetalle { get; set; }
}
