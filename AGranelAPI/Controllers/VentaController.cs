using AGranelAPI.Models;
using AGranelAPI.Models.DTO;
using AGranelAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace AGranelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly ILogger<VentaController> _logger;
        private readonly IProductRepository _productRepo;
        private readonly IVentaRepository _ventaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response; 
        public VentaController(ILogger<VentaController> logger, IVentaRepository ventaRepo, IProductRepository productRepo, IMapper mapper)
        {
            _logger = logger;
            _ventaRepo = ventaRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponseVenta<IEnumerable<VentaDTO>>>> GetAllVentas()
        {
            try
            {
                _logger.LogInformation("Obteniendo las ventas");

                // Obtener las ventas de la base de datos
                var ventas = await _ventaRepo.ObtenerVentas();
                List<VentaDTO> ventaList = new List<VentaDTO>();

                foreach (var venta in ventas)
                {
                    var ventasDTO = new VentaDTO();
                    ventasDTO.VentaID = venta.VentaID;
                    string fechaFormateada = venta.FechaDeVenta.ToString("yyyy-MM-dd");
                    ventasDTO.FechaDeVenta = fechaFormateada;
                    ventasDTO.Monto = venta.Monto;
                    ventasDTO.DetalleVentas = new List<DetalleVentaDTO>();
                    foreach (var detalle in venta.DetallesVenta)
                    {
                        var detalleDTO = new DetalleVentaDTO();
                        detalleDTO.DetalleID = detalle.DetalleID;
                        detalleDTO.ProductoID = detalle.ProductoID;
                        detalleDTO.CantidadVendida = detalle.CantidadVendida;
                        ventasDTO.DetalleVentas.Add(detalleDTO);
                    }
                    ventaList.Add(ventasDTO);
                }


                var response = new APIResponseVenta<IEnumerable<VentaDTO>>
                {
                    Resultado = ventaList,
                    statusCode = HttpStatusCode.OK,
                    IsExitoso = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new APIResponseVenta<IEnumerable<VentaDTO>>
                {
                    IsExitoso = false,
                    ErrorMessages = new List<string>() { ex.ToString() }
                };
                return BadRequest(response);
            }
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponseVenta<VentaDTO>>> GetVenta(int id)
        {
            try
            {
                _logger.LogInformation($"Obteniendo la venta con ID {id}");

                var venta = await _ventaRepo.ObtenerVentaPorId(id);

                if (venta == null)
                {
                    return NotFound();
                }

                var ventaDTO = new VentaDTO
                {
                    VentaID = venta.VentaID,
                    FechaDeVenta = venta.FechaDeVenta.ToString("yyyy-MM-dd"),
                    Monto = venta.Monto,
                    DetalleVentas = venta.DetallesVenta.Select(detalle => new DetalleVentaDTO
                    {
                        DetalleID = detalle.DetalleID,
                        ProductoID = detalle.ProductoID,
                        CantidadVendida = detalle.CantidadVendida
                    }).ToList()
                };

                return Ok(ventaDTO);
            }
            catch (Exception ex)
            {
                var response = new APIResponseVenta<VentaDTO>
                {
                    IsExitoso = false,
                    ErrorMessages = new List<string>() { ex.ToString() }
                };
                return BadRequest(response);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CrearVenta([FromBody] VentaCreateDTO createDTO)
        { 
            try
            {
                if (createDTO == null)
                {
                    return BadRequest("La solicitud no contiene datos válidos para crear la venta.");
                }

                Venta model = _mapper.Map<Venta>(createDTO);
                decimal montoTotal = 0;


                foreach (var detalle in createDTO.DetallesVenta)
                {
                    var producto = await _productRepo.Obtener(p => p.ProductoID == detalle.ProductoID);

                    if (producto != null)
                    {
                        if (producto.CantidadEnInventario >= detalle.CantidadVendida)
                        {
                            decimal montoPorProducto = detalle.CantidadVendida * producto.Precio;
                            montoTotal += montoPorProducto;
                            producto.CantidadEnInventario -= detalle.CantidadVendida;
                            await _productRepo.Update(producto);
                        }
                        else
                        {
                            return BadRequest("La cantidad vendida supera el inventario disponible para el producto.");
                        }
                    }
                    else
                    {
                        return BadRequest($"El producto con ID {detalle.ProductoID} no se encuentra en la base de datos.");
                    }
                }
                model.Monto = montoTotal;
                await _ventaRepo.Crear(model);
                _response.Resultado = _mapper.Map<VentaDTO>(model);
                _response.statusCode = HttpStatusCode.Created;
                _response.IsExitoso = true;
                return CreatedAtAction("GetVenta", new { id = model.VentaID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVenta(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var venta = await _ventaRepo.Obtener(v => v.VentaID == id);
                if (venta == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _ventaRepo.Remover(venta);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsExitoso = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVenta(int id, [FromBody] VentaDTO updateDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(ModelState);
                }
                var venta = await _ventaRepo.Obtener(v => v.VentaID == id);
                if (venta == null)
                {
                    _response.IsExitoso = false;
                    _response.statusCode = HttpStatusCode.NotFound;
                    ModelState.AddModelError("No existe", "La venta con ese id NO existe");
                }
                _mapper.Map(updateDTO, venta);
                await _ventaRepo.Update(venta!);
                _response.statusCode = HttpStatusCode.NoContent;
                _response.IsExitoso = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}
