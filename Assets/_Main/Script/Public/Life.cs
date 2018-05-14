using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Life : MonoBehaviour {
    public float HP = 100;
    public float MaxHP = 100;
    private bool isDying = false;

    public void GetDamage(float amount)
    {
        HP = HP - amount;
        if(HP <= 0)
        {
            HP = 0;
            if (!isDying)
            {
                isDying = true;
                StartCoroutine(Die());
            }
        }
    }

    public void Reset()
    {
        HP = MaxHP;
        isDying = false;
    }

    public abstract IEnumerator Die();
}
