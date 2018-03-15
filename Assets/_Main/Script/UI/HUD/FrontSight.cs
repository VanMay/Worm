using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrontSight : MonoBehaviour {
    [SerializeField]
    private Image center;
    [SerializeField]
    private Image[] Axis = new Image[4];

    private Vector3[] axisPos;

	void Start () {
        axisPos = new Vector3[Axis.Length];
        for(int i = 0; i < axisPos.Length; i++)
        {
            axisPos[i] = Axis[i].GetComponent<RectTransform>().localPosition;
        }
	}

    public void ShowBallisticDiffusion(Weapon weapon)
    {
        Vector3 worldSpacePoint =
            Camera.main.transform.position +
            (Camera.main.transform.forward + Camera.main.transform.up * weapon.CurrentBallisticDiffusion) * weapon.range;
        Vector3 screenSpacePoint = Camera.main.WorldToScreenPoint(worldSpacePoint);
        screenSpacePoint.z = 0;
        float distance = (screenSpacePoint - new Vector3(Screen.width / 2, Screen.height / 2, 0)).magnitude;
        Axis[0].GetComponent<RectTransform>().localPosition = axisPos[0] + Vector3.up * distance;
        Axis[1].GetComponent<RectTransform>().localPosition = axisPos[1] + Vector3.left * distance;
        Axis[2].GetComponent<RectTransform>().localPosition = axisPos[2] + Vector3.down * distance;
        Axis[3].GetComponent<RectTransform>().localPosition = axisPos[3] + Vector3.right * distance;
    }
}
