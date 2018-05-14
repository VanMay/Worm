using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionDetector : MonoBehaviour {
    /// <summary>
    /// 通常状态下的目标检测范围
    /// </summary>
    public float detectRange = 20;
    /// <summary>
    /// 冲突状态下该范围内目标锁定不会丢失
    /// </summary>
    public float lockTargetRange = 5;
    public Vector2 angle = new Vector2(120, 60);

    [SerializeField]
    private float rayDensity = 1;

    public AlertLevel alertLevel;

	void Update () {
        if (!alertLevel.alertTarget)
        {
            Detect();
        }
        else
        {
            if (alertLevel.state == AlertState.ConflictingState
                && (alertLevel.transform.position - alertLevel.alertTarget.transform.position).magnitude <= lockTargetRange)
            {
                //冲突状态下lockTargetRange范围内目标锁定不会丢失
            }
            else
            {
                DetectRaycast(detectRange);
            }
        }
	}

    void Detect()
    {
        if (DetectOverlapSphere(detectRange))
        {
            DetectRaycast(detectRange);
        }
    }

    //距离检测
    bool DetectOverlapSphere(float range)
    {
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, range, 1 << LayerMask.NameToLayer(Layers.Character));
        foreach (Collider obj in objectsAround)
        {
            if (BattleManager.instance.IsEnemy(alertLevel.tag, obj.tag))
            {
                Vector3 dir = obj.transform.position - alertLevel.transform.position;
                if (Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(dir, transform.up)) <= angle.x / 2
                    && Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(dir, transform.right)) <= angle.y / 2)
                {
                    return true;
                }
            }
        }
        //alertLevel.SetAlertTarget(null);
        return false;
    }

    //射线检测
    bool DetectRaycast(float range)
    {
        int rayNumX = Mathf.RoundToInt(angle.x * rayDensity);
        int rayNumY = Mathf.RoundToInt(angle.y * rayDensity);
        for (int i = 0; i < rayNumX; i++)
        {
            for(int j = 0; j < rayNumY; j++)
            {
                Vector3 rayDirX = Vector3.Slerp(
                    Quaternion.Euler(0, -angle.x / 2, 0) * transform.forward, 
                    Quaternion.Euler(0, angle.x / 2, 0) * transform.forward, 
                    i * 1.0f / (rayNumX - 1)).normalized;
                Vector3 rayDirY = Vector3.Slerp(
                    Quaternion.Euler(-angle.y / 2, 0, 0) * transform.forward,
                    Quaternion.Euler(angle.y / 2, 0, 0) * transform.forward,
                    j * 1.0f / (rayNumY - 1)).normalized;
                Ray ray = new Ray(transform.position, rayDirX + rayDirY - transform.forward);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, range))
                {
                    if (BattleManager.instance.IsEnemy(alertLevel.tag, hit.transform.tag))
                    {
                        //alertLevel.SetAlertTarget(hit.transform.gameObject);
                        return true;
                    }
                }
            }
        }
        //alertLevel.SetAlertTarget(null);
        return false;
    }

    public void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Mesh mesh = new Mesh();
        int rayNumX = Mathf.RoundToInt(angle.x * 0.1f);
        int rayNumY = Mathf.RoundToInt(angle.y * 0.1f);
        Vector3[] vertices = new Vector3[rayNumX * rayNumY + 1];
        for (int i = 0; i < rayNumX; i++)
        {
            for (int j = 0; j < rayNumY; j++)
            {
                Vector3 rayDirX = Vector3.Slerp(
                    Quaternion.Euler(0, -angle.x / 2, 0) * transform.forward,
                    Quaternion.Euler(0, angle.x / 2, 0) * transform.forward,
                    i * 1.0f / (rayNumX - 1)).normalized;
                Vector3 rayDirY = Vector3.Slerp(
                    Quaternion.Euler(-angle.y / 2, 0, 0) * transform.forward,
                    Quaternion.Euler(angle.y / 2, 0, 0) * transform.forward,
                    j * 1.0f / (rayNumY - 1)).normalized;
                vertices[i * rayNumY + j] = transform.position + (rayDirX + rayDirY - transform.forward).normalized * detectRange;
            }
        }
        vertices[rayNumX * rayNumY] = transform.position;
        int[] triangles = new int[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + (rayNumY - 1) * 6];
        for(int i = 0; i < rayNumX - 1; i++)
        {
            for(int j = 0; j < rayNumY - 1; j++)
            {
                triangles[(i * (rayNumY - 1) + j) * 6] = i * rayNumY + j;
                triangles[(i * (rayNumY - 1) + j) * 6 + 1] = i * rayNumY + j + 1;
                triangles[(i * (rayNumY - 1) + j) * 6 + 2] = (i + 1) * rayNumY + j;
                triangles[(i * (rayNumY - 1) + j) * 6 + 3] = (i + 1) * rayNumY + j;
                triangles[(i * (rayNumY - 1) + j) * 6 + 4] = i * rayNumY + j + 1;
                triangles[(i * (rayNumY - 1) + j) * 6 + 5] = (i + 1) * rayNumY + j + 1;
            }
        }
        for (int i = 0; i < rayNumX - 1; i++)
        {
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6] = rayNumX * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6 + 1] = i * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6 + 2] = (i + 1) * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6 + 3] = rayNumX * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6 + 4] = (i + 1) * rayNumY + rayNumY - 1;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + i * 6 + 5] = i * rayNumY + rayNumY - 1;
        }
        for (int i = 0; i < rayNumY - 1; i++)
        {
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6] = rayNumX * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6 + 1] = i + 1;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6 + 2] = i;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6 + 3] = rayNumX * rayNumY;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6 + 4] = (rayNumX - 1) * rayNumY + i;
            triangles[(rayNumX - 1) * (rayNumY - 1) * 6 + (rayNumX - 1) * 6 + i * 6 + 5] = (rayNumX - 1) * rayNumY + i + 1;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        Gizmos.color = Color.red;
        Gizmos.DrawWireMesh(mesh);
#endif
    }
}
