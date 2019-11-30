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
    public class EncabezadoFacturaController : ApiController
    {
        public string Post([FromBody] EncabezadoFactura encabezadoFactura)
        {
            return encabezadoFactura.insertarEncabezadoFactura("Insertar") ? "Se añadieron con exito" : "No se logro guardar un nuevo usuario";
        }

        public string Delete(int codigoFactura)
        {
            return new EncabezadoFactura().eliminarEncabezadoFactura(codigoFactura) ? "Se elimino el encabezado de la factura con codigoFactura: " + codigoFactura : "No se elimino el encabezado de la factura con el codigo: " + codigoFactura;
        }
    }
}