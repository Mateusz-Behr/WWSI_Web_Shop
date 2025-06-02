using HashidsNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop_3.Application.Utils
{
    public static class HashUtils
    {
        public static string EncodeHashid(this ulong id, IHashids hashIds)
        {
            return hashIds.EncodeLong((long)id);
        }

        public static string EncodeHashid(this ulong? id, IHashids hashIds)
        {
            ulong hashedId = id ?? default;

            return hashedId.EncodeHashid(hashIds);
        }

        public static ulong DecodeHashid(this string hashid, IHashids hashIds)
        {
            return (ulong)hashIds.DecodeSingleLong(hashid);
        }

        public static IEnumerable<ulong> DecodeHashIds(this IEnumerable<string> iDs, IHashids hashIds)
        {
            foreach (var id in iDs)
            {
                yield return id.DecodeHashid(hashIds);
            }
        }
    }
}
