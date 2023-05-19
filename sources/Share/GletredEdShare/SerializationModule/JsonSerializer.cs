// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.IO;

namespace GletredEdShare.SerializationModule
{
    public static class JsonSerializer
    {
        public static bool ToJson<T>(T file, string path)
            where T : class
        {
            try
            {
                using var stream = new StreamWriter(new FileStream(path, FileMode.Create));
                stream.Write(System.Text.Json.JsonSerializer.Serialize(file));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static T FromJson<T>(string path)
            where T : class
        {
            try
            {
                using var stream = new StreamReader(new FileStream(path, FileMode.Open));
                return System.Text.Json.JsonSerializer.Deserialize<T>(stream.ReadToEnd()) ?? throw new InvalidOperationException();
            }
            catch (Exception)
            {
                throw new InvalidOperationException();
            }

        }

    }
}
