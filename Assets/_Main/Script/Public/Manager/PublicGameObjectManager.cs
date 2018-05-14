using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicGameObjectManager : MonoBehaviour {
    public static PublicGameObjectManager instance;
    public GameObject Player;
    public GameObject FortArea;

    void Awake()
    {
        instance = this;
    }
}
