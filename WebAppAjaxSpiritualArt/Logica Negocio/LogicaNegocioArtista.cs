using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using WebAppAjaxSpiritualArt.Models;

namespace WebAppAjaxSpiritualArt.Logica_Negocio
{
    public class LogicaNegocioArtista
    {
        //Registar artista
        public void RegistrarArtista(REGISTRO_ARTISTA nuevoArtista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {


                bd.SP_REGISTRAR_ARTISTA(nuevoArtista.NOMBRE_ARTISTA, nuevoArtista.PRIMER_APELLIDO_ARTISTA, nuevoArtista.SEGUNDO_APELLIDO_ARTISTA, nuevoArtista.TELEFONO,
                    nuevoArtista.CORREO, nuevoArtista.PAIS, nuevoArtista.CIUDAD, nuevoArtista.LOCALIDAD, nuevoArtista.DIRECCION, nuevoArtista.FK_TIPO_PLAN, nuevoArtista.CLAVE, nuevoArtista.ESTADO, nuevoArtista.IMAGEN);

                bd.SaveChanges();
            }
        }

        //enviar correo con usuario, contraseña y nombre de la persona registrada

        public void EnviarEmail(string EmailDestino, string claveArtista, string usuario, string nombre)
        {
            string EmailOrigen = "cadavidcamilo360@gmail.com";
            string clave = "pbczcevcfbxykbky";


            MailMessage omailMessage = new MailMessage(EmailOrigen, EmailDestino, "Bienvenido a SpiritualArt " + nombre, "<p>Ahora Puedes mostrar tu creatividad ingresando con tu usuario: " + usuario + " y tu contraseña: " + claveArtista + "</p>");
            omailMessage.IsBodyHtml = true;

            SmtpClient osmtpClient = new SmtpClient();
            osmtpClient.Host = "smtp.gmail.com";
            osmtpClient.EnableSsl = true;
            osmtpClient.UseDefaultCredentials = false;
            osmtpClient.Port = 587;
            osmtpClient.Credentials = new NetworkCredential(EmailOrigen, clave);


            osmtpClient.Send(omailMessage);
            osmtpClient.Dispose();

        }

        //enviar contraseña recuperada al email(sobrecarga)
        public void EnviarEmail(string EmailDestino, string claveArtista,string nombre)
        {
            string EmailOrigen = "cadavidcamilo360@gmail.com";
            string clave = "pbczcevcfbxykbky";


            MailMessage omailMessage = new MailMessage(EmailOrigen, EmailDestino, "Muy bien " + nombre, "<p> tu contraseña se ha recuperado exitosamente: " + claveArtista + "</p>");
            omailMessage.IsBodyHtml = true;

            SmtpClient osmtpClient = new SmtpClient();
            osmtpClient.Host = "smtp.gmail.com";
            osmtpClient.EnableSsl = true;
            osmtpClient.UseDefaultCredentials = false;
            osmtpClient.Port = 587;
            osmtpClient.Credentials = new NetworkCredential(EmailOrigen, clave);


            osmtpClient.Send(omailMessage);
            osmtpClient.Dispose();
        }



        //obtener clave del artista para enviarselo al correo

        public REGISTRO_ARTISTA obtenerClaveArtista(string correo, string nombre, string primerApellido, string segundoApellido)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.FirstOrDefault(A => A.CORREO == correo && A.NOMBRE_ARTISTA == nombre && A.PRIMER_APELLIDO_ARTISTA == primerApellido && A.SEGUNDO_APELLIDO_ARTISTA == segundoApellido);
            }

        }



        //inicio de sesión
        public REGISTRO_ARTISTA Acceso(REGISTRO_ARTISTA VerificarSesion)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.FirstOrDefault(RA => RA.CORREO == VerificarSesion.CORREO && RA.CLAVE == VerificarSesion.CLAVE);

            }
        }

        //Recuperar contraseña
        public REGISTRO_ARTISTA RecuperarClave(REGISTRO_ARTISTA VerificarCorreo)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.FirstOrDefault(RA => RA.CORREO == VerificarCorreo.CORREO);

            }
        }
        //modifica imagen del perfil del artista
        public void modificarImagen(REGISTRO_ARTISTA nuevaImagen)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                bd.Entry(nuevaImagen).State = System.Data.Entity.EntityState.Modified;
                bd.SaveChanges();
            }
        }

        //listar artistas

        public List<REGISTRO_ARTISTA> listarArtistas()
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.ToList();
            }
        }



        //listar informacion de un solo artista(sobrecarga)
        public List<REGISTRO_ARTISTA> listarArtistas(int id_artista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.Where(RA => RA.PK_ID_ARTISTA == id_artista).ToList();
            }
        }


        //mostrar noticiaciones según la fk del artista

        public List<NOTIFICACION> notificaciones(int fk_artista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.NOTIFICACION.Include("CLIENTE").Include("PRODUCTO").Where(N => N.FK_ARTISTA == fk_artista).ToList();
            }
        }

        //eliminar notificacion
        public bool EliminarNotificacion(NOTIFICACION id)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                try
                {
                  bd.Entry(id).State = System.Data.Entity.EntityState.Deleted;
                  bd.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        //cosulta de un artista
        public REGISTRO_ARTISTA ConsultaArtista(int id_artista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.REGISTRO_ARTISTA.FirstOrDefault(A => A.PK_ID_ARTISTA == id_artista);
            }
        }

        //publicar biografía
        public bool publicarBiografia(BIOGRAFIA nuevaBiografia)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                try
                {
                    bd.BIOGRAFIA.Add(nuevaBiografia);
                    bd.SaveChanges();
                    return true;
                }catch(Exception e)
                {
                    return false;
                }
            }
        }

        //consultar Biografia
        public BIOGRAFIA consultarBiografia(int fk_artista)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                return bd.BIOGRAFIA.FirstOrDefault(B => B.FK_REGISTRO_ARTISTA == fk_artista);
            }
        }

        //editar biografía
        public bool editarBiografia(BIOGRAFIA biografia)
        {
            using (BD_SPIRITUAL_ARTEntities bd = new BD_SPIRITUAL_ARTEntities())
            {
                try
                {
                    bd.Entry(biografia).State = System.Data.Entity.EntityState.Modified;
                    bd.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }


    }
}