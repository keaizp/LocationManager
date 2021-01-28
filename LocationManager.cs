
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

            mGeoFenceClient.Call("addGeoFence", "丽景家园", "住宅", centerPoint, 1000F, 10, "打卡");
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
                txtLocation.text = "成功定位获取数据:";

                try
                {
                    txtLocation.text = txtLocation.text + "\r\n>>定位结果来源:" + amapLocation.Call<int>("getLocationType").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>纬度:" + amapLocation.Call<double>("getLatitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>经度:" + amapLocation.Call<double>("getLongitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>精度信息:" + amapLocation.Call<float>("getAccuracy").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>地址:" + amapLocation.Call<string>("getAddress").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>国家:" + amapLocation.Call<string>("getCountry").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>省:" + amapLocation.Call<string>("getProvince").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>城市:" + amapLocation.Call<string>("getCity").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>城区:" + amapLocation.Call<string>("getDistrict").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>街道:" + amapLocation.Call<string>("getStreet").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>门牌:" + amapLocation.Call<string>("getStreetNum").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>城市编码:" + amapLocation.Call<string>("getCityCode").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>地区编码:" + amapLocation.Call<string>("getAdCode").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>海拔:" + amapLocation.Call<double>("getAltitude").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>方向角:" + amapLocation.Call<float>("getBearing").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>定位信息描述:" + amapLocation.Call<string>("getLocationDetail").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>兴趣点:" + amapLocation.Call<string>("getPoiName").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>提供者:" + amapLocation.Call<string>("getProvider").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>卫星数量:" + amapLocation.Call<int>("getSatellites").ToString();
                    txtLocation.text = txtLocation.text + "\r\n>>当前速度:" + amapLocation.Call<float>("getSpeed").ToString();

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
        {//判断围栏是否创建成功
            txt3.text = "添加围栏成功!!";
            //geoFenceList是已经添加的围栏列表，可据此查看创建的围栏
        }
        else
        {
            txt3.text = "添加围栏失败!!";
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
