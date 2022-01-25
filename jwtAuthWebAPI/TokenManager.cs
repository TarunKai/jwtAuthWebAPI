using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace jwtAuthWebAPI
{
	public class TokenManager
	{
		
		public static string GenerateToken(string userName)
		{
		//var Secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication")).ToString();

		//	byte[] Key = Convert.FromBase64String(Secret);

			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));
			var Credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) }),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = Credentials
			};
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(descriptor);
			return tokenHandler.WriteToken(token);
		}

		public static ClaimsPrincipal GetClaimsPrincipal(string token)
		{
			try
			{
				JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)handler.ReadToken(token);
				if (jwtSecurityToken==null)
				{
					return null;
				}

				//byte[] key = Convert.FromBase64String("aksjkajskl");

				SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication"));
				TokenValidationParameters parameters = new TokenValidationParameters()
				{
					RequireExpirationTime = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					//IssuerSigningKey = new SymmetricSecurityKey(key)
					IssuerSigningKey = key,
				};

				SecurityToken securityToken;
				ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out securityToken);
				return principal;
			}
			catch (Exception)
			{

				return null;
			}
		}

		/// <summary>
		/// It will check if another user want to use same token it will give invalid
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static string ValidateToken(string token)
		{
			string username = null;
			ClaimsPrincipal principal = GetClaimsPrincipal(token);
			if (principal==null)
			{
				return null;
			}
			ClaimsIdentity identity = null;
			try
			{
				identity = (ClaimsIdentity)principal.Identity;
			}
			catch(NullReferenceException)
			{
				return null;
			}
			Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
			username = usernameClaim.Value;
			return username;
		}
	}
}