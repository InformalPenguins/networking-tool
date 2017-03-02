using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Class that enqueues tasks to be executed in the main thread.
 * 
 * Credits: https://github.com/PimDeWitte/UnityMainThreadDispatcher/blob/master/UnityMainThreadDispatcher.cs
 * */
public class MainThreadProcessor : MonoBehaviour
{
    //This is the entity that must handle UI changes (if any) from the received message.
    private IUIHandler uiHandler;

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadProcessor _instance = null;

    public static MainThreadProcessor Instance()
    {
        if (!Exists())
        {
            throw new Exception("MainThreadProcessor could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }

    public static bool Exists()
    {
        return _instance != null;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            uiHandler = GetComponent<IUIHandler>();
            if (uiHandler == null) {
                Application.Quit();
                throw new Exception("IUIHandler has not been added to your Client GameObject.");
            }
            //inputManager = GetComponent<InputManager>();
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }
    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }

    public IEnumerator processMessage(string text)
    {
        if (uiHandler != null) {
            uiHandler.processMessage (text);
        }
        yield return null;
    }
}
