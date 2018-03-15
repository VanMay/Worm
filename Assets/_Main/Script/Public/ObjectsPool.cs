using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour {
    [SerializeField]
    private GameObject objPrefab;
    [SerializeField]
    private int initNum;

    private int leftNum;
    private List<GameObject> pool;

	void Start () {
        pool = new List<GameObject>();
        for (int i = 0; i < initNum; i++)
        {
            GameObject obj = Instantiate(objPrefab, transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.SetActive(false);
            pool.Add(obj);
        }
        leftNum = initNum;
    }

    /// <summary>
    /// 从对象池取用
    /// </summary>
    /// <returns></returns>
    public GameObject Get()
    {
        //对象池未空
        if (leftNum > 0)
        {
            for(int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    leftNum--;
                    return pool[i];
                }
            }
            return null;
        }
        // 对象池已空
        else
        {
            //扩展对象池
            GameObject obj = Instantiate(objPrefab, transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localEulerAngles = Vector3.zero;
            pool.Add(obj);
            return obj;
        }
    }

    /// <summary>
    /// 一定时间后归还到对象池
    /// </summary>
    /// <param name="obj"></param>
    public void Return(GameObject obj)
    {
        if (leftNum < pool.Count)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i].activeInHierarchy)
                {
                    if (pool[i] == obj)
                    {
                        pool[i].transform.localPosition = Vector3.zero;
                        pool[i].transform.localEulerAngles = Vector3.zero;
                        pool[i].SetActive(false);
                        leftNum++;
                        break;
                    }
                }
            }
        }
    }
}
