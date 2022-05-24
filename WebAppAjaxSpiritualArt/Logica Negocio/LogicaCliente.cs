using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppAjaxSpiritualArt.Models;

namespace WebAppAjaxSpiritualArt.Logica_Negocio
{
    public class LogicaCliente
    {
        //registro cliente
        public bool registroCliente(CLIENTE cliente)
        {
            try
            {
                using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
                {
                    bd.SP_REGISTRO_CLIENTE(cliente.NOMBRE_CLIENTE, cliente.APELLIDO_CLIENTE, cliente.TELEFONO_CLIENTE, cliente.EMAIL_CLIENTE, cliente.ASUNTO, cliente.MENSAJE, cliente.FK_ARTISTA,cliente.FK_PRODUCTO);
                    bd.SaveChanges();

                }

                return true;
            }catch(Exception e)
            {
                return false;
            }
        }
    }
}