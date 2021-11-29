using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace api.fakebookTests.helpers
{
    static class Helper
    {

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        public static ClaimsPrincipal GetRandomUser(string name = null, string id = null)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, name ?? Helper.RandomString(7)),
                new Claim(ClaimTypes.NameIdentifier, id ?? Guid.NewGuid().ToString()),
            }, "mock"));
        }



    }
}
