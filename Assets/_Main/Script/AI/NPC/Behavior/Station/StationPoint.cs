using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationPoint : MonoBehaviour {
    public GameObject[] stationLocationList;
    private List<GameObject> npcList;
    private List<bool> npcArriveLocationList;
    private List<int> emptyIndexList;
    private List<int> occupiedIndexList;
    public bool HasEmpty
    {
        get
        {
            return emptyIndexList.Count > 0;
        }
    }

    [SerializeField]
    private float stationTime = 10;
    [SerializeField]
    private List<string> animParameterList;

	void Awake () {
        npcList = new List<GameObject>();
        npcArriveLocationList = new List<bool>();
        emptyIndexList = new List<int>();
        occupiedIndexList = new List<int>();
        for(int i = 0; i < stationLocationList.Length; i++)
        {
            npcList.Add(null);
            npcArriveLocationList.Add(false);
            emptyIndexList.Add(i);
        }
	}

    /// <summary>
    /// 进入驻点
    /// </summary>
    /// <param name="npc"></param>
    /// <returns>驻点中指定位置</returns>
    public GameObject EnterStation(GameObject npc)
    {
        if(npcList.Contains(null))
        {//人未满
            if (AnimatorMatched(npc.GetComponent<Animator>()))
            {
                int index = emptyIndexList[Random.Range(0, emptyIndexList.Count)];
                npcList[index] = npc;
                emptyIndexList.Remove(index);
                occupiedIndexList.Add(index);
                return stationLocationList[index];
            }
        }
        return null;
    }

    private bool AnimatorMatched(Animator anim)
    {
        if(anim == null)
        {
            return false;
        }
        //获得Animator中所有Bool参数的名称
        List<string> tempAnimParameter = new List<string>();
        for (int i = 0; i < anim.parameters.Length; i++)
        {
            if(anim.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                tempAnimParameter.Add(anim.parameters[i].name);
            }
        }
        //比较是否符合条件
        for (int i = 0; i < animParameterList.Count; i++)
        {
            if (animParameterList[i] != "" && !tempAnimParameter.Contains(animParameterList[i]))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 离开驻点
    /// </summary>
    /// <param name="npc"></param>
    public void ExitStation(GameObject npc)
    {
        int index = npcList.IndexOf(npc);
        npcList[index] = null;
        npcArriveLocationList[index] = false;
        emptyIndexList.Add(index);
        occupiedIndexList.Remove(index);
    }

    /// <summary>
    /// 到达驻点
    /// </summary>
    /// <param name="npc"></param>
    public void ArriveStation(GameObject npc)
    {
        int index = npcList.IndexOf(npc);
        npcArriveLocationList[index] = true;
        SendAnimParameter(npc);//更新归属于此StationPoint的NPC的动画指令
    }

    private void SendAnimParameter(GameObject npc)
    {
        string animParameter = animParameterList[Mathf.Min(occupiedIndexList.Count - 1, animParameterList.Count - 1)];
        if (animParameter == "")
        {//无动画指令，向刚加入的NPC发送指令
            Station station = npc.GetComponent<Station>();
            station.stationAnimParameter = animParameter;
            station.stationStartTime = Time.time;
            station.stationTime = stationTime;
        }
        else
        {//有动画指令时，向归属于此StationPoint的所有NPC发送指令
            for (int i = 0; i < occupiedIndexList.Count; i++)
            {
                Station station = npcList[occupiedIndexList[i]].GetComponent<Station>();
                if (station.stationAnimParameter != animParameter)
                {//重复动画指令不进行更新
                    station.stationAnimParameter = animParameter;
                    station.stationStartTime = Time.time;
                    station.stationTime = stationTime;
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.yellow;
        for (int i = 0; i < stationLocationList.Length; i++)
        {
            Gizmos.DrawWireSphere(stationLocationList[i].transform.position, 0.25f);
            Gizmos.DrawLine(stationLocationList[i].transform.position, stationLocationList[i].transform.position + Vector3.up * 2);
        }
#endif
    }
}
