using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Worker : MonoBehaviour {
    [Header("State")]
    public GameObject workLocation;
    public Workshop workshop;
    public bool isWorking;
    [Header("Work Flow")]
    public List<Workshop> WorkFlow;
    public int currentWorkIndex;

    private Animator anim;
    private BehaviorTree behaviorTree;
    private NavMeshAgent agent;

    void Start () {
        GetWork();
        anim = GetComponent<Animator>();
        anim.SetFloat("AnimCycleOffset", Random.value);
        behaviorTree = GetComponent<BehaviorTree>();
        agent = GetComponent<NavMeshAgent>();
    }

	void Update () {
        float speed = Mathf.Clamp01(agent.velocity.magnitude / agent.speed);
        anim.SetFloat("Speed", speed);
        SendValueToBehaviorTree();
    }

    public void GetWork()
    {
        if (workshop != null)
        {
            currentWorkIndex = (currentWorkIndex + 1) % WorkFlow.Count;
        }
        workshop = WorkFlow[currentWorkIndex];
        workLocation = workshop.GetWork(gameObject);
        if (workLocation == null)
        {
            GetWork();
        }
    }

    public void StartWork()
    {
        workshop.StartWork(gameObject);
        isWorking = true;
    }

    public void EndWork()
    {
        workshop.EndWork(gameObject);
        isWorking = false;
    }

    void SendValueToBehaviorTree()
    {
        behaviorTree.SetVariableValue("WorkLocation", workLocation);
    }

    public void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.red;
        for (int i = 0; i < WorkFlow.Count; i++)
        {
            Gizmos.DrawWireSphere(WorkFlow[i].transform.position, 0.25f);
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(WorkFlow[i].transform.position, WorkFlow[(i + 1) % WorkFlow.Count].transform.position, NavMesh.AllAreas, path);
            for (int j = 0; j < path.corners.Length - 1; j++)
            {
                Gizmos.DrawLine(path.corners[j], path.corners[(j + 1)]);
            }
        }
#endif
    }
}
