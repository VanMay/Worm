using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerGenerator : MonoBehaviour {
    public int num = 5;
    public GameObject prefab;
    public List<WalkPoint> walkPointList;

    void Start () {
		for(int i = 0; i < num; i++)
        {
            GameObject npc = Instantiate(prefab, transform.position, transform.rotation);
            Walker walker = npc.GetComponent<Walker>();
            walker.walkerGenerator = this;
            walker.Init();
        }
	}

	void Update () {
		
	}
}
