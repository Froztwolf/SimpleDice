using System;
using UnityEngine;

namespace SimpleDice.Utils
{
    public static class SimpleDiceUtils
    {
        public static bool AngleWithinError(float actualValue, float targetValue, float allowedError)
        {
            // Keep the angles from 0 to 360
            actualValue = Mathf.RoundToInt(actualValue);
            actualValue %= 360;

            // We allow this to exceed the 0 to 360 range and handle it accordingly below
            float upperbound = (targetValue + allowedError);
            float lowerbound = (targetValue - allowedError);

            bool withinBounds = false;

            Debug.Assert(actualValue <= 360, "Angle: " + actualValue);
            Debug.Assert(actualValue >= 0, "Angle: " + actualValue);

            //Check if the lower bound laps the 360 degree circle and segment the test accordingly
            if (lowerbound < 0)
            {
                lowerbound += 360; //-1 degree = 359 degrees

                if (actualValue > lowerbound && actualValue <= 360)
                {
                    withinBounds = true;
                }
                else if (actualValue >= 0 && actualValue < upperbound)
                {
                    withinBounds = true;
                }
            }

            //Check if the upper bound laps the 360 degree circle and segment the test accordingly
            else if (upperbound > 360)
            {
                upperbound -= 360; // 361 degrees = 1 degree

                if (actualValue > lowerbound && actualValue <= 360)
                {
                    withinBounds = true;
                }
                else if (actualValue >= 0 && actualValue < upperbound)
                {
                    withinBounds = true;
                }
            }

            // Default case where nothing laps the circle
            else if (actualValue > lowerbound && actualValue < upperbound)
            {
                withinBounds = true;
            }

            return withinBounds;
        }

        public static bool VectorWithinError(Vector3 actualVector, Vector3 targetVector, float allowedError)
        {
            float error = Vector3.Angle(actualVector, targetVector);
            return (error < allowedError);
        }

        public static float RandomFloatRandomSign(float min, float max)
        {
            // Returns a float value within the range specified, either negative or positive

            // Get 0 or 1 randomly
            int sign = UnityEngine.Random.Range(0, 1) <= 0.5 ? -1 : 1;

            // Get the value and multiply with the sign
            float randomFloat = UnityEngine.Random.Range(min, max) * sign;

            return randomFloat;
        }
    }
}
