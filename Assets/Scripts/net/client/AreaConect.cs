using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pomelo.DotNetClient;
using SimpleJson;
using System.Threading;
using UnityEngine.SceneManagement;
using tpgm;
using tpgm.UI;

public class AreaConect : MonoBehaviour {
	#if false
	private static AreaConect instance = null;

	private TaskExecutor mTaskExecutor = null;

	public static AreaConect Instance
	{
		get { return instance; }
	}

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		//注册主线程
		mTaskExecutor = GetComponent<TaskExecutor>();
		if(mTaskExecutor == null)
			mTaskExecutor = gameObject.AddComponent<TaskExecutor>();


		//注册网络事件监听
		InitNetEvent();
		//onMatch();
	}	
		

	//发送准备请求
	public void onPrepare()
	{

		if (ConectData.Instance.pClient != null) {
			JsonObject jsMsg = new JsonObject();
			jsMsg["roomNum"] = "";   //房间号
			jsMsg["type"] = 1;       //1：准备，2：取消准备
			ConectData.Instance.pClient.request ("area.gloryHandler.prepare", jsMsg, (data) => {
				Debug.Log ("onPrepare" + data);
				UDFriend.FriendBuf buf = null;
				/*
				if(null != data)
				{
					buf = JsonConvert.DeserializeObject<UDFriend.FriendBuf>(data.ToString());
					ConectData.Instance.roomNum = buf.roomNum;
					//ConectData.Instance.friedns = new List<UDFriend.Friend>(buf.friendArr);
					//Debug.Log(""+buf.friendArr.Length);
					//ConectData.Instance.Channel = buf.frienaArr =UDfriends;
					//eventObj.onove(buf);
				}*/
			});
		} else {
			Debug.LogError ("pClient null");
		}
	}


	//发送匹配请求
	public void onMatch()
	{
		if (ConectData.Instance.pClient != null) {
			JsonObject jsMsg = new JsonObject ();
			jsMsg ["roomNum"] = ConectData.Instance.roomNum;
			Debug.Log ("" + ConectData.Instance.roomNum);
			ConectData.Instance.pClient.request ("area.gloryHandler.match", jsMsg, (data) => {
				Debug.Log("onMatch" + data);
			});
		} else {
			Debug.LogError ("pClient null");
		}
	}


	//发送加入房间请求
	public void onEnterRoom()
	{
		if (ConectData.Instance.pClient != null) {
			ConectData.Instance.pClient.request ("area.gloryHandler.enterRoom", (data) => {
				//onMatch();
				UDFriend.FriendBuf buf = null;
				Debug.Log("onEnterRoom"+data);
				if(null != data)
				{

					JObject jsMsg = JObject.Parse(data.ToString());
					if(jsMsg.Property("friendArr")!=null)
					{
						buf = JsonConvert.DeserializeObject<UDFriend.FriendBuf>(data.ToString());
						ConectData.Instance.roomNum = buf.roomNum;
						ConectData.Instance.friedns = new List<UDFriend.Friend>(buf.friendArr);
					}
					else
					{
						Debug.Log("null friends");
						object roomNum = new object();
						object code = new object();
						object utcMs = new object();
						if (data.TryGetValue("roomNum", out roomNum) &&
							data.TryGetValue("code", out code) &&
							data.TryGetValue("utcMs", out utcMs))
						{
							Debug.Log(""+roomNum.ToString());
							ConectData.Instance.roomNum = roomNum.ToString();
						}
					}
				}
			});
		} else {
			Debug.LogError ("pClient null");
		}


	}





	//注册网络事件
	void InitNetEvent()
	{
		PomeloClient pClient = ConectData.Instance.pClient;
		if (pClient != null)
		{
			

			pClient.on("add", (data) =>
				{
					Debug.Log("InitNetEvent add" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("leave", (data) =>
				{
					Debug.Log("leave" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("ready", (data) =>
				{
					Debug.Log("ready" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("invite", (data) =>
				{
					Debug.Log("invite" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("onChat", (data) =>
				{
					Debug.Log("onChat" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("match", (data) =>
				{
					Debug.Log("match" +data);
					//onChatAdd(data);
					//Debug.Log("message"+data);
				});
			pClient.on("gloryAdd", (data) => {
				Debug.Log("InitNetEvent gloryAdd" + data);
				onGloryAdd(data);
			});

			pClient.on("roomMember", (data) => {
				//Debug.Log("InitNetEvent roomAdd" + data);
				//onUserLeave(data);
				ClientMgr.Instance().onRecv(RecvType.roomMember ,data);
			});

			pClient.on("playerInfo", (data) => {
				//Debug.Log("InitNetEvent roomAdd" + data);
				ClientMgr.Instance().onRecv(RecvType.playerInfo ,data);
				//onUserLeave(data);
			});


			pClient.on("magicStage", (data) => {
				ClientMgr.Instance().onRecv(RecvType.magicStage ,data);
			});


			pClient.on("first", (data) => {
				ClientMgr.Instance().onRecv(RecvType.first ,data);
			});

			pClient.on("attackInfo", (data) => {
				ClientMgr.Instance().onRecv(RecvType.attackInfo ,data);
			});

			pClient.on("attackNum", (data) => {
				ClientMgr.Instance().onRecv(RecvType.attackNum ,data);
			});

			pClient.on("dead", (data) => {
				ClientMgr.Instance().onRecv(RecvType.dead ,data);
			});

			pClient.on("sceneInfo", (data) => {
				ClientMgr.Instance().onRecv(RecvType.sceneInfo ,data);
			});

			pClient.on("attackData", (data) => {
				ClientMgr.Instance().onRecv(RecvType.attackData ,data);
			});
		}
	}

	private void onPrepareAdd(JsonObject jsMsg)
	{

		//mTaskExecutor.ScheduleTask(new Task(new Action<FrameBuf>(eventObj.onMove), buf));
		//eventObj.onove(buf);
		UDGroup.PreparePlayerBuf buf = null;
		buf = JsonConvert.DeserializeObject<UDGroup.PreparePlayerBuf>(jsMsg.ToString());
		Debug.Log ("" + buf.message [0].group);
		Debug.Log (""+buf.message.Length);

	}

	private void onAreaLeave(JsonObject jsMsg)
	{

	}
	private void onAreaReady(JsonObject jsMsg)
	{

	}

	private void onAreaInvite(JsonObject jsMsg)
	{

	}

	private void onAreaChat(JsonObject jsMsg)
	{

	}

	//收到要开始的玩家数据，填充到准备页面
	private void onGloryAdd(JsonObject jsMsg)
	{
		UDGroup.StartPlayerBuf buf = null;
		buf = JsonConvert.DeserializeObject<UDGroup.StartPlayerBuf>(jsMsg.ToString());
		ConectData.Instance.start_groups = new List<UDGroup.Group>(buf.newUser);
	}

	#endif
}

