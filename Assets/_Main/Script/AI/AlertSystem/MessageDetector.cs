using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDetector : MonoBehaviour {
    public float sendRange = 10;
    public AlertLevel alertLevel;

	void Start () {
		
	}

	void Update () {
		
	}

    public void GetAlarm(GameObject target, float level)
    {
        if (alertLevel.alertTarget == null)
        {
            alertLevel.alertTarget = target;
            alertLevel.alertLevel = level;
        }
    }

    public void SendAlarm(GameObject target)
    {
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, sendRange);
        foreach (Collider obj in objectsAround)
        {
            if (BattleManager.instance.IsAlly(alertLevel.tag, obj.tag))
            {
                MessageDetector messageDetector = obj.GetComponentInChildren<MessageDetector>();
                if (messageDetector)
                {
                    messageDetector.GetAlarm(target, 1);
                }
            }
        }
    }
}
