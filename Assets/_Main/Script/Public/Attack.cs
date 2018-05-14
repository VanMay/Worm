using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public GameObject self;
    public float damage;

    private Collider collider;
    private AlertLevel alertLevel;

    void Start()
    {
        collider = GetComponent<Collider>();
        alertLevel = self.GetComponent<AlertLevel>();
    }

    void Update()
    {
        if(alertLevel.state == AlertState.ConflictingState)
        {
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }
}
