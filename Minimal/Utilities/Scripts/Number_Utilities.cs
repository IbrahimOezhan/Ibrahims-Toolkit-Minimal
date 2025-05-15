using System.Collections.Generic;
using UnityEngine;

namespace TemplateTools
{
    public class Number_Utilities
    {
        public static int GetRandomEvent(List<int> chances)
        {
            List<int> startAt = new();

            int totalValue = -1;

            for (int i = 0; i < chances.Count; i++)
            {
                startAt.Add(totalValue + 1);
                totalValue += chances[i];
            }

            int rdm = Random.Range(0, totalValue);

            for (int i = 0; i < chances.Count; i++)
            {
                if (rdm >= startAt[i] && rdm < startAt[i] + chances[i])
                {
                    return i;
                }
            }

            return Random.Range(0, chances.Count);
        }

        public static float Map(float _x, float _in_min, float _in_max, float _out_min, float _out_max)
        {
            return (_x - _in_min) * (_out_max - _out_min) / (_in_max - _in_min) + _out_min;
        }

        public static int LoopNumber(int number, int min, int max)
        {
            if (number < min)
            {
                return max;
            }
            else if (number > max)
            {
                return min;
            }
            return number;
        }

        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}