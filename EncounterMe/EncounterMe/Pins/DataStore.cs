// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace EncounterMe.Pins
{
    class DataStore<T>
    {
        private T[] store;

        public DataStore()
        {
            store = new T[20];
        }

        public DataStore(int length)
        {
            store = new T[length];
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 && index >= store.Length)
                    throw new IndexOutOfRangeException("Index out of range");

                return store[index];
            }

            set
            {
                if (index < 0 || index >= store.Length)
                    throw new IndexOutOfRangeException("Index out of range");

                store[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return store.Length;
            }
        }
    }
}
