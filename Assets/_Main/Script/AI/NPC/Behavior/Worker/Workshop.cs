using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workshop : MonoBehaviour {
    public GameObject[] workLocationList;
    private List<GameObject> npcList;
    private List<int> emptyIndexList;//未分配给NPC的工作地点
    private List<int> markedIndexList;//已分配给NPC的工作地点，但NPC未在该地点工作
    private List<int> occupiedIndexList;//已分配给NPC的工作地点，且NPC正在该地点工作

    void Awake () {
        npcList = new List<GameObject>();
        emptyIndexList = new List<int>();
        markedIndexList = new List<int>();
        occupiedIndexList = new List<int>();
        for(int i = 0; i < workLocationList.Length; i++)
        {
            npcList.Add(null);
            emptyIndexList.Add(i);
        }
	}

	void Update () {
		
	}

    /// <summary>
    /// 获得工作
    /// </summary>
    /// <param name="npc"></param>
    /// <returns>返回Work Location, 如果全满返回null</returns>
    public GameObject GetWork(GameObject npc)
    {
        if (emptyIndexList.Count == 0)
        {
            return null;
        }

        int index = emptyIndexList[Random.Range(0, emptyIndexList.Count)];
        npcList[index] = npc;
        emptyIndexList.Remove(index);
        markedIndexList.Add(index);
        return workLocationList[index];
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    /// <param name="npc"></param>
    public void StartWork(GameObject npc)
    {
        int index = npcList.IndexOf(npc);
        markedIndexList.Remove(index);
        occupiedIndexList.Add(index);
    }

    /// <summary>
    /// 结束工作
    /// </summary>
    /// <param name="npc"></param>
    public void EndWork(GameObject npc)
    {
        int index = npcList.IndexOf(npc);
        npcList[index] = null;
        occupiedIndexList.Remove(index);
        emptyIndexList.Add(index);
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.blue;
        for (int i = 0; i < workLocationList.Length; i++)
        {
            Gizmos.DrawWireSphere(workLocationList[i].transform.position, 0.25f);
            Gizmos.DrawLine(workLocationList[i].transform.position, workLocationList[i].transform.position + Vector3.up * 2);
        }
#endif
    }
}
