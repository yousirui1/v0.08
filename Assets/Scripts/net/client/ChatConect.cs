using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Pomelo.DotNetClient;
using SimpleJson;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft;
using tpgm;

/**************************************
*FileName: ChatConect.cs
*User: ysr 
*Data: 2017/12/19
*Describe: 连接Chat服务器和数据处理
**************************************/

public class ChatConect : MonoBehaviour
{
#region Define
    private static ChatConect instance = null;

    //用户表
    private List<string> UserNameList = new List<string>();


	EventManage eventObj ;
   
    

    //JoyControl joy = null;
    private TaskExecutor mTaskExecutor = null;

    //单列
    public static ChatConect Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		instance = this;
	}

  
    // Use this for initialization
    void Start () 
	{
        //线程池不能find物体
        //ThreadPool.QueueUserWorkItem(InitNetEvent);
        mTaskExecutor = GetComponent<TaskExecutor>();
        if (mTaskExecutor == null)
            mTaskExecutor = gameObject.AddComponent<TaskExecutor>();

		eventObj = GameObject.Find("EventSystem").GetComponent<EventManage>() as EventManage;
			
        //注册网络事件用于接收
        InitNetEvent();

        //创建用户表
        ConectData.Instance.UserNameList = UserNameList;

        onJoin();

    }

    float timer = 0.1f;
    // Update is called once per frame
    void Update()
    {
        /*timer -= Time.deltaTime;
        if (timer <= 0)
        {
            onChat(eventObj.onInput());
            Debug.Log(string.Format("Timer1 is up !!! time=${0}", Time.time));
            timer = 0.1f;
        }*/
    }
    #endregion


    #region  Send

    //加入房间请求
    void onJoin()
    {
		if (null != SavedContext.s_client)
        {
			SavedContext.s_client.request("chat.roomHandler.join", (data) =>
            {

                Debug.Log("onJoin" + data);
               
            });
        }
        else
        {
            //Debug.LogError("client error");
        }
    }

   

    //定时检测输入发送数据
    void FixedUpdate()
    {
       onChat(eventObj.onInput());

    }


    void onChat(PlayerVal entite)
    {
        if (null != entite)
        {
            JsonObject message = new JsonObject();
            message["uid"] = entite.uid;
            message["x"] = entite.x;
            message["y"] = entite.y;
            message["d"] = entite.d;
            message["v"] = entite.v;
            message["skill"] = entite.skill;
            message["hp"] = entite.hp;
            message["score"] = entite.score;
			SavedContext.s_client.request("chat.roomHandler.move", message, (data) =>
            {
               
            });
            entite.skill = 0;
        }
        else
        {

        }
    }

    //程序退出时通知服务器
    void OnApplicationQuit()
    {
        if (null != ConectData.Instance.pClient)
        {
			SavedContext.s_client.request("chat.roomHandler.leave", (data) =>
            {
                Debug.Log("OnApplicationQuit" + data);
            });
			SavedContext.s_client.disconnect();
        }
        else
        {
            //Debug.LogError("client error");
        }
    }

    #endregion

    #region Recv

    //注册网络事件
    void InitNetEvent()
    {

        Debug.Log("InitNetEvent");

		PomeloClient pClient = SavedContext.s_client;

        if (pClient != null)
        {
            pClient.on("add", (data) => {
                Debug.Log("InitNetEvent onAdd"+data);
                onUserAdd(data);

            });
 
            pClient.on("onLeave", (data) => {
                Debug.Log("InitNetEvent onLeave" + data);
                onUserLeave(data);
            });

            pClient.on("message", (data) =>
            {
                //Debug.Log("InitNetEvent message" +data);
                onChatAdd(data);
            });
        }
    }


    void onChatAdd(JsonObject jsonObj)
    {
     	FrameBuf buf = null;
	   	if(null != jsonObj)
		{
			buf = JsonConvert.DeserializeObject<FrameBuf>(jsonObj.ToString());
            mTaskExecutor.ScheduleTask(new Task(new Action<FrameBuf>(eventObj.onMove), buf));
            //eventObj.onove(buf);
		}	
    }


    void onUserAdd(JsonObject data)
	{
     	Debug.Log("onUserAdd"+data);	 
	}
   


	void onUserLeave(JsonObject data)
	{
		System.Object user = null;
		if(data.TryGetValue("user", out user))
		{
			string userName =user.ToString();
            int count = 0;
            foreach (string name in ConectData.Instance.UserNameList)
            {
                if(name == userName)
                {
                    break;
                }
                count++;
            }
            ConectData.Instance.UserNameList.Remove(userName);
            //eventObj.delPlayer(count);

        }	
	}


    #endregion
}
