// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncounterMe.Pins
{
    [Serializable]
    public class Evaluation
    {
        public double Average { get; set; }

        public string Name { get; set; }

        public int[] AllEvaluations { get; set; }

        public Evaluation(string name, IEnumerable<int> evaluations = null)
        {
            this.Name = name;
            AllEvaluations = evaluations.ToArray();
        }

        public void Append(int value)
        {
            AllEvaluations = AllEvaluations.Concat(new int[] { value }).ToArray();
        }

        public void CountAverage()
        {
            if (AllEvaluations != null)
            {
                Average = Convert.ToDouble(AllEvaluations.Average());
            }
        }
    }
}
