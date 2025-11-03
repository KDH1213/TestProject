using UnityEngine;

public static class Utills
{

    public static float HaversineDistance(Vector2 p1, Vector2 p2)
    {
        const float R = 6371e3f; // 지구 반지름(m)
        float dLat = Mathf.Deg2Rad * (p2.x - p1.x);
        float dLon = Mathf.Deg2Rad * (p2.y - p1.y);
        float lat1 = Mathf.Deg2Rad * p1.x;
        float lat2 = Mathf.Deg2Rad * p2.x;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
                  Mathf.Cos(lat1) * Mathf.Cos(lat2) *
                  Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        return R * c; // meter 단위
    }
}