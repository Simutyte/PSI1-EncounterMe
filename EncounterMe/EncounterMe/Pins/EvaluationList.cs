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
            ListOfEvaluation.FirstOrDefault(e => e.MapPinId == mapPinId && e.UserId == userId).Value = newValue;
        }

        public int GetMapPinEvaluationFromUserId(int mapPinId, int userId)
        {
            var evaluation = ListOfEvaluation.FirstOrDefault(e => e.MapPinId == mapPinId && e.UserId == userId);
            return evaluation == null ? 0 : evaluation.Value;
        }

        public double GetMapPinEvaluationAverage(int mapPinId)
        {
            var evaluations = ListOfEvaluation.Where(e => e.MapPinId == mapPinId);
            return evaluations == null ? 0 : evaluations.Average(e => e.Value);
        }
    }
}
