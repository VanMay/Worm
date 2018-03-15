using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : MonoBehaviour {
    public bool PlatformLow;
    public bool PlatformHigh;
    public bool Roadblock;

	void Start () {
		
	}

	void Update () {
        PlatformLow = false;
        PlatformHigh = false;
        Roadblock = false;

        float detectLength = 0.3f;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * detectLength, Color.red);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, detectLength, 1 << 8))
        {
            if (Vector3.Angle(-hit.normal, transform.forward) < 45)
            {
                PlatformLow = hit.transform.tag == Tags.PlatformLow;
                PlatformHigh = hit.transform.tag == Tags.PlatformHigh;
                Roadblock = hit.transform.tag == Tags.Roadblock;
            }
        }
	}
}
