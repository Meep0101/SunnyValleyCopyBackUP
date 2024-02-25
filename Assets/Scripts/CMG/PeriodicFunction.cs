using System;
using UnityEngine;

public class PeriodicFunction
{
    private Action action;
    private float interval;
    private bool isRunning;

    public PeriodicFunction(Action action, float interval)
    {
        this.action = action;
        this.interval = interval;
    }

    public void Start()
    {
        if (!isRunning)
        {
            isRunning = true;
            MonoBehaviourHelper.Instance.StartCoroutine(PeriodicExecution());
        }
    }

    public void Stop()
    {
        isRunning = false;
    }

    private System.Collections.IEnumerator PeriodicExecution()
    {
        while (isRunning)
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }
}

public class MonoBehaviourHelper : MonoBehaviour
{
    private static MonoBehaviourHelper instance;
    public static MonoBehaviourHelper Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MonoBehaviourHelper");
                instance = go.AddComponent<MonoBehaviourHelper>();
            }
            return instance;
        }
    }
}
