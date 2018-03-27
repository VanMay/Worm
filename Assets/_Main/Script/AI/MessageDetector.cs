using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDetector : MonoBehaviour {
    public float sendMessageRange = 5;

    public AlertLevel alertLevel;

	void Start () {
		
	}

	void Update () {
		
	}

    public void GetAlarm(GameObject target)
    {
        alertLevel.alertTarget = target;
        alertLevel.alertLevel = 1;
    }

    public void SendAlarm(GameObject target)
    {
        Collider[] objectsAround = Physics.OverlapSphere(transform.position, sendMessageRange);
        foreach (Collider obj in objectsAround)
        {
            if (BattleManager.instance.IsAlly(alertLevel.tag, obj.tag))
            {
                MessageDetector messageDetector = obj.GetComponentInChildren<MessageDetector>();
                if (messageDetector)
                {
                    messageDetector.GetAlarm(target);
                }
            }
        }
    }
}
