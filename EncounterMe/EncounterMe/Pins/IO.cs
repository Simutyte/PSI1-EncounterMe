// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;

namespace EncounterMe
{
    static class IO
    {
        //public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = true)
        //{
        //    using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        //    {
        //        var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        //        binaryFormatter.Serialize(stream, objectToWrite);
        //    }
        //}

        //public static List<T> ReadFromBinaryFile<T>(string filePath)
        //{
        //    var list = new List<T>();

        //    try
        //    {
        //        using (Stream stream = File.Open(filePath, FileMode.Open))
        //        {
        //            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

        //            while (stream.Position < stream.Length)
        //            {
        //                T obj = (T)binaryFormatter.Deserialize(stream);
        //                list.Add(obj);
        //            }
        //            return list;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
