using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour {
    public GameObject PlatformLow;
    public GameObject PlatformHigh;
    public GameObject Roadblock;

    public Vector3 hitNormal;

	void Start () {
		
	}

	void Update () {
        PlatformLow = null;
        PlatformHigh = null;
        Roadblock = null;
        hitNormal = Vector3.zero;

        float detectLength = 0.3f;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, detectLength, LayerMask.GetMask(Layers.Interaction)))
        {
            if (Vector3.Angle(-hit.normal, transform.forward) < 45)
            {
                PlatformLow = hit.transform.tag == Tags.PlatformLow ? hit.collider.gameObject : null;
                PlatformHigh = hit.transform.tag == Tags.PlatformHigh ? hit.collider.gameObject : null;
                Roadblock = hit.transform.tag == Tags.Roadblock ? hit.collider.gameObject : null;
                hitNormal = hit.normal;
            }
        }
	}
}
