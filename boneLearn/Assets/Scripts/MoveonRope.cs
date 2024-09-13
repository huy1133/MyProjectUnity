using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveonRope : MonoBehaviour
{
    public Transform[] points;
    public GameObject a;
    [Range(0, 1)]
    public float percentageRanged01; // Điều chỉnh vị trí dọc theo sợi dây

    private float percentage;

    void Update()
    {
        int total = points.Length;
        //percentage = (((float)total - 1) / total) * percentageRanged01;

        // Di chuyển hình vuông dọc theo các điểm điều khiển
        for (int i = 0; i < total - 1; i++)
        {
            if (percentageRanged01 >= (float)i / total)
            {
                // Tính toán vị trí và góc của hình vuông giữa các điểm điều khiển
                float lerpFactor = (percentageRanged01 - ((float)i / total)) * total;
                a.transform.position = Vector2.Lerp(points[i].position, points[i + 1].position, lerpFactor);
                Vector3 vectorOffset = Vector3.forward * -270;
                a.transform.eulerAngles = Vector3.Lerp(points[i].eulerAngles, points[i + 1].transform.eulerAngles, lerpFactor)+vectorOffset;
            }
        }
    }
}
