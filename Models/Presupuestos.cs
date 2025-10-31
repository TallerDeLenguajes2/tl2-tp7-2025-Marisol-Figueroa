namespace tl2_tp7_2025_TuNombre.Models;

public class Presupuestos
{
    public int idPresupuesto { get; set; }
    public string? NombreDestinatario { get; set; }
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestoDetalle> detalle { get; set; } = new();

    public double MontoPresupuesto()
    {
        return detalle.Sum(d => d.producto.Precio * d.cantidad);
    }

    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }

    public int CantidadProductos()
    {
        return detalle.Sum(d => d.cantidad);
    }
}
