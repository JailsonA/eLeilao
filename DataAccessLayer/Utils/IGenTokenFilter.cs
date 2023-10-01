using DataAccessLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataAccessLayer.Utils
{
    public interface IGenTokenFilter
    {
        string GenerateToken(UserModel user);
    }

    /* implementation  */
    public class GenTokenFilter : IGenTokenFilter
    {
        private IConfiguration _configuration;

        public GenTokenFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /*Generate Secret key*/
        private string GenerateRandomSecretKey(int length)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                sb.Append(validChars[index]);
            }
            return sb.ToString();
        }

        /*Generate Token*/
        public string GenerateToken(UserModel user)
        {
            try
            {
                if (user != null)
                {
                    string signingKey = GenerateRandomSecretKey(64);

                    // Atualiza a chave no arquivo appsettings.json
                    string appSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "sKey.json");
                    bool isUpdate = UpdateOrCreateJwtKeyForUser(appSettingsPath, user.UserId, signingKey);
                    if (!isUpdate) return null;

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("FullName", user.FullName),
                        new Claim("UserType", user.UserType.ToString())
                    };

                    var signIn = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)), SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(90),
                        signingCredentials: signIn);

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /* Update secret key on sKey.json */

        private bool UpdateOrCreateJwtKeyForUser(string appSettingsPath, int userId, string newKey)
        {
            try
            {
                // Lê o conteúdo do arquivo appsettings.json
                string json = File.ReadAllText(appSettingsPath);

                // Converte o JSON em um objeto representando as configurações
                AppSettings appSettings = System.Text.Json.JsonSerializer.Deserialize<AppSettings>(json);

                // Verifica se a chave JwtKeys já existe no objeto AppSettings
                if (appSettings.JwtKeys == null)
                {
                    appSettings.JwtKeys = new Dictionary<string, string>();
                }

                // Atualiza a chave JWT para o usuário específico
                appSettings.JwtKeys[userId.ToString()] = newKey;

                // Converte as configurações atualizadas de volta para JSON
                string updatedJson = System.Text.Json.JsonSerializer.Serialize(appSettings, new JsonSerializerOptions
                {
                    WriteIndented = true // Para formatar o JSON de maneira legível
                });

                // Escreve o JSON atualizado de volta para o arquivo appsettings.json
                File.WriteAllText(appSettingsPath, updatedJson);

                return true;
            }
            catch (Exception ex)
            {
                // Lide com exceções, se necessário
                return false;
            }
        }


        // Defina uma classe para representar a estrutura do arquivo appsettings.json
        public class AppSettings
        {
            [JsonPropertyName("Jwt")]
            public Dictionary<string, string> JwtKeys { get; set; } = new Dictionary<string, string>();
        }

    }

}
