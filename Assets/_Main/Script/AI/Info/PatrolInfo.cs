using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolInfo : MonoBehaviour {
    [SerializeField]
    private bool pingpong;
    [SerializeField]
    private Vector4[] patrolRoute;

    private int index = 0;
    private bool pong; //是否返回

    void Start()
    {

    }

    public Vector4 GetPatrolInfo()
    {
        return patrolRoute[index];
    }

    public void Next()
    {
        if (pingpong)
        {
            if (pong)
            {
                if (index == 0)
                {
                    pong = false;
                }
            }
            else
            {
                if (index == patrolRoute.Length - 1)
                {
                    pong = true;
                }
            }
            index = pong ? (index + patrolRoute.Length - 1) % patrolRoute.Length : (index + 1) % patrolRoute.Length;
        }
        else
        {
            index = (index + 1) % patrolRoute.Length;
        }
    }

    public void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.green;
        for(int i = 0; i < patrolRoute.Length - 1; i++)
        {
            Vector3 pos = new Vector3(patrolRoute[i].x, patrolRoute[i].y, patrolRoute[i].z);
            Vector3 nextPos = new Vector3(patrolRoute[i + 1].x, patrolRoute[i + 1].y, patrolRoute[i + 1].z);
            Gizmos.DrawWireSphere(pos, 0.5f);
            Gizmos.DrawLine(pos, nextPos);
        }
        if (pingpong)
        {
            Vector3 pos = new Vector3(patrolRoute[patrolRoute.Length - 1].x, patrolRoute[patrolRoute.Length - 1].y, patrolRoute[patrolRoute.Length - 1].z);
            Gizmos.DrawWireSphere(pos, 0.5f);
        }
        else
        {
            Vector3 pos = new Vector3(patrolRoute[patrolRoute.Length - 1].x, patrolRoute[patrolRoute.Length - 1].y, patrolRoute[patrolRoute.Length - 1].z);
            Vector3 nextPos = new Vector3(patrolRoute[0].x, patrolRoute[0].y, patrolRoute[0].z);
            Gizmos.DrawWireSphere(pos, 0.5f);
            Gizmos.DrawLine(pos, nextPos);
        }
#endif
    }
}
