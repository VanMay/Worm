using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLife : Life {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override IEnumerator Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        yield return 0;
    }
}
