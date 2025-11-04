using Microsoft.AspNetCore.Mvc;
using tl2_tp7_2025.Models;
using tl2_tp7_2025.Repositorios;

namespace tl2_tp7_2025.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private ProductoRepository repo = new();

    // GET /api/Producto
    [HttpGet]
    public ActionResult<List<Producto>> GetProductos()
    {
        var productos = repo.GetAll();
        return Ok(productos);
    }

    // GET /api/Producto/{id}
    [HttpGet("{id}")]
    public ActionResult<Producto> GetProductoById(int id)
    {
        var producto = repo.GetById(id);
        if (producto == null)
            return NotFound($"No se encontró el producto con ID {id}");
        return Ok(producto);
    }

    // POST /api/Producto
    [HttpPost]
    public ActionResult<string> Crear(Producto nuevo)
    {
        repo.Crear(nuevo);
        return Ok("Producto dado de alta exitosamente");
    }

    // PUT /api/Producto/{id}
    [HttpPut("{id}")]
    public ActionResult<string> ActualizarProducto(int id, Producto producto)
    {
        var existente = repo.GetById(id);
        if (existente == null)
            return NotFound($"No se encontró el producto con ID {id}");

        repo.ActualizarProducto(id, producto);
        return Ok("Producto actualizado correctamente");
    }

    // DELETE /api/Producto/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        bool eliminado = repo.Eliminar(id);
        if (eliminado)
            return NoContent(); // HTTP 204
        else
            return NotFound($"No se encontró el producto con ID {id}");
    }
}
