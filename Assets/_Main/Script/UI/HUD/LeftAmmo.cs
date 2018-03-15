using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftAmmo : MonoBehaviour {
    [SerializeField]
    private Text text;

    public void ShowLeftAmmo(int leftAmmo)
    {
        text.text = leftAmmo.ToString();
    }
}
