using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPoint : MonoBehaviour {
    public List<WalkPoint> connectedWalkPoints;

	void Start () {
		
	}

	void Update () {
		
	}

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        if (connectedWalkPoints != null)
        {
            for (int i = 0; i < connectedWalkPoints.Count; i++)
            {
                Gizmos.DrawLine(transform.position, connectedWalkPoints[i].transform.position);
                Vector3 dir = transform.position - connectedWalkPoints[i].transform.position;
                Vector3 localRight = Vector3.Cross(dir, Vector3.up);
                Vector3 rightPoint = connectedWalkPoints[i].transform.position + (dir.normalized + localRight.normalized / 2) * 2;
                Vector3 leftPoint = connectedWalkPoints[i].transform.position + (dir.normalized - localRight.normalized / 2) * 2;
                Gizmos.DrawLine(connectedWalkPoints[i].transform.position, rightPoint);
                Gizmos.DrawLine(connectedWalkPoints[i].transform.position, leftPoint);
                Gizmos.DrawLine(leftPoint, rightPoint);
            }
        }
#endif
    }
}
