using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    private Vector3 velocity;
    private Vector3 lastPos;

    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private AlertLevel alertLevel;
	
	void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        alertLevel = GetComponent<AlertLevel>();

        lastPos = transform.position;
	}
	
	void Update () {
        LogicLayerUpdate();
        ViewLayerUpdate();
    }

    void LogicLayerUpdate()
    {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    void ViewLayerUpdate()
    {
        anim.SetFloat("AlertLevel", alertLevel.alertLevel);
        float speed = velocity.magnitude / navMeshAgent.speed;
        anim.SetFloat("Speed", speed);
    }
}
