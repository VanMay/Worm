using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

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

    void Start () {
        GetWork();
        anim = GetComponent<Animator>();
        anim.SetFloat("AnimCycleOffset", Random.value);
        behaviorTree = GetComponent<BehaviorTree>();
    }

	void Update () {
        float speed = Mathf.Max(GetComponent<UnityEngine.AI.NavMeshAgent>().velocity.magnitude, 1f);
        anim.SetFloat("MoveSpeed", speed);
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
}
