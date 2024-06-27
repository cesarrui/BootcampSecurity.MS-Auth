using BootcampSecurity.MS_Auth.Domain;
using BootcampSecurity.MS_Auth.Helpers;
using BootcampSecurity.MS_Auth.Infraestructure;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace BootcampSecurity.MS_Auth.Services
{
    public class AuthService
    {
        private readonly JwtHelper _jwtHelper;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenRepository _tokenRepository;
        public AuthService(JwtHelper jwtHelper, IUsuarioRepository usuarioRepository, ITokenRepository tokenRepository)
        {

            _jwtHelper = jwtHelper;
            _usuarioRepository = usuarioRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<Result<TokenApiModel>> GenerateToken(HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(requestBody)!;

                //Validar contra una base de datos el inicio de sesión ---------------->
                if (usuario == null)
                {
                    throw new ArgumentException("Se necesita un objeto para la deserialización");
                }

                if (string.IsNullOrWhiteSpace(usuario.usuario) || string.IsNullOrWhiteSpace(usuario.Clave))
                {
                    throw new ArgumentException("Campos obligatorios faltantes o inválidos");
                }

                var valor = await _usuarioRepository.FindUser(usuario);

                if (valor != null)
                {
                    var token = _jwtHelper.GenerateToken(valor.idUsuario.ToString());
                    var refreshToken = _jwtHelper.GenerateRefreshToken();


                    await _tokenRepository.CreateOrUpdate(new Token
                    {
                        IdUsuario = valor.idUsuario,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                    });

                    return Result<TokenApiModel>.Success(new TokenApiModel
                    {
                        Token = token,
                        RefreshToken = refreshToken

                    });

                }
                else
                {
                    throw new ArgumentException("Clave o usuarios invalidos");
                }

                
            }
            catch (Exception ex)
            {
                return Result<TokenApiModel>.Failure(ex.Message);
            }
        }

       

        public async Task<Result<bool>> ValidateToken(HttpRequest req)
        {
            try
            {

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                string token = requestBody;

                if (string.IsNullOrEmpty(token))
                {
                    throw new ArgumentException(message: "Token is null");
                }

                var principal = _jwtHelper.ValidateToken(token, true);
                if (principal == null)
                {
                    throw new ArgumentException(message: "Token isn't valid or expired");
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message);
            }
        }

        public async Task<Result<TokenApiModel>> RefreshToken(HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var tokenApiModel = JsonConvert.DeserializeObject<TokenApiModel>(requestBody)!;

                if (tokenApiModel is null)
                    throw new ArgumentException(message: "Token is empty");

                string accessToken = tokenApiModel.Token;
                string refreshToken = tokenApiModel.RefreshToken;

                var principal = _jwtHelper.ValidateToken(accessToken, false);
                if (principal is null)
                {
                    throw new ArgumentException(message: "Token isn't valid or expired");
                }

                var idusuario = principal.Identity!.Name;
                var token = new Token
                {
                    IdUsuario = Guid.Parse(idusuario!)

                };

                var tkt = await _tokenRepository.Get(token);

                if (tkt is null || tkt.RefreshToken != refreshToken || tkt.RefreshTokenExpiryTime <= DateTime.Now)
                    throw new ArgumentException(message: "Token has expired");

                var newAccessToken = _jwtHelper.GenerateToken(idusuario!);
                var newRefreshToken = _jwtHelper.GenerateRefreshToken();

                await _tokenRepository.CreateOrUpdate(new Token
                {
                    IdUsuario = Guid.Parse(idusuario!),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
                });

                return Result<TokenApiModel>.Success(new TokenApiModel
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken,
                });

            }
            catch (Exception ex)
            {
                return Result<TokenApiModel>.Failure(ex.Message);
            }
        }
    }
}
