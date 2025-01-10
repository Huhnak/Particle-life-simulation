using UnityEngine;

public static class Helper
{
    public static float Particle(float x, float t)
    {
        const float beta = 0.3f;
        if (x < beta)
            return x / beta - 1;
        if (beta < x && x < 1)
            return t * (1 - Mathf.Abs(2 * x - 1 - beta) / (1 - beta));
        return 0;
    }
}
