using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using BLL;

namespace RestAPI.Controllers.FACTURASControllers
{
    public class ClienteController : ApiController
    {
        public string Get()
        {
            return new Cliente().mostrarCliente();
        }

        public string Get(int id)
        {
            return new Cliente().mostrarCodigoCliente();
        }

        public string Post([FromBody]Cliente cliente)
        {
            return cliente.insertarCliente("Insertar") ? "Se añadieron con exito" : "No se logro guardar un nuevo usuario";
        }
    }
}