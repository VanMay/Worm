using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memory<T> {
    public int Count
    {
        get
        {
            return memoryQueue.Count;
        }
    }
    [SerializeField]
    private List<MemoryUnit> memoryQueue;
    [System.Serializable]
    public class MemoryUnit
    {
        public T content;
        public float memorizeTime;
        public float duration;

        public MemoryUnit(T content, float duration)
        {
            this.content = content;
            this.memorizeTime = Time.time;
            this.duration = duration;
        }
    }

    public Memory()
    {
        memoryQueue = new List<MemoryUnit>();
    }

    /// <summary>
    /// 记住指定物品
    /// </summary>
    /// <param name="content">记忆物品</param>
    /// <param name="duration">记忆有效时间</param>
    public void Memorize(T content, float duration)
    {
        bool hasMemory = false;
        for (int i = memoryQueue.Count - 1; i >= 0; i--)
        {
            if (Time.time > memoryQueue[i].memorizeTime + memoryQueue[i].duration)
            {//超出记忆时间则将其遗忘
                memoryQueue.RemoveAt(i);
            }
            else if (memoryQueue[i].content.Equals(content))
            {//未超出记忆时间 且 内容匹配
                memoryQueue[i].memorizeTime = Time.time;
                memoryQueue[i].duration = duration;
                hasMemory = true;
            }
        }
        if (!hasMemory)
        {
            memoryQueue.Add(new MemoryUnit(content, duration));
        }
    }

    /// <summary>
    /// 查询记忆
    /// </summary>
    /// <param name="content">记忆物品</param>
    /// <returns></returns>
    public bool Contains(T content)
    {
        bool hasMemory = false;
        for(int i = memoryQueue.Count - 1; i >= 0; i--)
        {
            if (Time.time > memoryQueue[i].memorizeTime + memoryQueue[i].duration)
            {//超出记忆时间则将其遗忘
                memoryQueue.RemoveAt(i);
            }
            else if (memoryQueue[i].content.Equals(content))
            {//未超出记忆时间 且 内容匹配
                hasMemory = true;
            }
        }
        return hasMemory;
    }

    /// <summary>
    /// 对于指定物品保持记忆新鲜度
    /// </summary>
    /// <param name="content">记忆物品数组</param>
    /// <param name="duration">记忆有效时间数组</param>
    public void KeepInMind(T[] content, float[] duration)
    {
        if(content.Length != duration.Length)
        {
            Debug.LogError("[Memory]: Content Length != Duration Length");
        }
        bool[] hasMemory = new bool[content.Length];
        for (int i = memoryQueue.Count - 1; i >= 0; i--)
        {
            if (Time.time > memoryQueue[i].memorizeTime + memoryQueue[i].duration)
            {//超出记忆时间则将其遗忘
                memoryQueue.RemoveAt(i);
            }
            else if (memoryQueue[i].content.Equals(content))
            {//未超出记忆时间 且 内容匹配
                memoryQueue[i].memorizeTime = Time.time;
                memoryQueue[i].duration = duration[i];
                hasMemory[i] = true;
            }
        }
        for(int i = 0; i < hasMemory.Length; i++)
        {
            if (!hasMemory[i])
            {
                memoryQueue.Add(new MemoryUnit(content[i], duration[i]));
            }
        }
    }
}
