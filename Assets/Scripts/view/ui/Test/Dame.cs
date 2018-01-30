using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;
using SimpleJson;
using Pomelo.DotNetClient;

public class Dame : UIPage
{

	private const string TAG = "";

	public Dame() : base(UIType.PopUp, UIMode.DoNothing, UICollider.WithBg)
	{
		//布局预制体
		uiPath = "prefab/ui/MainUI/PublicUICheck";
	}

	public override void Awake(GameObject go)
	{

		this.gameObject.transform.Find("content/btn_back").GetComponent<Button>().onClick.AddListener(() =>
			{
				// 隐藏
				Hide();
			});
	}

	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	Controller m_controller;



	class Controller : NetHttp.INetCallback
	{
		NetHttp m_netHttp;
		PomeloClient m_pClient;

		public Controller()
		{
			m_netHttp = new NetHttp();
			m_netHttp.setPageNetCallback(this);

		}

		public void onDestroy()
		{
			m_netHttp.setPageNetCallback (null);

			//可能在login页面没有登录
			if (null != m_pClient) {
				m_pClient.NetWorkStateChangedEvent -= (state) => {
					Debug.Log (""+state);
				};
			}

		}

		//用于标识是那个接口用于处理接受函数
		private const int REQ_THIRD_LOGIN = 1;

		public void reqThirdLogin(bool isRetry)
		{

			ReqThirdLogin paramsValObj;
			JsonObject jsObj = new JsonObject();
			string checkID;

			string api = "/login";

			AppUtils.apiCheckID (api);

			if (isRetry) {
				paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdLogin> ();
				paramsValObj.m_isRetry = 1;

				checkID = paramsValObj.m_checkID;

			} else {
				checkID = AppUtils.apiCheckID(api);
				/*paramsValObj = new ReqThirdLogin();
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_type = 1;*/
				jsObj["isRetry"] = 0;
				jsObj["checkID"] = checkID ;
				jsObj["type"] = 1;
				jsObj["mac"] = "abc";

			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsJsonAsync(url, jsObj, REQ_THIRD_LOGIN, checkID);

		}



		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_LOGIN:
				{
					Debug.Log ("" + respData.m_json);
					RespThirdLogin resp = new RespThirdLogin ();
					resp = SimpleJson.SimpleJson.DeserializeObject<RespThirdLogin> (respData.m_json);
					Debug.Log ("" + resp.m_code);
					switch (resp.m_code) {
					case 200:
						{
							if (null == SavedData.s_instance) {
								SavedData.s_instance = new SavedData ();
							}
							User user = SavedData.s_instance.m_user;
							user.m_uid = resp.m_uid; 
							user.m_token = resp.m_token; 


							LoginConect client = new GameObject ("Client").AddComponent<LoginConect> ();
							client.onLogin (SavedData.s_instance.s_clientUrl,SavedData.s_instance.s_clientPort);

						}
						break;

					default:
						{
							Debug.Log (resp.m_code);
						}
						break;



					}
				}
				break;
			}

		}

		public virtual void onHttpErr(DataNeedOnResponse data, int statusCode, string errMsg)
		{
			Debug.Log (TAG +":" +"onHttpErr");
		}

		public virtual void onOtherErr(DataNeedOnResponse data, int type)
		{
			Debug.Log (TAG +":" +"onOtherErr");

		}
	}
}