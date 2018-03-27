using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditoryDetector : MonoBehaviour {
    public AlertLevel alertLevel;

    void Start () {
		
	}
	
	void Update () {
		
	}

    public void GetAuditory(Vector3 sourcePos)
    {
        alertLevel.hasCheckTarget = true;
        alertLevel.checkPosition = sourcePos;
        alertLevel.alertLevel += 0.5f;
    }
}
