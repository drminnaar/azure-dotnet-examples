using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProductManagerFncAppV5.Data
{
    public interface IEntityIdProvider
    {
        string GetEntityId();
    }

    internal class HashEntityIdProvider : IEntityIdProvider
    {
        public string GetEntityId()
        {
            var data = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(data);
            var hashString = new StringBuilder();
            Enumerable
                .Range(0, hash.Length)
                .ToList()
                .ForEach(index => hashString.Append($"{hash[index]:X2}"));
            return hashString.ToString();
        }
    }
}
