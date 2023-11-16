using AGranelAPI.DataContext;
using AGranelAPI.Models;
using AGranelAPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using AGranelAPI.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace AGranelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _db;
        protected APIResponse _response;
        public AuthController(IConfiguration config, ILogger<AuthController> logger, IUserRepository userRepo, IMapper mapper, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _response = new ();
        }
        private string GenerateToken(User user, List<Rol> roles)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username.ToString())
        // Puedes agregar más claims según tu modelo de roles, como ClaimTypes.Role
    };

            // Agregar los roles del usuario como claims
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol.NombreDelRol));
                // Puedes personalizar el claim según tu estructura de roles
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var secToken = new JwtSecurityToken(
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(secToken);
            return token;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<LoginDTO>> LoginUsuario(UserLoginDTO request)
        {
            var user = await _db.Usuarios
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Rol)
                .SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
            {
                return BadRequest("Usuario no encontrado");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return BadRequest("Contraseña incorrecta");
            }

            // Obtener los roles del usuario
            var roles = user.Roles.Select(ur => ur.Rol).ToList();

            var token = GenerateToken(user, roles);

            // Crear una clase de respuesta para contener el nombre del usuario y el token
            var response = new LoginDTO
            {
                UserId = user.UserID,
                UserName = user.Username, // Ajusta según la propiedad real que contenga el nombre del usuario
                Token = token
            };

            return Ok(response);
        }


        [HttpPost("Register")]
        public async Task<ActionResult<APIResponse>> Register(UserRegisterDTO request)
        {
            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _db.Usuarios.AnyAsync(u => u.Username == request.Username))
                {
                    ModelState.AddModelError("UserName", "El nombre de usuario ya está en uso.");
                }
                else
                {
                    var user = new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Password = passwordHash
                    };
                    user.Roles = new List<UserRol>
            {
                new UserRol
                {
                    UserID = user.UserID,
                    RoleID = request.Rol
                }
            };

                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve,
                        // Otras opciones si es necesario
                    };

                    var result = _db.Usuarios.Add(user);
                    if (result != null)
                    {
                        _response.Resultado = user;
                        _response.statusCode = HttpStatusCode.OK;
                        _response.IsExitoso = true;
                        await _db.SaveChangesAsync();

                        var json = JsonSerializer.Serialize(_response, options);
                        return Content(json, "application/json"); // Devuelve la respuesta JSON
                    }
                    else
                    {
                        ModelState.AddModelError("", "Hubo un error al registrar el usuario.");
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            var errorJson = JsonSerializer.Serialize(_response);
            return Content(errorJson, "application/json"); // Devuelve la respuesta JSON en caso de error
        }

       [HttpGet("Usuarios")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       public async Task<ActionResult<APIResponseVenta<IEnumerable<UserDTO>>>> GetAllUsuarios()
       {
            try
                {
                   _logger.LogInformation("Obtener los usuarios");

                   var users = await _userRepo.ObtenerUsuarios();
                   List<UserDTO> userList = new List<UserDTO>();

                foreach (var user in users)
                {
                    var userDTO = new UserDTO();
                    userDTO.UserID = user.UserID;
                    userDTO.Username = user.Username;
                    userDTO.Email = user.Email;
                    userDTO.Roles = new List<RolDTO>();
                    foreach (var rol in user.Roles) 
                    {
                        var rolDTO = new RolDTO();
                        rolDTO.RoleID = rol.RoleID;
                        userDTO.Roles.Add(rolDTO);
                    }
                    userList.Add(userDTO);
                }

                var response = new APIResponseVenta<IEnumerable<UserDTO>>
                {
                    Resultado = userList,
                    statusCode = HttpStatusCode.OK,
                    IsExitoso = true
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
               _response.IsExitoso = false;
               _response.ErrorMessages = new List<string>() { ex.ToString() };
                return BadRequest(_response);
            }
        }


        [HttpGet("id:int", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetUsuario(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer usuario con el id" + id);
                    _response.statusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);
                }

                //var agranel = AGranelStore.aGranelList.FirstOrDefault(a => a.Id == id);
                var user = await _userRepo.Obtener(u => u.UserID == id);

                if (user == null)
                {
                    _response.statusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<UserDTO>(user);
                _response.statusCode = HttpStatusCode.OK;
                _response.IsExitoso = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
