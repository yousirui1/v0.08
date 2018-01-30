using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;
using Pomelo.DotNetClient;
using SimpleJson;

public class ChatUIPage : UIPage
{

	private const string TAG = "ChatUIPage";

    public ChatUIPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        //布局预制体
        uiPath = "Prefab/UI/ChatUI/ChatUIMainPage";

    }

    public override void Awake(GameObject go)
    {
        //Init(UIValue.friends_btnID, UIValue.friends_inputID, UIValue.friends_txID, UIValue.friends_imgID);
        
    }

	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	class Controller 
	{

		PomeloClient m_pClient;
		private MainLooper m_initedLooper;

		public Controller()
		{

		}

		public void onDestroy()
		{

			//可能在login页面没有登录
			if (null != SavedContext.s_client) {
				SavedContext.s_client.NetWorkStateChangedEvent -= (state) => {
					Debug.Log (""+state);
				};
			}

		}


		//发送加入房间请求
		public void onEnterRoom()
		{
			if (SavedContext.s_client != null) {
				SavedContext.s_client.request ("area.gloryHandler.enterRoom", (data) => {
					//onMatch();
					UDFriend.FriendBuf buf = null;
					Debug.Log("onEnterRoom"+data);
					if(null != data)
					{

						/*JObject jsMsg = JObject.Parse(data.ToString());
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
						}*/
					}
				});
			} else {
				Debug.LogError ("pClient null");
			}


		}

		//发送准备请求
		public void onPrepare()
		{

			if (SavedContext.s_client != null) {
				JsonObject jsMsg = new JsonObject();
				jsMsg["roomNum"] = "";   //房间号
				jsMsg["type"] = 1;       //1：准备，2：取消准备
				SavedContext.s_client.request ("area.gloryHandler.prepare", jsMsg, (data) => {
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
			if (SavedContext.s_client != null) {
				JsonObject jsMsg = new JsonObject ();
				jsMsg ["roomNum"] = ConectData.Instance.roomNum;
				SavedContext.s_client.request ("area.gloryHandler.match", jsMsg, (data) => {
					Debug.Log("onMatch" + data);
				});
			} else {
				Debug.LogError ("pClient null");
			}
		}


		//注册网络事件
		void InitNetEvent()
		{
			PomeloClient pClient = SavedContext.s_client;
			if (pClient != null)
			{
				pClient.on ("add", (data) => {
					m_initedLooper = MainLooper.instance();
					HandlerMessage msg = MainLooper.obtainMessage(handleMessage, MSG_HTTP_OK);
					msg.m_dataObj = data;
					m_initedLooper.sendMessage(msg);
				});

				pClient.on ("add", (data) => {
					HandlerMessage msg = MainLooper.obtainMessage(handleMessage, MSG_HTTP_OK);
					msg.m_dataObj = data;
					m_initedLooper.sendMessage(msg);
				});


				pClient.on ("add", (data) => {
					HandlerMessage msg = MainLooper.obtainMessage(handleMessage, MSG_HTTP_OK);
					msg.m_dataObj = data;
					m_initedLooper.sendMessage(msg);
				});

				pClient.on ("add", (data) => {
					HandlerMessage msg = MainLooper.obtainMessage(handleMessage, MSG_HTTP_OK);
					msg.m_dataObj = data;
					m_initedLooper.sendMessage(msg);
				});
			}
		}



		private const int MSG_HTTP_OK = 1;
		private const int MSG_HTTP_ERR = 2;
		private const int MSG_NET_ERR = 3;

		public void handleMessage(HandlerMessage msg)
		{
			Log.d<NetHttp>("handleMessage");

			switch (msg.m_what) {
			case MSG_HTTP_OK:
				{

				}
				break;
			}
		}

	}


}
