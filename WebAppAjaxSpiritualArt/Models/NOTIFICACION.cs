//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppAjaxSpiritualArt.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NOTIFICACION
    {
        public int PK_ID_NOTIFICACION { get; set; }
        public Nullable<System.DateTime> FECHA { get; set; }
        public Nullable<int> FK_CLIENTE { get; set; }
        public Nullable<int> FK_ARTISTA { get; set; }
        public Nullable<int> FK_PRODUCTO { get; set; }
    
        public virtual CLIENTE CLIENTE { get; set; }
        public virtual REGISTRO_ARTISTA REGISTRO_ARTISTA { get; set; }
        public virtual PRODUCTO PRODUCTO { get; set; }
    }
}
