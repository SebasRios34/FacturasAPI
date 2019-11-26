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
    public class FacturaController : ApiController
    {
        public string Get()
        {
            return new Factura().mostrarFactura();
        }

        public string Get(int id)
        {
            return new Cliente().mostrarCodigoCliente();
        }

        public string Post([FromBody]Factura factura)
        {
            return factura.insertarFactura("Insertar") ? "Se añadieron con exito" : "No se logro guardar un nuevo usuario";
        }
    }
}