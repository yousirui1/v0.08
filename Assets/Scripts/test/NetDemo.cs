using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJson;
using tpgm;
using System.Text;
using System.Collections.Generic;
using Pomelo.DotNetClient;


public class NetDemo : MonoBehaviour, NetHttp.INetCallback
{

	public virtual void onHttpOk(DataNeedOnResponse data, ResponseData resp)
	{

	}

	//异常是指网络断链,或数据解析失败。不包括code值不对
	public virtual void onHttpErr(DataNeedOnResponse data, int statusCode, string errMsg)
	{
		//Log.d<NetHttpDemo>("onHttpErr statusCode: " + statusCode);
		Debug.Log ("onHttpErr statusCode: " + statusCode);
	}

	public virtual void onOtherErr(DataNeedOnResponse data, int type)
	{
		//Log.d<NetHttpDemo>("onNetErr: ");
		Debug.Log ("onNetErr: "+type);
	}
	#if false

	NetHttp m_netHttp;

	private Canvas m_uiRoot;

	Dictionary<string, JsonObject>m_requesStack = new Dictionary<string, JsonObject>();
	Stack<string> m_failRequestStack = new Stack<string>();
		

	const int REQ_THIRD_LOGIN = 1;

	void Awake()
	{
		MainLooper.checkSetup ();
		m_netHttp = new NetHttp();
		m_netHttp.setPageNetCallback(this);

		Debug.Log (""+ AppUtils.GetVerStr());



		//onEvent ();
		//reqThirdLogin(false);
		SavedContext.setup ("game_Data");
		Debug.Log(SavedContext.getExternalPath("data"));
	}


	public void reqThirdLogin(bool isRetry)
	{
		ReqThirdLogin paramsValObj;
	    string checkID;

        string api = "fsdzz";
        if (isRetry)
        {
				//超时重连
			paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdLogin>();
                paramsValObj.m_isRetry = 1;
                checkID = paramsValObj.m_checkID;
        }
        else
        {
			
                checkID = AppUtils.apiCheckID(api);
				Debug.Log ("checkID"+checkID);
				paramsValObj = new ReqThirdLogin();
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_type = 0;
				paramsValObj.m_mac = "asdasdad";

         }

		string url =  SavedContext.getApiUrl("/login");
		m_netHttp.postParamsValAsync(url, paramsValObj, REQ_THIRD_LOGIN, checkID);
     }


	
		
	public void onEvent()
	{
		 paramsValObj;
		paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdLogin>();

		string checkID = "aaa";
		JsonObject jsObj = new JsonObject();
		jsObj["isRetry"] = 0;
		jsObj["checkID"] = 1;
		jsObj["type"] = 1;
		jsObj["mac"] = "asdsdasd";
		jsObj["account"] = "";
		jsObj["password"] = "";

		m_requesStack[checkID] = jsObj;
		m_failRequestStack.Push(checkID);

		postJsonObject(jsObj, checkID);
	}	

	void postJsonObject(JsonObject jsonObject, string checkID)
	{
		string url = "http://192.168.52.1:3000/login";

		m_netHttp.postParamsValAsync(url, jsonObject, 1, checkID);

	}
	PomeloClient m_pClient;
  public virtual void onHttpOk(DataNeedOnResponse data, ResponseData resp)
	{
	
		switch (data.m_reqTag)
		{
			case REQ_THIRD_LOGIN:
			{
				RespLogin re = Utils.bytesToObject<RespLogin>(resp.m_protobufBytes);

				switch (re.m_code)
				{
					case 200:
					{
						if (null == SavedData.s_instance)
						{
							SavedData.s_instance = new SavedData();
						}
						User user = SavedData.s_instance.m_user;
						user.m_uid = re.m_uid; 
						user.m_token = re.m_token; 


						//登录成功开启长连接;
						if (null == m_pClient)
						{
							if (null == SavedContext.s_client)
							{
								m_pClient = new PomeloClient();
								SavedContext.s_client = m_pClient;
							}
							else
							{
								m_pClient = SavedContext.s_client;
							}
							m_pClient.NetWorkStateChangedEvent += (state) =>
							{
								Debug.Log(state);
								//长连接状态改变，多是断连
								onPomeloState(state);
							};
						
							m_pClient.initClient("192.168.52.1", 7014, () =>
								{
									m_pClient.connect(null, (data1) =>
										{
											Debug.Log("onLogin"+data1);
											JsonObject jsMsg = new JsonObject();
											jsMsg["uid"] = "123123";
											m_pClient.request("gate.gateHandler.queryEntry", jsMsg, OnEnter);
										});
								});

						}

						SavedContext.s_gameServerConnected = false;
						

					}
					break;

				default:
				{
						Debug.Log (re.m_code);
				}
				break;

				}
			}
			break;

		}
    }


	//异常是指网络断链,或数据解析失败。不包括code值不对
	 public virtual void onHttpErr(DataNeedOnResponse data, int statusCode, string errMsg)
    {
        //Log.d<NetHttpDemo>("onHttpErr statusCode: " + statusCode);
		reqThirdLogin(true);
		Debug.Log ("onHttpErr statusCode: " + statusCode);
    }

    public virtual void onOtherErr(DataNeedOnResponse data, int type)
    {
        //Log.d<NetHttpDemo>("onNetErr: ");
		Debug.Log ("onNetErr: "+type);
    }



	void OnEnter(JsonObject data)
	{
		Debug.Log("loginTest2");
		Debug.Log ("" + data);
	}

	//长连接状态改变
	private void onPomeloState(NetWorkState state)
	{
		switch(state)
		{

		case NetWorkState.CLOSED:
			{

			}
			break;
		case NetWorkState.CONNECTING:
			{

			}
			break;
		case NetWorkState.CONNECTED:
			{

			}
			break;
		case NetWorkState.DISCONNECTED:
			{

			}
			break;
		case NetWorkState.TIMEOUT:
		{

		}
		break;
		case NetWorkState.ERROR:
			{

			}
			break;
		}
	}
	#endif

}
