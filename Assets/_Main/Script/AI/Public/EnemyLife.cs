using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class EnemyLife : Life {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override IEnumerator Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GetComponent<BehaviorTree>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Worker>().enabled = false;
        GetComponent<Cyber>().enabled = false;
        GetComponent<Collider>().enabled = false;
        yield return 0;
    }
}
