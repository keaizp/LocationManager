using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Concurrent;

public class UnityBroadcastProxy : AndroidJavaProxy
{

    public ConcurrentQueue<mTask> taskQueue = new ConcurrentQueue<mTask>();
    public UnityBroadcastProxy() : base("com.unity3d.player.BroadcastReceiverInterface") { }

    /// <summary>
    /// public void onReceive(Context context, Intent intent)
    /// </summary>
    /// <param name="context">Context.</param>
    /// <param name="intent">Intent.</param>
    public void onReceive(AndroidJavaObject context, AndroidJavaObject intent)
    {
        mTask item = new mTask { Context=context,Intent=intent};
        taskQueue.Enqueue(item);
    }

}