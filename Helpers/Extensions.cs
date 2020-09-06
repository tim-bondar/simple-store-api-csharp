using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SimpleStore.Helpers
{
    public static class Extensions
    {
        public static Guid? ExtractUserId(this IHttpContextAccessor contextAccessor)
        {
            var userId = contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

            if (userId != null && Guid.TryParse(userId, out Guid id))
                return id;

            return null;
        }
    }
}
