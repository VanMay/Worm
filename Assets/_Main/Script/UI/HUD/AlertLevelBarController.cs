using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertLevelBarController : MonoBehaviour {
    private ObjectsPool alertLevelBarPool;
    private List<GameObject> barList;

	void Start () {
        alertLevelBarPool = GetComponent<ObjectsPool>();
        barList = new List<GameObject>();
	}

	void Update () {
        if (barList.Count < BattleManager.instance.playerEnemyList.Count)
        {
            for (int i = barList.Count; i < BattleManager.instance.playerEnemyList.Count; i++)
            {
                GameObject bar = alertLevelBarPool.Get();
                barList.Add(bar);
            }
        }
        else if (barList.Count > BattleManager.instance.playerEnemyList.Count)
        {
            for (int i = barList.Count - 1; i >= BattleManager.instance.playerEnemyList.Count; i--)
            {
                alertLevelBarPool.Return(barList[i]);
                barList.RemoveAt(i);
            }
        }

        for (int i = 0; i < barList.Count; i++)
        {
            Vector3 enemyDir = BattleManager.instance.playerEnemyList[i].transform.position - PublicGameObjectManager.instance.Player.transform.position;
            Vector3 enemyDirOnXZ = Vector3.ProjectOnPlane(enemyDir, Vector3.up).normalized;
            Vector3 viewDir = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            Vector3 localEnemyDirOnXZ = Quaternion.FromToRotation(viewDir, Vector3.forward) * enemyDirOnXZ;
            Vector3 screenDir = new Vector3(localEnemyDirOnXZ.x, localEnemyDirOnXZ.z, 0);
            barList[i].GetComponent<RectTransform>().up = screenDir;
            barList[i].GetComponentInChildren<Image>().material.SetFloat("_Value", BattleManager.instance.playerEnemyList[i].alertLevel);
        }
	}
}
