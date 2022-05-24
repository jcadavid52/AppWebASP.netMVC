using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebAppAjaxSpiritualArt.Logica_Negocio;
using WebAppAjaxSpiritualArt.Models;
using WebAppAjaxSpiritualArt.Models.PagosPlan;

namespace WebAppAjaxSpiritualArt.Controllers
{
    public class PrincipalController : Controller
    {
        LogicaNegocioArtista logicaNegocioArtista = new LogicaNegocioArtista();
        LogicaPlanes logicaPlanes = new LogicaPlanes();
        LogicaCategoria logicaCategoria = new LogicaCategoria();
        LogicaProductos logicaProductos = new LogicaProductos();
        // GET: Principal
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult IniciarSesion()
        {

            return View();
        }

        public ActionResult IniciarSesionAccion(REGISTRO_ARTISTA datosAcceso)
        {
            REGISTRO_ARTISTA acceso = logicaNegocioArtista.Acceso(datosAcceso);
            //listar categorías
            List<CATEGORIA> categorias = logicaCategoria.listarCategoria();

            if (acceso == null)
            {
                ViewBag.Mensaje = "Clave o usuario inválido, intente nuevamente";
                return View("IniciarSesion");
            }
            else if (acceso.ESTADO != true)
            {
                ViewBag.Mensaje = "Tu usuario actualmente está desactivado, comunicate con nosotros";
                return View("IniciarSesion");
            }
            else
            {
                Session["correo"] = acceso.CORREO;
                Session["clave"] = acceso.CLAVE;
                Session["imagen"] = acceso.IMAGEN;
                Session["ciudad"] = acceso.CIUDAD;
                Session["id_registro"] = acceso.PK_ID_ARTISTA;
                Session["pais"] = acceso.PAIS;
                Session["localidad"] = acceso.LOCALIDAD;
                Session["direccion"] = acceso.DIRECCION;
                Session["fk_plan"] = acceso.FK_TIPO_PLAN;
                Session["nombre_artista"] = acceso.NOMBRE_ARTISTA;
                Session["primer_apellido"] = acceso.PRIMER_APELLIDO_ARTISTA;
                Session["segundo_apellido"] = acceso.SEGUNDO_APELLIDO_ARTISTA;
                Session["telefono"] = acceso.TELEFONO;

                //consultar obras del artista
                int fk_artista = Convert.ToInt32(Session["id_registro"].ToString());
                ViewBag.ConsultaObras = logicaProductos.consultarObra(fk_artista);


                return View("IndexSesion", categorias);
            }
        }

        public ActionResult IndexSesion()
        {
            //listar categorías
            List<CATEGORIA> categorias = logicaCategoria.listarCategoria();
            //consultar obras del artista
            int fk_artista = Convert.ToInt32(Session["id_registro"].ToString());
            ViewBag.ConsultaObras = logicaProductos.consultarObra(fk_artista);
            if (Session["correo"] != null && Session["clave"] != null)
            {
                return View(categorias);
            }
            else
            {
                return View("Index");
            }



        }

        //solo vista de registro
        public ActionResult RegistrarArtista()
        {
            List<TIPO_PLAN> listarPlanes = logicaPlanes.listarPlanes();
            return View(listarPlanes);
        }

        //registro de artista
        public ActionResult RegistrarArtistaAccion(REGISTRO_ARTISTA nuevoArtista)
        {

            TempData["correo"] = nuevoArtista.CORREO;
            TempData["nombre_artista"] = nuevoArtista.NOMBRE_ARTISTA;
            TempData["primer_apellido"] = nuevoArtista.PRIMER_APELLIDO_ARTISTA;
            TempData["segundo_apeliido"] = nuevoArtista.SEGUNDO_APELLIDO_ARTISTA;
            TempData["telefono"] = nuevoArtista.TELEFONO;
            TempData["pais"] = nuevoArtista.PAIS;
            TempData["ciudad"] = nuevoArtista.CIUDAD;
            TempData["localidad"] = nuevoArtista.LOCALIDAD;
            TempData["direccion"] = nuevoArtista.DIRECCION;
            TempData["fk_plan"] = nuevoArtista.FK_TIPO_PLAN;
            TempData["imagen"] = nuevoArtista.IMAGEN;
            int? fk_plan = nuevoArtista.FK_TIPO_PLAN;



            //verifica el tipo de plan
            if (nuevoArtista.FK_TIPO_PLAN == 8 || nuevoArtista.FK_TIPO_PLAN == 9)
            {
                TempData["correoRegistrado"] = nuevoArtista.CORREO;
                List<TIPO_PLAN> listarPlanes = logicaPlanes.listarPlanes(fk_plan);
                return View("OfertaPlanes", listarPlanes);
            }
            else
            {

                logicaNegocioArtista.RegistrarArtista(nuevoArtista);


                //consulta con varios parametros para traer la clave del artista
                REGISTRO_ARTISTA obtenerClave = logicaNegocioArtista.obtenerClaveArtista(nuevoArtista.CORREO, nuevoArtista.NOMBRE_ARTISTA, nuevoArtista.PRIMER_APELLIDO_ARTISTA, nuevoArtista.SEGUNDO_APELLIDO_ARTISTA);


                //envio de los parametros que va a llevar el envio del email
                logicaNegocioArtista.EnviarEmail(obtenerClave.CORREO, obtenerClave.CLAVE, obtenerClave.CORREO, obtenerClave.NOMBRE_ARTISTA);
                Session["id_registro"] = obtenerClave.PK_ID_ARTISTA;
                TempData["correoRegistrado"] = nuevoArtista.CORREO;
                return View("IniciarSesionPago", obtenerClave);
            }
        }

        //solo vista de los planes
        public ActionResult OfertaPlanes()
        {
            return View();
        }

        //vista que se muestra si el usuario relaizo el pago o se registró gratuitamente
        public ActionResult IniciarSesionPago()
        {
            string correo = TempData["correo"].ToString();
            string nombre_artista = TempData["nombre_artista"].ToString();
            string primer_apellido = TempData["primer_apellido"].ToString();
            string segundo_apellido = TempData["segundo_apeliido"].ToString();
            string telefono = TempData["telefono"].ToString();
            string pais = TempData["pais"].ToString();
            string ciudad = TempData["ciudad"].ToString();
            string localidad = TempData["localidad"].ToString();
            string direccion = TempData["direccion"].ToString();
            int? fk_plan = Convert.ToInt32(TempData["fk_plan"].ToString());
            string imagen = TempData["imagen"].ToString();

            var registrarArtista = new REGISTRO_ARTISTA()
            {
                NOMBRE_ARTISTA = nombre_artista,
                PRIMER_APELLIDO_ARTISTA = primer_apellido,
                SEGUNDO_APELLIDO_ARTISTA = segundo_apellido,
                TELEFONO = telefono,
                PAIS = pais,
                CIUDAD = ciudad,
                LOCALIDAD = localidad,
                DIRECCION = direccion,
                FK_TIPO_PLAN = fk_plan,
                CORREO = correo,
                IMAGEN = imagen


            };

            logicaNegocioArtista.RegistrarArtista(registrarArtista);

            REGISTRO_ARTISTA obtenerClave = logicaNegocioArtista.obtenerClaveArtista(correo, nombre_artista, primer_apellido, segundo_apellido);
            Session["id_registro"] = obtenerClave.PK_ID_ARTISTA;
            logicaNegocioArtista.EnviarEmail(obtenerClave.CORREO, obtenerClave.CLAVE, obtenerClave.CORREO, obtenerClave.NOMBRE_ARTISTA);
            return View(obtenerClave);
        }

        //método que se usa para hacer el pago del plan desde paypal
        [HttpPost]
        public async Task<JsonResult> PagoPlan(string precio, string plan)
        {
            bool status = false;
            string respuesta = string.Empty;

            using (var client = new HttpClient())
            {
                var userName = "Aedtf_Rt5Y4MwJCsIph_C3re97TvnicO-cyxmTM_VRPsG1y-eUXREfVXpzPrhH1zZZkcNuRnb1QTYbmC";
                var password = "EFfc3lvPYwDO7FJBZuFg2BhZWb3EfUz-KLt_KfKKU_HzT1pHJPnG87BtMtlpopXbwJlSK-GGA5RaEqb7";

                client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{userName}:{password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var orden = new OrdenPlanes()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<PurchaseUnit>()
                      {
                          new PurchaseUnit()
                          {
                              amount = new Amount()
                              {
                                  currency_code = "USD",
                                  value = precio
                              },
                              description = plan
                          }
                      },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "SpiritualArt.com",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://localhost:44329/Principal/IniciarSesionPago",
                        cancel_url = "https://localhost:44329/Principal/Index"

                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);
                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }

            }

            return Json(new { status = status, respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }

        //exposiciones de todas las obras de los artistas en general
        public ActionResult Exposicion()
        {
            List<PRODUCTO> listarProductoArtistas = logicaProductos.listarObras();
            ViewBag.Categorias = logicaCategoria.listarCategoria();
            return View(listarProductoArtistas);
        }

        //filtrar obras por categoria
        public ActionResult FiltrarObras(int id_categoria)
        {
            List<PRODUCTO> listarProductoArtistas = logicaProductos.listarObras(id_categoria);
            ViewBag.Categorias = logicaCategoria.listarCategoria();

            return View("Exposicion", listarProductoArtistas);
        }

        //listar artistas
        public ActionResult ListarArtistas()
        {
            List<REGISTRO_ARTISTA> listarArtista = logicaNegocioArtista.listarArtistas();
            return View(listarArtista);
        }

        public ActionResult CerrarSesion()
        {


            Session["correo"] = null;
            Session["clave"] = null;

            return Content("1");
        }


        // cambia la imagen de perfil del artista
        public ActionResult CambiarImagen(REGISTRO_ARTISTA nuevaImagen)
        {
            try
            {

                //listar categorías
                List<CATEGORIA> categorias = logicaCategoria.listarCategoria();

                //consultar obras del artista
                int fk_artista = Convert.ToInt32(Session["id_registro"].ToString());
                ViewBag.ConsultaObras = logicaProductos.consultarObra(fk_artista);

                string nombreArchivo = Path.GetFileNameWithoutExtension(nuevaImagen.archivo.FileName);
                string extension = Path.GetExtension(nuevaImagen.archivo.FileName);
                nombreArchivo = nombreArchivo + DateTime.Now.ToString("yymmssfff") + extension;
                nuevaImagen.IMAGEN = "~/ArchivosLectura/" + nombreArchivo;
                nombreArchivo = Path.Combine(Server.MapPath("~/ArchivosLectura/"), nombreArchivo);
                nuevaImagen.archivo.SaveAs(nombreArchivo);
                nuevaImagen.ESTADO = true;
                logicaNegocioArtista.modificarImagen(nuevaImagen);

                ModelState.Clear();
                Session["imagen"] = nuevaImagen.IMAGEN;



                return Content("1");
            }
            catch (Exception e)
            {
                return Content("2");
            }




        }


        //detalle de obra para hacer solicitud de compra
        public ActionResult DetalleObraCompra(int id)
        {
            // consulta un producto en especifico incluido el artista con la fk
            var productoEspecifico = logicaProductos.detalleObraCompra(id);

            //lo manda a la vista
            ViewBag.DetalleObra = productoEspecifico;

            //obtiene la fk
            int? fk_artista = productoEspecifico.FK_ARTISTA;

            //conslta las obras en especifico del artista a detallar con la fk
            var obrasArtistaEspecifico = logicaProductos.listaDetalleObraCompra(fk_artista);


            return View(obrasArtistaEspecifico);
        }

    }
}