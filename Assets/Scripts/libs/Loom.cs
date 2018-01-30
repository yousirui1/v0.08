using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class Loom : MonoBehaviour
{
    public const int MAX_THREADS = 8;
    static int m_numThreads;

    private static Loom s_current;
    private int _count;
    public static Loom Current
    {
        get
        {
            Initialize();
            return s_current;
        }
    }

    void Awake()
    {
        Debug.Log("Loom Awake");

        s_current = this;
        s_initialized = true;

        MonoBehaviour.DontDestroyOnLoad(this); //#否则OnDestroy不会被调用;
    }

    void OnDestroy()
    {
        Debug.Log("Loom OnDestroy");

        s_current = null;
        s_initialized = false;
    }

    static bool s_initialized;

    static void Initialize()
    {
        if (!s_initialized)
        {

            if (!Application.isPlaying)
                return;
            s_initialized = true;
            var g = new GameObject("Loom");
            s_current = g.AddComponent<Loom>();
        }

    }

    private List<Action> m_todoThisFrame = new List<Action>();
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }


    private List<DelayedQueueItem> m_delayedTodo = new List<DelayedQueueItem>();

    List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f);
    }
    public static void QueueOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            lock (Current.m_delayedTodo)
            {
                Current.m_delayedTodo.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else
        {
            lock (Current.m_todoThisFrame)
            {
                Current.m_todoThisFrame.Add(action);
            }
        }
    }

    public static Thread RunAsync(Action a)
    {
        Initialize();
        while (m_numThreads >= MAX_THREADS)
        {
            Thread.Sleep(1);
        }
        Interlocked.Increment(ref m_numThreads);
        ThreadPool.QueueUserWorkItem(RunAction, a);
        return null;
    }

    private static void RunAction(object action)
    {
        try
        {
            ((Action)action)();
        }
        catch
        {
        }
        finally
        {
            Interlocked.Decrement(ref m_numThreads);
        }

    }


    void OnDisable()
    {
        if (s_current == this)
        {
            s_current = null;
        }
    }



    // Use this for initialization  
    void Start()
    {
    }

    List<Action> m_tmpDoing = new List<Action>();

    // Update is called once per frame  
    void Update()
    {
        lock (m_todoThisFrame)
        {
            m_tmpDoing.Clear();
            m_tmpDoing.AddRange(m_todoThisFrame);
            m_todoThisFrame.Clear();
        }
        foreach (var a in m_tmpDoing)
        {
            a();
        }

        lock (m_delayedTodo)
        {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(m_delayedTodo.Where(d => d.time <= Time.time)); //#delay时间到的;
            foreach (var item in _currentDelayed)
                m_delayedTodo.Remove(item);
        }
        foreach (var delayed in _currentDelayed)
        {
            delayed.action();
        }
    }
}


