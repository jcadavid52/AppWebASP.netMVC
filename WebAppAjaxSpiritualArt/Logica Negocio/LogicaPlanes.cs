using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppAjaxSpiritualArt.Models;

namespace WebAppAjaxSpiritualArt.Logica_Negocio
{
    public class LogicaPlanes
    {
        //listar planes por tipo
        public List<TIPO_PLAN> listarPlanes(int? id_tipoPlan)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                var planes = bd.TIPO_PLAN.Where(P => P.PK_ID_TIPO_PLAN == id_tipoPlan).ToList();

                return planes;
            }
        }

        //listar planes
        public List<TIPO_PLAN> listarPlanes()
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.TIPO_PLAN.ToList();


            }
        }
    }
}