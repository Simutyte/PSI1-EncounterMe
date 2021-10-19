// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;

namespace EncounterMe
{
    static class IO
    {
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = true)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        public static T ReadFromBinaryFile<T>(string filePath) where T : List<MapPin>, new()
        {
            T list = new T();

            try
            {
                using (Stream stream = File.Open(filePath, FileMode.Open))
                {
                    var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    while (stream.Position < stream.Length)
                    {
                        MapPin obj = (MapPin)binaryFormatter.Deserialize(stream);
                        list.Add(obj);
                    }
                    return (T)list;
                }
            }
            catch (Exception) //pirma karta failas dar ner sukurtas ir jo neranda
            {
                return (T)list;
            }
            
        }
    }
}
