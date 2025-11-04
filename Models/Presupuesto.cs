namespace tl2_tp7_2025.Models;

public class Presupuesto
{
    public int IdPresupuesto { get; set; }
    public string nombreDestinatario { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public List<PresupuestoDetalle> detalle { get; set; } = new();

    public double MontoPresupuesto()
    {
        return detalle.Sum(d => d.producto.precio * d.cantidad);
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
