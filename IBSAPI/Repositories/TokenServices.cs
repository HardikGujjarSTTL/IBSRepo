using IBSAPI.DataAccess;
using IBSAPI.Interfaces;
using IBSAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IBSAPI.Repositories
{
    public class TokenServices : ITokenServices
    {
        private readonly ModelContext context;
        private readonly IConfiguration _config;

        public TokenServices(ModelContext context, IConfiguration configuration)
        {
            this.context = context;
            _config = configuration;
        }


        public TokenEntity GenerateToken(string userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(Convert.ToDouble(_config["MyAppSettings:AuthTokenExpiry"]));
            var tokendomain = new DataAccess.Token
            {
                UserId = userId,
                Authtoken = token,
                Issueon = issuedOn,
                Expireson = expiredOn
            };

            context.Tokens.Add(tokendomain);
            context.SaveChanges();
            var tokenModel = new TokenEntity()
            {
                TokenId = tokendomain.Tokenid,
                UserId = userId,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                AuthToken = token,

            };

            return tokenModel;
        }

        public bool ValidateToken(string tokenId)
        {
            var token = context.Tokens
                .Where(t => t.Authtoken == tokenId && t.Expireson > DateTime.Now)
                .FirstOrDefault();

            if (token != null && !(DateTime.Now > token.Expireson))
            {
                token.Expireson = token.Expireson.Value.AddSeconds(Convert.ToDouble(_config["MyAppSettings:AuthTokenExpiry"]));
                context.Entry(token).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }

            return false;
        }

        public bool ValidateToken(string tokenId, string user_id)
        {
            var token = context.Tokens.Where(t => t.Authtoken == tokenId && t.UserId == user_id && t.Expireson > DateTime.Now).FirstOrDefault();
            if (token != null && !(DateTime.Now > token.Expireson))
            {
                token.Expireson = token.Expireson.Value.AddSeconds(Convert.ToDouble(_config["MyAppSettings:AuthTokenExpiry"]));
                context.Entry(token).State = EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            if (_config["MyAppSettings:Isdev"] != null)
                return true;
            return false;
        }

        public bool Kill(string tokenId)
        {
            var token = context.Tokens.Where(t => t.Authtoken == tokenId).FirstOrDefault();
            if (token != null)
            {
                //token.ExpiresOn = DateTime.Now.AddMinutes(-10);
                //context.Entry(token).State = System.Data.Entity.EntityState.Modified;
                context.Entry(token).State = EntityState.Deleted;
                context.SaveChanges();
            }

            var isNotDeleted = ValidateToken(tokenId);
            if (isNotDeleted) { return false; }
            return true;
        }

        public bool DeleteByUserId(string userId)
        {
            var token = context.Tokens.Where(t => t.UserId == userId).FirstOrDefault();
            if (token != null)
            {
                context.Entry(token).State = EntityState.Deleted;
                context.SaveChanges();
            }

            var isNotDeleted = GetTokenByUserID(userId);
            if (isNotDeleted) { return false; }
            return true;
        }

        public bool InActiveOldActiveTokens(string UserId, string AuthToken)
        {
            var success = false;

            using (context)
            {
                //var tokenlist = context.Tokens.Where(t => t.User_Id == UserId && t.AuthToken != AuthToken && t.ExpiresOn > DateTime.Now).ToList();
                var tokenlist = context.Tokens.Where(t => t.UserId == UserId && t.Authtoken != AuthToken).ToList();
                if (tokenlist.Any())
                {
                    foreach (DataAccess.Token token in tokenlist)
                    {
                        //token.ExpiresOn = DateTime.Now.AddSeconds(-10);
                        //context.Entry(token).State = System.Data.Entity.EntityState.Modified;

                        context.Entry(token).State = EntityState.Deleted;
                        context.SaveChanges();
                    }

                    success = true;
                }
            }
            return success;
        }

        public string GetUserOnValidateToken(string tokenId)
        {
            var token = context.Tokens.Where(t => t.Authtoken == tokenId && t.Expireson > DateTime.Now).FirstOrDefault();
            if (token != null && !(DateTime.Now > token.Expireson))
            {
                token.Expireson = token.Expireson.Value.AddSeconds(Convert.ToDouble(_config["MyAppSettings:AuthTokenExpiry"]));
                context.Entry(token).State = EntityState.Modified;
                context.SaveChanges();

                var user = context.T02Users.Where(x => x.UserId == token.UserId).FirstOrDefault();
                if (user != null)
                    return "";//need to change
            }
            return "";
        }

        public bool GetTokenByUserID(string userId)
        {
            var token = context.Tokens.Where(t => t.UserId == userId && t.Expireson > DateTime.Now).FirstOrDefault();
            if (token != null && !(DateTime.Now > token.Expireson))
            {
                return true;
            }
            return false;
        }

        public string GetUserByToken(string tokenid)
        {
            var token = context.Tokens.Where(t => t.Authtoken == tokenid).FirstOrDefault();
            if (token != null)
            {
                return token.UserId;
            }
            return "0";
        }
    }
}
