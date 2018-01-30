using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;
using System;

/**************************************
*FileName: PublicUICheckPage.cs
*User: ysr 
*Data: 2018/1/30
*Describe: 签到页面
**************************************/

public class PublicUICheckPage : UIPage
{
	private const string TAG = "PublicUICheckPage";

	MainLooper m_initedLooper;

	Controller m_controller;

	//滑动列表
	private GameObject Item = null;

	private GameObject List = null;


	public PublicUICheckPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.WithBg)
	{
		//布局预制体
		uiPath = "Prefab/UI/PublicUI/PublicUICheck";

	}

	public override void Awake(GameObject go)
	{

		m_controller = new Controller (this);

		init ();

		m_controller.reqThirdGetData (false);

		this.gameObject.transform.Find("content/btn_back").GetComponent<Button>().onClick.AddListener(() =>
		{
				// 隐藏
				Hide();
		});
	}

	private void init()
	{
		
		Item = this.transform.Find("content/Panels/Viewport/Content/item").gameObject;
		List = this.transform.Find("content/Panels").gameObject;
		Item.SetActive (false);
		
		ValTableCache valCache = getValTableCache ();
		Dictionary<int, ValSignInAdd> valDict = valCache.getValDictInPageScopeOrThrow<ValSignInAdd>(m_pageID, ConstsVal.val_signInAdd);

		for (int i = 1; i < valDict.Count; i++) {
			ValSignInAdd val = ValUtils.getValByKeyOrThrow(valDict, i);
			CreateItem(val);			
		}


	}


	public override void Refresh()
	{
		
	}

	private void CreateItem(ValSignInAdd val)
	{
		GameObject go = GameObject.Instantiate(Item) as GameObject;
		go.transform.SetParent(Item.transform.parent);
		go.transform.localScale = Vector3.one;
		go.SetActive(true);
		
		UISignInAdd item = go.AddComponent<UISignInAdd>();
		item.Refresh(val);
		
		go.AddComponent<Button>().onClick.AddListener(OnClickItem);

	}

	private void OnClickItem()
	{
		UISignInAdd item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UISignInAdd>();
		
	}

	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
		valCache.markPageUseOrThrow<ValGlobal> (m_pageID, ConstsVal.val_global);
		valCache.markPageUseOrThrow<ValSignIn7> (m_pageID, ConstsVal.val_signIn7);
		valCache.markPageUseOrThrow<ValSignInAdd> (m_pageID, ConstsVal.val_signInAdd);
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{
		valCache.unmarkPageUse(m_pageID, ConstsVal.val_global);
		valCache.unmarkPageUse(m_pageID, ConstsVal.val_signIn7);
		valCache.unmarkPageUse(m_pageID, ConstsVal.val_signInAdd);
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

	class Controller : BaseController<RegisterUIPage>,NetHttp.INetCallback
	{
		NetHttp m_netHttp;


		PublicUICheckPage m_check;
		public Controller(PublicUICheckPage iview):base(null)
		{
			m_netHttp = new NetHttp();
			m_netHttp.setPageNetCallback(this);
			m_check = iview;
		}


		public void onDestroy()
		{
			m_netHttp.setPageNetCallback (null);

			//可能在login页面没有登录
			if (null != SavedContext.s_client) {
				SavedContext.s_client.NetWorkStateChangedEvent -= (state) => {
					Debug.Log (""+state);
				};
			}

		}

		//用于标识是那个接口用于处理接受函数
		private const int REQ_THIRD_GETDATA = 4;

		public void reqThirdGetData(bool isRetry)
		{

			ReqThirdGetData paramsValObj;
			string checkID;

			string api = "/getdata";

			AppUtils.apiCheckID (api);

			if (isRetry) {
				paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdGetData> ();
				paramsValObj.m_isRetry = 1;
				checkID = paramsValObj.m_checkID;

			} else {
				checkID = AppUtils.apiCheckID(api);
				paramsValObj = new ReqThirdGetData();
				//重连
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_token = SavedData.s_instance.m_user.m_token;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_signIn = 1;

			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsValAsync(url,paramsValObj, REQ_THIRD_GETDATA, checkID);

		}





		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_GETDATA:
				{
					RespThirdGetData resp = Utils.bytesToObject<RespThirdGetData> (respData.m_protobufBytes);
					switch (resp.m_code) {
					case 200:
						{
							Debug.Log (resp.m_signInData);


							Debug.Log (resp.m_signInData);


							JsonThirdSignInData jsSignInData = SimpleJson.SimpleJson.DeserializeObject<JsonThirdSignInData> (resp.m_signInData);
							Debug.Log (jsSignInData.signInAdd);

							/*ValTableCache valCache = m_check.getValTableCache ();
							Dictionary<int, ValSignInAdd> valDict = valCache.getValDictInPageScopeOrThrow<ValSignInAdd>(m_check.m_pageID, ConstsVal.val_signInAdd);
							ValSignInAdd val = ValUtils.getValByKeyOrThrow(valDict,jsSignInData.signInAdd);
							Debug.Log (val.reward);*/


			
							/*Dictionary<int, ValGlobal> gvalDict = valCache.getValDictInPageScopeOrThrow<ValGlobal>(m_check.m_pageID, ConstsVal.val_signInAdd);
							ValGlobal gval = ValUtils.getValByKeyOrThrow(valDict,jsSignInData.signInAdd);
							Debug.Log (val.reward);*/

							/*//m_looper = MainLooper.instance();
							HandlerMessage msg = MainLooper.obtainMessage(handleMessage, MSG_HTTP_OK);
							msg.m_dataObj = data;
							m_looper.sendMessage(msg);


							//Debug.Log (resp.m_signInData);
							//JsonThirdSignInData jsSignData = new JsonThirdSignInData ();
							jsSignData = SimpleJson.SimpleJson.DeserializeObject (resp.m_signInData);

							//计算可以签到天数
							DateTime dt1 = Convert.ToDateTime(jsSignData.createDate);
							TimeSpan span = DateTime.UtcNow.Subtract(dt1); 
							int dayDiff = (span.Days + 1)%7;

							string[] st_daycheck = jsSignData.signIn7.Split(',');

							//补签天数
							int dayfillcheck = 0;

							//已签到天数
							int daycheck = 0;*/
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
