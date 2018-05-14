using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : Life {
    [SerializeField]
    private Vector3 StartPoint;

    void Start () {
		
	}

	void Update () {
		
	}

    public override IEnumerator Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        for (int i = 0; i < BattleManager.instance.playerEnemyList.Count; i++)
        {
            BattleManager.instance.playerEnemyList[i].GetComponent<Cognition>().memory.ForgetAll();
            BattleManager.instance.playerEnemyList[i].SetAlertTarget(null);
            BattleManager.instance.playerEnemyList[i].alertLevel = 0;
        }
        BattleManager.instance.playerEnemyList.Clear();
        yield return new WaitForSeconds(5);
        transform.position = StartPoint;
        GetComponent<PlayerInfo>().Load();
        GetComponent<Animator>().SetTrigger("Reset");
        GetComponent<Collider>().enabled = true;
        Reset();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.Attack)
        {
            float damage = other.GetComponent<Attack>().damage;
            GetDamage(damage);
        }
    }
}
