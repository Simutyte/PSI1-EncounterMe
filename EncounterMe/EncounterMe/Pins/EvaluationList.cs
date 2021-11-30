// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EncounterMe.Pins
{
    class EvaluationList
    {
        private static EvaluationList s_instance;

        private static readonly object s_locker = new object();

        public List<Evaluation> ListOfEvaluation = new List<Evaluation>();

        private protected EvaluationList()
        {

        }

        public static EvaluationList GetEvaluationList()
        {
            if (s_instance == null)
            {
                lock (s_locker)
                {
                    if (s_instance == null)
                    {
                        s_instance = new EvaluationList();
                    }
                }
            }
            return s_instance;
        }

        public void AddEvaluation(int mapPinId, int userId, int value)
        {
            ListOfEvaluation.Add(new Evaluation(mapPinId, userId, value));
        }

        public void ChangeEvaluation(int mapPinId, int userId, int newValue)
        {
            ListOfEvaluation.First(e => e.MapPinId == mapPinId && e.UserId == userId).Value = newValue;
        }

        public int GetMapPinEvaluationFromUserId(int mapPinId, int userId)
        {
            return ListOfEvaluation.First(e => e.MapPinId == mapPinId && e.UserId == userId).Value;
        }

        public double GetMapPinEvaluationAverage(int mapPinId)
        {
            return ListOfEvaluation.Where(e => e.MapPinId == mapPinId).Average(e => e.Value);
        }
    }
}
