using UnityEngine;

public class Degree_Utilities
{
    public static float FormatDegree(float degree)
    {
        if (degree < 0) return 360 + degree;
        return degree;
    }

    public static float DegreeDifference(float degree1, float degree2)
    {
        return Mathf.Abs(degree1 - degree2);
    }

    public static float InvertDegree(float degree)
    {
        if (degree < 0) return 360 + degree;
        else return -360 + degree;
    }

    public static (float, float) GetShortestDegrees(float degree1, float degree2)
    {
        float degree1Inv = InvertDegree(degree1);
        float degree2Inv = InvertDegree(degree2);

        float ab12 = DegreeDifference(degree1, degree2);
        float ab1Inv2 = DegreeDifference(degree1Inv, degree2);
        float ab1I2Inv = DegreeDifference(degree1, degree2Inv);

        if (ab12 < ab1Inv2 && ab12 < ab1I2Inv) return (degree1, degree2);
        if (ab1Inv2 < ab12 && ab1Inv2 < ab1I2Inv) return (degree1Inv, degree2);

        return (degree1, degree2Inv);
    }
}