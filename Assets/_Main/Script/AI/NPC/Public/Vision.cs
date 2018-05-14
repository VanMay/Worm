using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Vision : MonoBehaviour {
    //视觉检测结果
    public List<GameObject> ObjectsInVision;//视域内物体列表
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
    private List<Vector4> boundsAngleRangeList;

    public bool detectEnable = true;

    void Start()
    {
        ObjectsInVision = new List<GameObject>();
        boundsAngleRangeList = new List<Vector4>();
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
        if (detectEnable)
        {
            Detect();
        }
        else
        {
            if (ObjectsInVision.Count > 0)
            {
                ObjectsInVision.Clear();
            }
        }
    }

    //更新视觉内物体列表
    void Detect()
    {
        ObjectsInVision.Clear();
        bool inVision = false;
        //OverlapSphere检测视觉内物体
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, visionRange, LayerMask.GetMask(Layers.Character));
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
            RaycastHit hit = new RaycastHit();
            int rayNumH = (int)(visionAngle_Horizontal * raycastDensity);
            int raycastLayerMask = LayerMask.GetMask(Layers.Character, Layers.Default);
            int defaultLayer = LayerMask.NameToLayer(Layers.Default);
            for (int i = 0; i <= rayNumH; i++)
            {
                Vector3 topDir = Vector3.Slerp(transform.rotation * corner[0], transform.rotation * corner[2], i * 1f / rayNumH);
                Vector3 bottomDir = Vector3.Slerp(transform.rotation * corner[1], transform.rotation * corner[3], i * 1f / rayNumH);
                float tempVisionAngle_Vertical = Vector3.Angle(topDir, bottomDir);
                int rayNumV = (int)(tempVisionAngle_Vertical * raycastDensity);
                for (int j = 0; j <= rayNumV; j++)
                {
                    Vector3 rayDir = Vector3.Slerp(topDir, bottomDir, j * 1f / rayNumV);
                    if (Physics.Raycast(transform.position, rayDir, out hit, visionRange, raycastLayerMask))
                    {
                        GameObject hitObject = hit.collider.gameObject;
                        if (hitObject.layer != defaultLayer)
                        {
                            if (!ObjectsInVision.Contains(hitObject))
                            {
                                ObjectsInVision.Add(hitObject);
                            }
                        }
                    }
                }
            }
            ////计算视域内所有包围体在视角中范围
            //boundsAngleRangeList.Clear();
            //for (int i = 0; i < objectsAround.Length; i++)
            //{
            //    if(objectsAround[i].gameObject != self)
            //    {
            //        //计算包围体
            //        Vector3 boundsCenterWorldPos = objectsAround[i].bounds.center;
            //        Vector3 boundsExtents = objectsAround[i].bounds.extents;
            //        Vector3[] objectsBoundsCornerWorldPos = {
            //                boundsCenterWorldPos + (Vector3.right * boundsExtents.x + Vector3.up * boundsExtents.y + Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (-Vector3.right * boundsExtents.x + Vector3.up * boundsExtents.y + Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (-Vector3.right * boundsExtents.x - Vector3.up * boundsExtents.y + Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (Vector3.right * boundsExtents.x - Vector3.up * boundsExtents.y + Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (Vector3.right * boundsExtents.x + Vector3.up * boundsExtents.y - Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (-Vector3.right * boundsExtents.x + Vector3.up * boundsExtents.y - Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (-Vector3.right * boundsExtents.x - Vector3.up * boundsExtents.y - Vector3.forward * boundsExtents.z),
            //                boundsCenterWorldPos + (Vector3.right * boundsExtents.x - Vector3.up * boundsExtents.y - Vector3.forward * boundsExtents.z)
            //            };
            //        //计算包围体在视角中范围
            //        Vector4 angleRange = new Vector4(
            //            float.MaxValue,
            //            float.MinValue,
            //            float.MaxValue,
            //            float.MinValue
            //        );
            //        for (int j = 0; j < objectsBoundsCornerWorldPos.Length; j++)
            //        {
            //            Vector2 angle = RayToProjectedAngle(objectsBoundsCornerWorldPos[j] - transform.position);
            //            if (angle.x < angleRange.x)
            //            {
            //                angleRange.x = angle.x;
            //            }
            //            if (angle.x > angleRange.y)
            //            {
            //                angleRange.y = angle.x;
            //            }
            //            if (angle.y < angleRange.z)
            //            {
            //                angleRange.z = angle.y;
            //            }
            //            if (angle.y > angleRange.w)
            //            {
            //                angleRange.w = angle.y;
            //            }
            //        }
            //        if (angleRange.x > visionAngle_Horizontal / 2
            //            || angleRange.y < -visionAngle_Horizontal / 2
            //            || angleRange.z > visionAngle_Vertical / 2
            //            || angleRange.w < -visionAngle_Vertical / 2)
            //        {
            //            continue;
            //        }
            //        angleRange.x = Mathf.Max(angleRange.x, -visionAngle_Horizontal / 2);
            //        angleRange.y = Mathf.Min(angleRange.y, visionAngle_Horizontal / 2);
            //        angleRange.z = Mathf.Max(angleRange.z, -visionAngle_Vertical / 2);
            //        angleRange.w = Mathf.Min(angleRange.w, visionAngle_Vertical / 2);
            //        boundsAngleRangeList.Add(angleRange);
            //    }
            //}
            ////Raycast检测
            //Ray ray = new Ray();
            //ray.origin = transform.position;
            //RaycastHit hit = new RaycastHit();
            //int rayNumH = (int)(visionAngle_Horizontal * raycastDensity);
            //for (int i = 0; i <= rayNumH; i++)
            //{
            //    Vector3 topDir = Vector3.Slerp(transform.rotation * corner[0], transform.rotation * corner[2], i * 1f / rayNumH);
            //    Vector3 bottomDir = Vector3.Slerp(transform.rotation * corner[1], transform.rotation * corner[3], i * 1f / rayNumH);
            //    float tempVisionAngle_Vertical = Vector3.Angle(topDir, bottomDir);
            //    int rayNumV = (int)(tempVisionAngle_Vertical * raycastDensity);
            //    for (int j = 0; j <= rayNumV; j++)
            //    {
            //        Vector3 rayDir = Vector3.Slerp(topDir, bottomDir, j * 1f / rayNumV);
            //        Vector2 angle = RayToProjectedAngle(rayDir);
            //        for (int k = 0; k < boundsAngleRangeList.Count; k++)
            //        {
            //            if (angle.x > boundsAngleRangeList[k].x
            //                && angle.x < boundsAngleRangeList[k].y
            //                && angle.y > boundsAngleRangeList[k].z
            //                && angle.y < boundsAngleRangeList[k].w)
            //            {
            //                ray.direction = rayDir;
            //                if (Physics.Raycast(ray, out hit, visionRange, LayerMask.GetMask(Layers.Character, Layers.Default)))
            //                {
            //                    if (hit.transform.gameObject.layer != LayerMask.NameToLayer(Layers.Default))
            //                    {
            //                        if (!ObjectsInVision.Contains(hit.transform.gameObject))
            //                        {
            //                            ObjectsInVision.Add(hit.transform.gameObject);
            //                        }
            //                    }
            //                }
            //                break;
            //            }
            //        }
            //    }
            //}
        }
    }

    Vector2 RayToProjectedAngle(Vector3 rayDir)
    {
        Vector3[] tempProjectedDir = {
            Vector3.ProjectOnPlane(rayDir, transform.up),
            Vector3.ProjectOnPlane(rayDir, transform.right)
        };
        return new Vector2(
            (Vector3.Dot(Vector3.Cross(tempProjectedDir[0], transform.forward), transform.up) > 0 ? 1 : -1) * Vector3.Angle(tempProjectedDir[0], transform.forward),
            (Vector3.Dot(Vector3.Cross(tempProjectedDir[1], transform.forward), transform.right) > 0 ? -1 : 1) * Vector3.Angle(tempProjectedDir[1], transform.forward)
        );
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
