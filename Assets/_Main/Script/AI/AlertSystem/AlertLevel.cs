using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public enum AlertState
{
    NormalState,
    WatchState,
    AlertState,
    ConflictingState
}

public class AlertLevel : MonoBehaviour {
    [Header("Alert Level Parameter")]
    [SerializeField]
    private float alertStateThreshold = 0.5f;
    [SerializeField]
    private float conflictingStateThreshold = 1;
    [SerializeField]
    private AnimationCurve increaseSpeedCurve;
    [SerializeField]
    private float decreaseSpeed = 0.25f;

    [Header("Alert Attribute")]
    [Range(0, 1)]
    public float alertLevel;
    public AlertState state;
    public GameObject alertTarget;
    public bool hasCheckTarget = false;
    public Vector3 checkPosition;

    public VisionDetector visionDetector;
    public MessageDetector messageDetector;

    private BehaviorTree behaviorTree;

	void Start () {
        visionDetector.alertLevel = this;
        behaviorTree = GetComponent<BehaviorTree>();
	}

	void Update () {
        UpdateAlertLevel();
        UpdateState();
        SendValueToBT();
	}

    void UpdateAlertLevel()
    {
        if (alertTarget)
        {
            float distance = (alertTarget.transform.position - transform.position).magnitude / visionDetector.detectRange;
            float increaseSpeed = increaseSpeedCurve.Evaluate(distance);
            alertLevel += increaseSpeed * Time.deltaTime;
            hasCheckTarget = true;
            checkPosition = alertTarget.transform.position;
        }
        else
        {
            if (!hasCheckTarget)
            {
                alertLevel -= decreaseSpeed * Time.deltaTime;
            }
        }
        alertLevel = Mathf.Clamp01(alertLevel);
    }

    void UpdateState()
    {
        if(alertLevel == 0)
        {
            state = AlertState.NormalState;
            if (BattleManager.instance.playerEnemyList.Contains(this))
            {
                BattleManager.instance.playerEnemyList.Remove(this);
            }
        }
        else if (alertLevel < alertStateThreshold)
        {
            state = AlertState.WatchState;
        }
        else if (alertLevel < conflictingStateThreshold)
        {
            state = AlertState.AlertState;
        }
        else
        {
            state = AlertState.ConflictingState;
            if (alertTarget)
            {
                messageDetector.SendAlarm(alertTarget);
            }
        }
    }

    void SendValueToBT()
    {
        behaviorTree.SetVariableValue("AlertTarget", alertTarget);
        behaviorTree.SetVariableValue("CheckPosition", checkPosition);
    }

    public void SetAlertTarget(GameObject target)
    {
        alertTarget = target;
        if (alertTarget)
        {
            if (alertTarget.tag == Tags.Player)
            {
                if (!BattleManager.instance.playerEnemyList.Contains(this))
                {
                    BattleManager.instance.playerEnemyList.Add(this);
                }
            }
        }
    }
}
