using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    [SerializeField]
    private GameObject ElevatorBox;
    [SerializeField]
    private List<float> floorHeight;
    [SerializeField]
    private float speed = 1;
    private int currentFloor = 0;
    private int nextFloor = 0;

    private List<GameObject> objectsInElevator;

    private Animator anim;

	void Start () {
        objectsInElevator = new List<GameObject>();
        anim = GetComponent<Animator>();
        StartCoroutine(PlayAnim());
	}

	void Update () {
		if(currentFloor == nextFloor)
        {
            nextFloor = (currentFloor + 1) % floorHeight.Count;
        }
	}

    IEnumerator PlayAnim()
    {
        while (true)
        {
            if (nextFloor != currentFloor)
            {
                //关门
                while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
                {
                    anim.SetBool("CloseDoor", true);
                    yield return 0;
                }
                anim.SetBool("CloseDoor", false);
                //移动
                float timer = 0;
                float duration = Mathf.Abs(floorHeight[currentFloor] - floorHeight[nextFloor]) / speed;
                while (timer <= duration)
                {
                    Vector3 pos = ElevatorBox.transform.position;
                    float nextPosY = Mathf.SmoothStep(floorHeight[currentFloor], floorHeight[nextFloor], Mathf.Clamp01(timer / duration));
                    float deltaY = nextPosY - pos.y;
                    for(int i = 0; i < objectsInElevator.Count; i++)
                    {
                        objectsInElevator[i].transform.position += Vector3.up * deltaY;
                    }
                    pos.y = nextPosY;
                    ElevatorBox.transform.position = pos;
                    timer += Time.deltaTime;
                    yield return 0;
                }
                currentFloor = nextFloor;
                //开门
                while (anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
                {
                    anim.SetBool("OpenDoor", true);
                    yield return 0;
                }
                anim.SetBool("OpenDoor", false);
                yield return new WaitForSeconds(5);
            }
            else
            {
                yield return 0;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(Layers.Character))
        {
            objectsInElevator.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.Character))
        {
            objectsInElevator.Remove(other.gameObject);
        }
    }
}
