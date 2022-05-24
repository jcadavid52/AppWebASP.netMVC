using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppAjaxSpiritualArt.Logica_Negocio;
using WebAppAjaxSpiritualArt.Models;

namespace WebAppAjaxSpiritualArt.Controllers
{
    public class JsonProductosController : Controller
    {
        LogicaNegocioArtista logicaNegocioArtista = new LogicaNegocioArtista();
        LogicaPlanes logicaPlanes = new LogicaPlanes();
        LogicaCategoria logicaCategoria = new LogicaCategoria();
        LogicaProductos logicaProductos = new LogicaProductos();
        LogicaCliente logicaCliente = new LogicaCliente();
        //lista las obras premium

        [HttpGet]
        public JsonResult ObrasPremium()
        {
            List<PRODUCTO> productosPremium = logicaProductos.listarObrasPremium();

            List<strObrasPremium> structs = new List<strObrasPremium>();

            foreach (var item in productosPremium)
            {
                strObrasPremium str = new strObrasPremium();

                str.PK_ID_PRODUCTO = item.PK_ID_PRODUCTO;
                str.NOMBRE_PRODUCTO = item.NOMBRE_PRODUCTO;
                str.PRECIO = item.PRECIO;
                str.DESCRIPCION = item.DESCRIPCION;
                str.CANTIDAD = item.CANTIDAD;
                str.ESTADO = item.ESTADO;
                str.IMAGEN_PRODUCTO = item.IMAGEN_PRODUCTO;
                str.FK_CATEGORIA = item.FK_CATEGORIA;
                str.FK_ARTISTA = item.FK_ARTISTA;
                str.nombreArtista = item.REGISTRO_ARTISTA.NOMBRE_ARTISTA;
                structs.Add(str);
            }

            return Json(structs, JsonRequestBehavior.AllowGet);
        }

        struct strObrasPremium
        {
            public int PK_ID_PRODUCTO { get; set; }
            public string NOMBRE_PRODUCTO { get; set; }
            public double? PRECIO { get; set; }
            public string DESCRIPCION { get; set; }
            public int? CANTIDAD { get; set; }
            public bool? ESTADO { get; set; }
            public string IMAGEN_PRODUCTO { get; set; }
            public int? FK_CATEGORIA { get; set; }
            public int? FK_ARTISTA { get; set; }

            public string nombreArtista { get; set; }
        }

        public JsonResult ConsultarObra()
        {
            //consultar obras del artista
            int fk_artista = Convert.ToInt32(Session["id_registro"].ToString());
            List<PRODUCTO> consultaObra = logicaProductos.consultarObra(fk_artista);

            List<strConsultarObra> structs = new List<strConsultarObra>();

            foreach(var item in consultaObra)
            {
                strConsultarObra str = new strConsultarObra();

                str.PK_ID_PRODUCTO = item.PK_ID_PRODUCTO;
                str.NOMBRE_PRODUCTO = item.NOMBRE_PRODUCTO;
                str.PRECIO = item.PRECIO;
                str.DESCRIPCION = item.DESCRIPCION;
                str.CANTIDAD = item.CANTIDAD;
                str.ESTADO = item.ESTADO;
                str.IMAGEN_PRODUCTO = item.IMAGEN_PRODUCTO;
                str.FK_CATEGORIA = item.FK_CATEGORIA;
                str.FK_ARTISTA = item.FK_ARTISTA;
                structs.Add(str);
            }

            return Json(structs, JsonRequestBehavior.AllowGet);

        }

        struct strConsultarObra
        {
            public int PK_ID_PRODUCTO { get; set; }
            public string NOMBRE_PRODUCTO { get; set; }
            public double? PRECIO { get; set; }
            public string DESCRIPCION { get; set; }
            public int? CANTIDAD { get; set; }
            public bool? ESTADO { get; set; }
            public string IMAGEN_PRODUCTO { get; set; }
            public int? FK_CATEGORIA { get; set; }
            public int? FK_ARTISTA { get; set; }

        }

        public JsonResult ListarCategoria()
        {
            List<CATEGORIA> categorias = logicaCategoria.listarCategoria();
            List<strListarCategorias> structs = new List<strListarCategorias>();

            foreach (var item in categorias)
            {
                strListarCategorias str = new strListarCategorias();

                str.PK_ID_CATEGORIA = item.PK_ID_CATEGORIA;
                str.NOMBRE_CATEGORIA = item.NOMBRE_CATEGORIA;
                structs.Add(str);
            }

            return Json(structs, JsonRequestBehavior.AllowGet);
        }
        struct strListarCategorias
        {
            public int PK_ID_CATEGORIA { get; set; }
            public string NOMBRE_CATEGORIA { get; set; }
        }

        public ActionResult PublicarObraAccion(PRODUCTO nuevoProducto)
        {
            //listar categorías
            List<CATEGORIA> categorias = logicaCategoria.listarCategoria();

            //guardar imagen y obtener ruta
            string nombreArchivo = Path.GetFileNameWithoutExtension(nuevoProducto.archivoProducto.FileName);
            string extension = Path.GetExtension(nuevoProducto.archivoProducto.FileName);

            nombreArchivo = nombreArchivo + DateTime.Now.ToString("yymmssfff") + extension;
            nuevoProducto.IMAGEN_PRODUCTO = "../ArchivosLectura/" + nombreArchivo;
            nombreArchivo = Path.Combine(Server.MapPath("~/ArchivosLectura/"), nombreArchivo);

            nuevoProducto.archivoProducto.SaveAs(nombreArchivo);


            ModelState.Clear();

            //publicar obra

            var agreagrObra = logicaProductos.AgregarObra(nuevoProducto);

            if(agreagrObra == true)
            {
                return Content("1");
            }
            else
            {
                return Content("2");
            }

           


            
        }

        public ActionResult RegistroCliente(CLIENTE cliente)
        {
            var nuevoCliente = logicaCliente.registroCliente(cliente);

            if (nuevoCliente == false)
            {
                return Content("2");
            }
            return Content("1");
        }

        [HttpGet]
        public JsonResult EnviarNotificaciones(int id)
        {
            List<NOTIFICACION> notificaciones = logicaNegocioArtista.notificaciones(id);
            List<strListarNotificaciones> structs = new List<strListarNotificaciones>();

            foreach (var item in notificaciones)
            {
                strListarNotificaciones str = new strListarNotificaciones();

                str.PK_ID_NOTIFICACION = item.PK_ID_NOTIFICACION;
                DateTime fecha = Convert.ToDateTime( item.FECHA);
                
                str.fecha = Convert.ToString( fecha.ToShortDateString());
                str.nombreCliente = item.CLIENTE.NOMBRE_CLIENTE;
                str.apellidoCliente = item.CLIENTE.APELLIDO_CLIENTE;
                str.correoCliente = item.CLIENTE.EMAIL_CLIENTE;
                str.telefonoCliente = item.CLIENTE.TELEFONO_CLIENTE;
                str.asunto = item.CLIENTE.ASUNTO;
                str.mensaje = item.CLIENTE.MENSAJE;
                str.nombreProducto = item.PRODUCTO.NOMBRE_PRODUCTO;
                structs.Add(str);
            }

            

   

            return Json(structs, JsonRequestBehavior.AllowGet);

        }

        struct strListarNotificaciones
        {
            public int PK_ID_NOTIFICACION { get; set; }
            public string fecha { get; set; }
            public string nombreCliente { get; set; }
            public string apellidoCliente { get; set; }
            public string correoCliente { get; set; }
            public string telefonoCliente { get; set; }
            public string asunto { get; set; }
            public string mensaje { get; set; }
            public string nombreProducto { get; set; }

            

        }







    }


}