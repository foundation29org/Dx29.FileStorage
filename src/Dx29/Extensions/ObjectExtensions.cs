﻿using System;
using System.Text;
using System.Security.Cryptography;

using Newtonsoft.Json;

namespace Dx29
{
    static public class ObjectExtensions
    {
        static public string Serialize(this object obj, bool indented = true)
        {
            return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
        }

        static public T Deserialize<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        static public string AsString(this byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }

        static public string HashString(this string str)
        {
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
