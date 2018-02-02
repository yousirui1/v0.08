using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm;
using tpgm.UI;
using Pomelo.DotNetClient;
using SimpleJson;
//using Spine.Unity;

public class MainDameUIPage : UIPage
{
	private const string TAG = "MainUIPage";


	public MainDameUIPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		//布局预制体
		uiPath = "Prefab/ui/MainUI/MainUIPage";

	}


	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{

		/*{//贴图和动画
			texCache.markPageUseTex (m_pageID, "prefab/fish/yu_600001");
			texCache.pageBeginUseIconGlobal (m_pageID);
		}

		//json 数值表
		{
			valCache.markPageUseOrThrow<ValFish> (m_pageID, ConstsVal.val_fishWiKi);
		}*/
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{
		/*foreach (var item in m_loadedFishTexPathList) {
			texCache.unmarkPageUse ("1", item);
		}*/
	}

	#if false
	TexCache.ResLoadDoneProxy m_resLoadDoneProxy;
	//记录加载了的资源路径
	List<string> m_loadedFishTexPathList = new List<string>();
		
	public override void Awake(GameObject go)
	{

		//定时器
		//UIRoot.Instance.StartCoroutine(Timer());

		Debug.Log ("" + m_pageID);

		//Init(UIValue.friends_btnID, UIValue.friends_inputID, UIValue.friends_txID, UIValue.friends_imgID);
		m_controller = new Controller();
		m_controller.reqThirdLogin (false);

		/*if (null == m_resLoadDoneProxy) {
			m_resLoadDoneProxy = new TexCache.ResLoadDoneProxy(onResLoadDone);
		}*/
		/*string path = "prefab/fish/yu_600001";
		TexCache texCache = getTexCache ();
		if(!m_loadedFishTexPathList.Contains(path))
		{
			m_loadedFishTexPathList.Add (path);

			//异步加载资源
			texCache.loadResAsync(path, m_resLoadDoneProxy);
		}*/

		showFish ();
	}

	//秒定时器
	IEnumerator Timer() {
		while (true) {
			//onPomeloEvent_UpState ();
			yield return new WaitForSeconds(1.0f);

		}
	}


	void onResLoadDone(TexCache sender, string resPath, UnityEngine.Object resObjOrNull)
	{
		if (null != resObjOrNull) {
			TexCache texCache = getTexCache();
			texCache.markPageUseTex(m_pageID, resPath);
		}

	}

	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{

		/*{//贴图和动画
			texCache.markPageUseTex (m_pageID, "prefab/fish/yu_600001");
			texCache.pageBeginUseIconGlobal (m_pageID);
		}

		//json 数值表
		{
			valCache.markPageUseOrThrow<ValFish> (m_pageID, ConstsVal.val_fishWiKi);
		}*/
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{
		foreach (var item in m_loadedFishTexPathList) {
			texCache.unmarkPageUse ("1", item);
		}
	}


	protected override void onHandleMsg(HandlerMessage msg)
	{
		Debug.Log ("Message");
		switch (msg.m_what)
		{
		case 1:
			{
				Debug.Log (msg.m_dataStr);
				GameObject obj = (GameObject)msg.m_dataObj;
				Debug.Log (obj.name);
			}
			break;
		default:
			{

				Debug.Log (msg.m_dataStr);
				GameObject obj = (GameObject)msg.m_dataObj;
				Debug.Log (obj.name);
			}
			break;

		}
	}

	Controller m_controller;

	GameObject fishGo;
	void showFish()
	{

		string prefabPath = "Prefab/fish/yu_600001";

		TexCache texCache = getTexCache ();
		//载入缓存
		GameObject prefab = texCache.dynamicLoadOrNull<GameObject> (m_pageID, prefabPath);

		GameObject canvas = GameObject.Find ("UIRoot") as GameObject;

		fishGo = MonoBehaviour.Instantiate (prefab);
		fishGo.transform.SetParent (canvas.transform,false);
		//校正位置
		//Utils.seAnchoredPos(fishGo, 0 ,0);
		//创建点击事件
		//ClickDispatcher.create().onClick = ClickEvent;


		SkeletonGraphic skg = fishGo.GetComponent<SkeletonGraphic>();
		Spine.AnimationState animState = skg.AnimationState;

		animState.SetAnimation(0, "mov_you", true);
		animState.End += onSpineAnimEnd;
	
	}



	void onEventAnEnd(GameObject fishGo)
	{
		if (0 == m_flag) {
			m_flag = 1;
			SkeletonGraphic skg = fishGo.GetComponent<SkeletonGraphic>();
			Spine.AnimationState animState = skg.AnimationState;
			animState.SetAnimation(0, "mov_siwang", false);
		}

	}


	private int m_flag = 0;

	//第三方动画组件
	void onSpineAnimEnd(Spine.AnimationState pAnimState, int pTrackIndex)
	{
		string name = pAnimState.GetCurrent (pTrackIndex).Animation.Name;
		if ("mov_siwang".Equals (name)) {
			m_flag = 0;
			MainLooper looper = MainLooper.instance ();

			looper.removeMessages (looper_player);

			//looper.removeMessages
			HandlerMessage msg = MainLooper.obtainMessage(looper_player);
			msg.m_dataObj = pAnimState;
			looper.sendMessage(msg);
		}

	}

	void looper_player(HandlerMessage msg)
	{
		//判断是否要销毁
		if (hasDestroy ()) {
			return;
		}

		Spine.AnimationState animState = (Spine.AnimationState)msg.m_dataObj;
		animState.SetAnimation(0, "mov_you", true);
	}



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
			if (null != SavedContext.s_client) {
				SavedContext.s_client.NetWorkStateChangedEvent -= (state) => {
					Debug.Log (""+state);
				};
			}

		}

		//用于标识是那个接口用于处理接受函数
		private const int REQ_THIRD_LOGIN = 1;

		public void reqThirdLogin(bool isRetry)
		{

			ReqThirdLogin paramsValObj;
			string checkID;

			string api = "/login";

			AppUtils.apiCheckID (api);

			if (isRetry) {
				paramsValObj = m_netHttp.peekTopReqParamsValObj<ReqThirdLogin> ();
				paramsValObj.m_isRetry = 1;

				checkID = paramsValObj.m_checkID;

			} else {
				checkID = AppUtils.apiCheckID(api);
				paramsValObj = new ReqThirdLogin();
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_type = 1;
				paramsValObj.m_mac = "abc";

			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsValAsync(url,paramsValObj, REQ_THIRD_LOGIN, checkID);

		}


		//获取用户数据
		private const int REQ_THIRD_GETDATA = 2;

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
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_token = SavedData.s_instance.m_user.m_token;
				paramsValObj.m_signIn = 1;

			}

			string url = SavedContext.getApiUrl(api);
			m_netHttp.postParamsValAsync(url, paramsValObj, REQ_THIRD_GETDATA,checkID);

		}



		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_LOGIN:
				{
					RespThirdLogin resp = Utils.bytesToObject<RespThirdLogin>(respData.m_protobufBytes);
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
							reqThirdGetData (false);
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
			case REQ_THIRD_GETDATA:
				{

					Debug.Log ("REQ_THIRD_GETDATA");
					RespThirdGetData resp = Utils.bytesToObject<RespThirdGetData>(respData.m_protobufBytes);
					switch (resp.m_code) {
						case 200:
						{	
							Debug.Log ("" + resp.m_signInData);
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

	#endif
}
