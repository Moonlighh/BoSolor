using BODEGA_SOLORZANO.Models.BoSolor;
using BODEGA_SOLORZANO.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BODEGA_SOLORZANO.Controllers
{
    [Authorize]
    public class ProveedorProductoController : Controller
    {
        public IActionResult Index()
        {
            var listaProveedorProductos = logProveedorProducto.Instancia.ListarProveedorProducto();
            return View(listaProveedorProductos);
        }

        [HttpPost]
        public IActionResult Guardar(ProveedorProductoModel proveedorProducto, int producto, int proveedor)
        {
            if (ModelState.IsValid)
            {
                ProductoModel pro = new();
                ProveedorModel prov = new();

                pro.IdProducto = producto;
                prov.IdProveedor = proveedor;

                proveedorProducto.IdProductoNavegacion = pro;
                proveedorProducto.IdProveedorNavegacion = prov;


                var respuesta = logProveedorProducto.Instancia.CrearProveedorProductos(proveedorProducto);

                if (respuesta)
                {
                    return RedirectToAction("Listar");
                }
            }
            return View();
        }

    }
}
