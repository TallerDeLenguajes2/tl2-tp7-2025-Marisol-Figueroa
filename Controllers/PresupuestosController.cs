using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025.Models;
using tl2_tp7_2025.Repositorios;

namespace tl2_tp7_2025.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PresupuestosController : ControllerBase
{
    private PresupuestoRepository repo = new();

    // GET /api/Presupuesto
    [HttpGet]
    public ActionResult<List<Presupuesto>> GetPresupuestos()
    {
        var presupuestos = repo.GetAll();
        return Ok(presupuestos);
    }

    // GET /api/Presupuesto/{id}
    [HttpGet("{id}")]
    public ActionResult<Presupuesto> GetPresupuestoById(int id)
    {
        var presupuesto = repo.ObtenerDetallesPorId(id);
        if (presupuesto == null)
            return NotFound($"No se encontró el presupuesto con ID {id}");
        return Ok(presupuesto);
    }

    // POST /api/Presupuesto
    [HttpPost]
    public ActionResult<string> CrearPresupuesto(Presupuesto nuevo)
    {
        repo.CrearPresupuesto(nuevo);
        return Ok("Presupuesto creado exitosamente");
    }

    // POST /api/Presupuesto/{id}/ProductoDetalle
    [HttpPost("{id}/ProductoDetalle")]
    public ActionResult<string> AgregarProductoDetalle(int id, [FromBody] PresupuestoDetalle detalle)
    {
        var presupuesto = repo.ObtenerDetallesPorId(id);
        if (presupuesto == null)
            return NotFound($"No se encontró el presupuesto con ID {id}");

        repo.AgregarAPresupuesto(id, detalle.producto.idProducto, detalle.cantidad);
        return Ok("Producto agregado al presupuesto correctamente");
    }

    // DELETE /api/Presupuesto/{id}
    [HttpDelete("{id}")]
    public ActionResult DeletePresupuesto(int id)
    {
        bool eliminado = repo.Eliminar(id);
        if (eliminado)
            return NoContent(); // HTTP 204
        else
            return NotFound($"No se encontró el presupuesto con ID {id}");
    }
}
