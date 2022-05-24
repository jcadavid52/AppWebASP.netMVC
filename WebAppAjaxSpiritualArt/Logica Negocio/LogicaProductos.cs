using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppAjaxSpiritualArt.Models;

namespace WebAppAjaxSpiritualArt.Logica_Negocio
{
    public class LogicaProductos
    {
        //listar obras de los artistas premium
        public List<PRODUCTO> listarObrasPremium()
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Include("REGISTRO_ARTISTA").Where(p => p.REGISTRO_ARTISTA.FK_TIPO_PLAN == 9).ToList();
            }
        }

        //método que consulta las obras específicas de un solo artista
        public List<PRODUCTO> consultarObra(int fk_artista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Where(P => P.FK_ARTISTA == fk_artista).ToList();
            }
        }
        //Agregar obras
        public bool AgregarObra(PRODUCTO nuevaObra)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                try
                {
                bd.PRODUCTO.Add(nuevaObra);
                bd.SaveChanges();
                    return true;
                }catch(Exception e)
                {
                    return false;
                }
            }
        }

        //listar todas las obras con los artistas

        public List<PRODUCTO> listarObras()
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Include("REGISTRO_ARTISTA").ToList();
            }
        }

        public List<PRODUCTO> listarObras(int id_categoria)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Include("REGISTRO_ARTISTA").Where(P => P.FK_CATEGORIA == id_categoria).ToList();
            }
        }
        
        public List<PRODUCTO> listaDetalleObraCompra(int? fk)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Include("REGISTRO_ARTISTA").Include("CATEGORIA").Where(P => P.FK_ARTISTA == fk).ToList();
            }
        }

        public PRODUCTO detalleObraCompra(int id)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.PRODUCTO.Include("REGISTRO_ARTISTA").Include("CATEGORIA").FirstOrDefault(P => P.PK_ID_PRODUCTO == id);
            }
        }
    }
}