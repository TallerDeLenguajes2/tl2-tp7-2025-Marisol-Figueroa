using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025_TuNombre.Models;
using tl2_tp7_2025_TuNombre.Repositorios;

namespace tl2_tp7_2025_TuNombre.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private ProductosRepository repo = new();

    [HttpGet]
    public ActionResult<List<Producto>> GetProductos()
    {
        return Ok(repo.GetAll());
    }

    [HttpPost]
    public ActionResult<string> AltaProducto(Producto nuevo)
    {
        repo.Alta(nuevo);
        return Ok("Producto dado de alta exitosamente");
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        bool eliminado = repo.Eliminar(id);
        if (eliminado)
            return NoContent();
        else
            return NotFound($"No se encontró el producto con ID {id}");
    }
}
