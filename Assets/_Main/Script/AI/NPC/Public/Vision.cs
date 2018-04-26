using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Vision : MonoBehaviour {
    //视觉检测结果
    public List<GameObject> ObjectsInVision;//视域内物体列表
    public List<GameObject> EnemyList;
    //视觉检测参数
    public float visionRange = 15;
    [SerializeField, Range(0, 180)]
    private float visionAngle_Horizontal = 120;
    [SerializeField, Range(0, 180)]
    private float visionAngle_Vertical = 60;
    [SerializeField]
    private float raycastDensity = 1;
    //视觉检测接受者
    public GameObject self;
    //视域参数
    private Vector3[] middle;//中轴线点
    private Vector3[] corner;//角点

    void Start()
    {
        ObjectsInVision = new List<GameObject>();
        EnemyList = new List<GameObject>();
        CalcFrustum(out middle, out corner);
    }

    //计算视域参考点
    void CalcFrustum(out Vector3[] middle, out Vector3[] corner)
    {
        Vector2 visionAngle = new Vector2(visionAngle_Horizontal / 2, visionAngle_Vertical / 2);
        middle = new Vector3[]{
            (Quaternion.Euler(0, visionAngle.x, 0) * Vector3.forward).normalized * visionRange,
            (Quaternion.Euler(0, -visionAngle.x, 0) * Vector3.forward).normalized * visionRange,
            (Quaternion.Euler(visionAngle.y, 0, 0) * Vector3.forward).normalized * visionRange,
            (Quaternion.Euler(-visionAngle.y, 0, 0) * Vector3.forward).normalized * visionRange,
        };
        Vector3[] normal = {
            Vector3.Cross(middle[0], Vector3.up),
            Vector3.Cross(middle[1], Vector3.up),
            Vector3.Cross(middle[2], Vector3.right),
            Vector3.Cross(middle[3], Vector3.right)
        };
        corner = new Vector3[]{
            GetCornerDir(normal[0], normal[2]) * visionRange,
            GetCornerDir(normal[0], normal[3]) * visionRange,
            GetCornerDir(normal[1], normal[2]) * visionRange,
            GetCornerDir(normal[1], normal[3]) * visionRange
        };
    }

    //计算视域角点
    Vector3 GetCornerDir(Vector3 normal1, Vector3 normal2)
    {
        return new Vector3(
            normal1.z * normal2.y - normal1.y * normal2.z,
            normal1.x * normal2.z - normal1.z * normal2.x,
            normal1.y * normal2.x - normal1.x * normal2.y
        ).normalized;
    }

    void Update()
    {
        Detect();
    }

    //更新视觉内物体列表
    void Detect()
    {
        ObjectsInVision.Clear();
        EnemyList.Clear();
        bool inVision = false;
        //OverlapSphere检测视觉内物体
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, visionRange, ~(1 << LayerMask.NameToLayer(Layers.Default)));
        for (int i = 0; i < objectsAround.Length; i++)
        {
            if (objectsAround[i].gameObject != self)
            {//排除自身
                inVision = true;
                break;
            }
        }
        //Raycast检测视觉内物体
        if (inVision)
        {
            Ray ray = new Ray();
            ray.origin = transform.position;
            RaycastHit hit = new RaycastHit();
            int rayNumH = (int)(visionAngle_Horizontal * raycastDensity);
            for (int i = 0; i <= rayNumH; i++)
            {
                Vector3 topDir = Vector3.Slerp(transform.rotation * corner[0], transform.rotation * corner[2], i * 1f / rayNumH);
                Vector3 bottomDir = Vector3.Slerp(transform.rotation * corner[1], transform.rotation * corner[3], i * 1f / rayNumH);
                float tempVisionAngle_Vertical = Vector3.Angle(topDir, bottomDir);
                int rayNumV = (int)(tempVisionAngle_Vertical * raycastDensity);
                for (int j = 0; j <= rayNumV; j++)
                {
                    Vector3 rayDir = Vector3.Slerp(topDir, bottomDir, j * 1f / rayNumV);
                    ray.direction = rayDir;
                    if (Physics.Raycast(ray, out hit, visionRange))
                    {
                        if (hit.transform.gameObject.layer != LayerMask.NameToLayer(Layers.Default))
                        {
                            if (!ObjectsInVision.Contains(hit.transform.gameObject))
                            {
                                ObjectsInVision.Add(hit.transform.gameObject);
                                if(BattleManager.instance.IsEnemy(self.tag, hit.transform.tag))
                                {
                                    EnemyList.Add(hit.transform.gameObject);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        CalcFrustum(out middle, out corner);
        //中轴线绘制
        for (int i = 0; i < middle.Length; i++)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.rotation * middle[i]);
            Gizmos.DrawLine(transform.position, transform.position + transform.rotation * middle[i]);
        }
        //角线绘制
        for (int i = 0; i < corner.Length; i++)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.rotation * corner[i]);
        }
        //圆弧绘制
        float density = 1;
        int numX = (int)(density * visionAngle_Horizontal);
        Vector3[] lastDir = {
            transform.rotation * corner[0],
            transform.rotation * middle[0],
            transform.rotation * corner[2],
            transform.rotation * corner[0],
            transform.rotation * middle[2],
            transform.rotation * corner[1]
        };
        for (int i = 0; i <= numX; i++)
        {
            Vector3 currentDir = transform.rotation * Vector3.Slerp(corner[0], corner[1], i * 1f / numX);
            Gizmos.DrawLine(transform.position + lastDir[0], transform.position + currentDir);
            lastDir[0] = currentDir;

            currentDir = transform.rotation * Vector3.Slerp(middle[0], middle[1], i * 1f / numX);
            Gizmos.DrawLine(transform.position + lastDir[1], transform.position + currentDir);
            lastDir[1] = currentDir;

            currentDir = transform.rotation * Vector3.Slerp(corner[2], corner[3], i * 1f / numX);
            Gizmos.DrawLine(transform.position + lastDir[2], transform.position + currentDir);
            lastDir[2] = currentDir;
        }
        int numY = (int)(density * visionAngle_Vertical);
        for (int i = 0; i <= numY; i++)
        {
            Vector3 currentDir = transform.rotation * Vector3.Slerp(corner[0], corner[2], i * 1f / numY);
            Gizmos.DrawLine(transform.position + lastDir[3], transform.position + currentDir);
            lastDir[3] = currentDir;

            currentDir = transform.rotation * Vector3.Slerp(middle[2], middle[3], i * 1f / numY);
            Gizmos.DrawLine(transform.position + lastDir[4], transform.position + currentDir);
            lastDir[4] = currentDir;

            currentDir = transform.rotation * Vector3.Slerp(corner[1], corner[3], i * 1f / numY);
            Gizmos.DrawLine(transform.position + lastDir[5], transform.position + currentDir);
            lastDir[5] = currentDir;
        }
#endif
    }
}
