//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by Fernflower decompiler)
//

package com.unity3d.player;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

public class UABroadcastReceiver extends BroadcastReceiver {
    public static BroadcastReceiverInterface receiver;

    public UABroadcastReceiver() {
    }

    public void onReceive(Context context, Intent intent) {
        Log.d("Unity", "Android has receive broadcast");
        if (receiver != null) {
            receiver.onReceive(context, intent);
        } else {
            Log.e("Unity", "BroadcastReceiverInterface receiver is null");
        }

    }

    public static void setReceiver(BroadcastReceiverInterface unityReceiverProxy) {
        receiver = unityReceiverProxy;
    }
}
