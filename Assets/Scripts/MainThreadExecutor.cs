using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadExecutor : MonoBehaviour
{
    private static MainThreadExecutor instance;

    private static ConcurrentQueue<Action> actionQueue = new ConcurrentQueue<Action>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        while (!actionQueue.IsEmpty)
        {
            if (actionQueue.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }
    }

    public static void ExecuteOnMainThread(Action action)
    {
        if (instance == null)
        {
            Debug.LogError("MainThreadExecutor instance is not initialized.");
            return;
        }

        // Enqueue the action to be executed on the main thread
        actionQueue.Enqueue(action);
    }

}
