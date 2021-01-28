
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
    public Text tvReult;
    public Text txtLocation;
    public Text txt3;
    public static string GEOFENCE_BROADCAST_ACTION = "com.location.apis.geofencedemo.broadcast";
    private ChangeJavaToC amap;
    private GFListener geofenceLis;
    private AndroidJavaClass jcu;
    private AndroidJavaObject jou;
    private AndroidJavaObject mLocationClient;
    private AndroidJavaObject mLocationOption;
    private AndroidJavaObject mGeoFenceClient;
    private AndroidJavaObject centerPoint;
    private AndroidJavaObject geofence;

    private UnityBroadcastProxy unlockBroadcast;
    private AndroidJavaObject BroadcastReceiver;
    private AndroidJavaClass Intent;
    private AndroidJavaClass Bundle;
    private AndroidJavaObject filter;
    private AndroidJavaObject ConnectivityManager;

    private void Start()
    {
        
    }
    public void StartLocation()
    {
        try
        {
            jcu = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jou = jcu.GetStatic<AndroidJavaObject>("currentActivity");
            mLocationClient = new AndroidJavaObject("com.amap.api.location.AMapLocationClient", jou);
            mLocationOption = new AndroidJavaObject("com.amap.api.location.AMapLocationClientOption");
            
            
            mLocationClient.Call("setLocationOption", mLocationOption);
            amap = new ChangeJavaToC();
            amap.locationChanged += OnLocationChanged;

            mLocationClient.Call("setLocationListener", amap);
           // 

            mLocationClient.Call("startLocation");
          

        }
        catch (Exception ex)
        {

            txtLocation.text = ex.Message;

            EndLocation();
        }
    }
    public void StartGeoFence()
    {
        try
        {
            jcu = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            jou = jcu.GetStatic<AndroidJavaObject>("currentActivity");
            mGeoFenceClient = new AndroidJavaObject("com.amap.api.fence.GeoFenceClient", jou);
            centerPoint = new AndroidJavaObject("com.amap.api.location.DPoint");
            geofence = new AndroidJavaObject("com.amap.api.fence.GeoFence");
            Intent = new AndroidJavaClass("android.content.Intent");
            Bundle = new AndroidJavaClass("android.os.Bundle");
            ConnectivityManager = new AndroidJavaClass("android.net.ConnectivityManager");
            geofenceLis = new GFListener();
            geofenceLis.GeoFenceCreateFinished += onGeoFenceCreateFinished;
            mGeoFenceClient.Call("setGeoFenceListener", geofenceLis);
            mGeoFenceClient.Call("setActivateAction", mGeoFenceClient.GetStatic<int>("GEOFENCE_IN") | mGeoFenceClient.GetStatic<int>("GEOFENCE_OUT") | mGeoFenceClient.GetStatic<int>("GEOFENCE_STAYED"));

            centerPoint.Call("setLatitude", 25.962D);
            centerPoint.Call("setLongitude", 115.405D);

            mGeoFenceClient.Call("addGeoFence", "������԰", "סլ", centerPoint, 1000F, 10, "��");
            mGeoFenceClient.Call<AndroidJavaObject>("createPendingIntent", GEOFENCE_BROADCAST_ACTION);
   
            BroadcastReceiver = new AndroidJavaObject("com.unity3d.player.UABroadcastReceiver");
            filter = new AndroidJavaObject("android.content.IntentFilter", ConnectivityManager.GetStatic<string>("CONNECTIVITY_ACTION"));
            filter.Call("addAction", GEOFENCE_BROADCAST_ACTION);
            jou.Call<AndroidJavaObject>("registerReceiver", BroadcastReceiver, filter);
            unlockBroadcast = new UnityBroadcastProxy();
            BroadcastReceiver.CallStatic("setReceiver", unlockBroadcast);
        }
        catch (Exception ex)
        {
            tvReult.text = ex.Message;
        }
        
    }

    public void EndLocation()
    {
        if (amap != null)
        {
            amap.locationChanged -= OnLocationChanged;
        }

        if (mLocationClient != null)
        {
            mLocationClient.Call("stopLocation");
            mLocationClient.Call("onDestroy");
        }

        txtLocation.text = "";
    }

    private void OnLocationChanged(AndroidJavaObject amapLocation)
    {
        if (amapLocation != null)
        {
            if (amapLocation.Call<int>("getErrorCode") == 0)
            {
                txtLocation.text = "�ɹ���λ��ȡ����:";

                try
                {
                    txtLocation.text = txtLocation.text + "\r\n>>��λ�����Դ:" + amapLocation.Call<int>("getLocationType").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>γ��:" + amapLocation.Call<double>("getLatitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<double>("getLongitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>������Ϣ:" + amapLocation.Call<float>("getAccuracy").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��ַ:" + amapLocation.Call<string>("getAddress").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<string>("getCountry").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>ʡ:" + amapLocation.Call<string>("getProvince").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<string>("getCity").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<string>("getDistrict").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>�ֵ�:" + amapLocation.Call<string>("getStreet").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<string>("getStreetNum").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>���б���:" + amapLocation.Call<string>("getCityCode").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��������:" + amapLocation.Call<string>("getAdCode").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>����:" + amapLocation.Call<double>("getAltitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>�����:" + amapLocation.Call<float>("getBearing").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��λ��Ϣ����:" + amapLocation.Call<string>("getLocationDetail").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��Ȥ��:" + amapLocation.Call<string>("getPoiName").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>�ṩ��:" + amapLocation.Call<string>("getProvider").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��������:" + amapLocation.Call<int>("getSatellites").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>��ǰ�ٶ�:" + amapLocation.Call<float>("getSpeed").ToString();

                }
                catch (Exception ex)
                {
                    txtLocation.text = txtLocation.text + "\r\n--------------ex-------------:";
                    txtLocation.text = txtLocation.text + "\r\n" + ex.Message;
                }

            }
            else
            {
                txtLocation.text = ">>amaperror:";
                txtLocation.text = txtLocation.text + ">>getErrorCode:" + amapLocation.Call<int>("getErrorCode").ToString();
                txtLocation.text = txtLocation.text + ">>getErrorInfo:" + amapLocation.Call<string>("getErrorInfo");
            }
        }
        else
        {
            txtLocation.text = "amaplocation is null.";
        }
    }

    public void onGeoFenceCreateFinished(AndroidJavaObject geoFenceList,
            int errorCode,string customID)
    {
        if (errorCode == 0)
        {//�ж�Χ���Ƿ񴴽��ɹ�
            txt3.text = "���Χ���ɹ�!!";
            //geoFenceList���Ѿ���ӵ�Χ���б��ɾݴ˲鿴������Χ��
        }
        else
        {
            txt3.text = "���Χ��ʧ��!!";
        }
    }

    public void onBroadcastReceive()
    {
        mTask item;
        if (unlockBroadcast == null)
        {
            return;
        }
        if(!unlockBroadcast.taskQueue.TryPeek(out item))
        {
            Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
        }
        else if (item != null)
        {
            Console.WriteLine("CQ: Expected TryPeek result of 0, got {0}", item);
        }
       
    }
    private void Update()
    {
        onBroadcastReceive();
    }
}
