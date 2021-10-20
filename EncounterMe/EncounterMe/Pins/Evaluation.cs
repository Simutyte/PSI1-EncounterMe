// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncounterMe.Pins
{
    public class Evaluation
    {
        public double average { get; set; }

        public string name { get; set; }

        public int[] allEvaluations { get; set; }

        public Evaluation(string name, IEnumerable<int> evaluations = null)
        {
            this.name = name;
            allEvaluations = evaluations.ToArray();
        }

        public void Append(int value)
        {
            allEvaluations = allEvaluations.Concat(new int[] { value }).ToArray();
        }

        public void CountAverage()
        {
            if (allEvaluations != null)
            {
                average = Convert.ToDouble(allEvaluations.Average());
            }
        }
    }
}
