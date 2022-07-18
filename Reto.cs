using Cancha10.Servicio.BusinessLogic;
using Cancha10.Servicio.Entidades;
using Cancha10.Servicio.ServiceWebAPI.Controllers.Base;
using System;
using System.Web;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using System.IO;
using Cancha10.Servicio.Common;

namespace Cancha10.Servicio.ServiceWebAPI.Controllers.Alquiler
{
    public class AlquilerController : BaseApiController
    {
        private AlquilerBusinessLogic oAlquilerBusinessLogic = new AlquilerBusinessLogic();
        private BD<Producto> oBase = new BD<Producto>(@"C:\Users\dbuenpas\Documents\BD\productos.json");


        [HttpGet]
        [ActionName("SearchProduct")]
        [ResponseType(typeof(int))]
        [Route("api/reto/SearchProduct")]
        public IHttpActionResult SearchProduct(Producto producto)
        {
            oBase.Cargar();
            List<Producto> oListaProducto = new List<Producto>();
            oListaProducto = oBase.Consultar(x => x.price < producto.price && producto.price > x.price);
            return Ok(oListaProducto);
        }

        [HttpPost]
        [ActionName("InsertProduct")]
        [ResponseType(typeof(int))]
        [Route("api/reto/InsertProduct")]
        public IHttpActionResult InsertProduct(Producto oProducto)
        {
            oBase.Cargar();
            oProducto.id = Convert.ToInt32(generarCodigo());
            oProducto.salePrice = oProducto.price * (1 + Constantes.IGV);
            oBase.Insert(oProducto);
            List<Producto> oListaProducto = new List<Producto>();
            oListaProducto = oBase.ConsultarEmpty();
            return Ok(oListaProducto);
        }

        [HttpPut]
        [ActionName("UpdateProduct")]
        [ResponseType(typeof(int))]
        [Route("api/reto/UpdateProduct")]
        public IHttpActionResult UpdateProduct(Producto oProducto)
        {
            List<Producto> oListaProducto = new List<Producto>();
            oBase.Cargar();
            int codigo = oProducto.id;
            oProducto.salePrice = oProducto.price * (1 + Constantes.IGV);
            oBase.Update(x => x.id == codigo, oProducto);
            oListaProducto = oBase.Consultar(x => x.id == codigo);
            return Ok(oListaProducto);
        }

        public string generarCodigo()
        {
            Random rdm = new Random();
            string numeroconcatenado = String.Empty;
            //String[] arrayNumeros;
            //int numero = rdm.Next(1000, 9999);
            //arrayNumeros = numero.ToString().Split('');
            int sumainicial = 0;
            int sumasecundaria = 0;
            for (int i = 0; i < 4; i++)
            {
                int numerordm = rdm.Next(0, 9);
                numeroconcatenado = numeroconcatenado + numerordm.ToString();
                sumainicial = sumainicial + numerordm;
            }

            //char[] charArray = suma.ToString().ToCharArray();
            //char[] first = charArray[0];

            int first = Convert.ToInt32(sumainicial.ToString().Substring(0, 1));
            int second = sumainicial.ToString().Length > 1 ? Convert.ToInt32(sumainicial.ToString().Substring(1, 1)) : 0;
            sumasecundaria = first + second;

            string concatenadofinal = numeroconcatenado + string.Empty + sumasecundaria.ToString();

            return concatenadofinal;
        }
    }
}