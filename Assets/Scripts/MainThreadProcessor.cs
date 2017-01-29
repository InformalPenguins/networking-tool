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
        //TODO: Delegate to a GameLogic entity
        if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " "))
        {
            text = text.Substring(NetworkConstants.ACTION_UPDATE_POSITION.ToString().Length + 1);
            this.LoadPositions(text);
        }
        else if (text.StartsWith(NetworkConstants.INPUT_POSITION))
        {
            text = text.Substring(NetworkConstants.INPUT_POSITION.Length);
            this.MoveToLocation(text);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN))
        {
            text = text.Substring(NetworkConstants.ACTION_SERVER_LOGIN.ToString().Length);
            this.IdentifyUser(text);
        }

        yield return null;
    }

    private void MoveToLocation(string text)
    {
        throw new NotImplementedException();
    }
    private void LoadPositions(string text)
    {
        throw new NotImplementedException();
    }
    private void IdentifyUser(string text)
    {
        throw new NotImplementedException();
    }
}
