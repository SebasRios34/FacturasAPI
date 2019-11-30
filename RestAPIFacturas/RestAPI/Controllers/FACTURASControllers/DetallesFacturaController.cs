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
    public class DetallesFacturaController : ApiController
    {
        public string Post([FromBody] DetallesFactura detallesFactura)
        {
            return detallesFactura.insertarDetallesFactura("Insertar") ? "Se añadieron con exito" : "No se logro guardar un nuevo usuario";
        }

        public string Delete(int codigoFactura)
        {
            return new DetallesFactura().eliminarDetallesFactura(codigoFactura) ? "Se elimino los detalles de la factura con codigoCliente: " + codigoFactura : "No se elimino los detalles de la factura con el codigo: " + codigoFactura;
        }
    }
}