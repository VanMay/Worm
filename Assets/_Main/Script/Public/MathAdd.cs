using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathAdd {
    /// <summary>
    /// XZ平面上范围为(-180, 180]的角度
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static float Angle_XZ_180(Vector3 from, Vector3 to)
    {
        if (from == Vector3.zero || to == Vector3.zero)
        {
            return 0;
        }
        float result = 0;
        Vector3 temp = Vector3.Cross(from, to);
        if (temp.y > 0)
        {
            result = Vector3.Angle(from, to);
        }
        else
        {
            result = -Vector3.Angle(from, to);
        }
        return result;
    }
}
