using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using DG.Tweening;
using tpgm;


public class InfoUIMainPage : UIPage
{

	private const string TAG = "InfoUIMainPage";

	private GameObject friendItem = null;
	private GameObject friendList = null;

	private List<UIFriendItem> friendItems = new List<UIFriendItem>();

	//当前item
	private UIFriendItem currentItem = null;	

	private TabControl tabControl = null;

	private List<TabIndex> tablist = new  List<TabIndex> ();



	public InfoUIMainPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
	{
		//布局预制体
		uiPath = "Prefab/UI/InfoUI/InfoUIMainPage";

	}

	public override void Awake(GameObject go)
	{
		//Init(UIValue.friends_btnID, UIValue.friends_inputID, UIValue.friends_txID, UIValue.friends_imgID);


		tabControl = this.transform.Find ("tabcontrol").GetComponent<TabControl> () as TabControl;

		tablist.Add (new TabIndex(0,"账号", Path.info_panel));
		tablist.Add (new TabIndex(1,"段位", Path.info_panel));
		tablist.Add (new TabIndex(2,"数据", Path.info_panel));

		for (int i = 0; i < tablist.Count; i++) {
			tabControl.CreateTab (tablist[i].id, tablist[i].tabname, tablist[i].panelPath);
		}

		this.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
		{
				ClosePage();
		});


		/*friendList = this.transform.Find("tabcontrol/Panels/Panel(Clone)").gameObject;

		friendItem = this.transform.Find("tabcontrol/Panels/Panel(Clone)/Viewport/Content/item").gameObject;
		friendItem.SetActive(false);

		this.transform.Find("user1").GetComponent<Button>().onClick.AddListener(() =>
			{
				Refresh();
			});

		this.transform.Find("btn_start").GetComponent<Button>().onClick.AddListener(() =>
			{
				Application.LoadLevel("Game");
			});

		this.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
			{
				ClosePage();
			});*/
	}
	#if false

	//更新
	public override void Refresh()
	{
		friendList.transform.localScale = Vector3.zero;	
		friendList.transform.DOScale(new Vector3(1, 1, 1), 0.5f);

		//Get Friend Data
		//查看前一个页面有没有传入参data没有则初始化GameData	
		//UDFriend.friends friendData = this.data != null ? this.data as UDFriend : GameData.Instance.playerFriend;


		if (ConectData.Instance.friedns == null) {
			//ConectData.Instance.friedns = GameData.Instance.playerFriend.friends;
		} else {
			for (int i = 0; i < ConectData.Instance.friedns.Count; i++) {
				CreateItem (ConectData.Instance.friedns [i]);
			}
		}

	}

	//隐藏
	public override void Hide()
	{
		for(int i = 0; i< friendItems.Count ;i++)
		{
			GameObject.Destroy(friendItems[i].gameObject);
		}
		friendItems.Clear();

		this.gameObject.SetActive(false);
	}


	#region this logic

	private void CreateItem(UDFriend.Friend friend)
	{
		GameObject go = GameObject.Instantiate(friendItem) as GameObject;
		go.transform.SetParent(friendItem.transform.parent);
		go.transform.localScale = Vector3.one;
		go.SetActive(true);

		UIFriendItem item = go.AddComponent<UIFriendItem>();
		item.Refresh(friend);
		friendItems.Add(item);	

		//add click btn listener
		go.AddComponent<Button>().onClick.AddListener(OnClickItem);

	}

	//邀请好友
	private void OnClickItem()
	{
		//获取好友信息
		UIFriendItem item = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UIFriendItem>();
		//发送给服务端	
		//Client.Send
	}
	#endregion

	#endif





	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	class Controller : NetHttp.INetCallback
	{
		NetHttp m_netHttp;


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

		public void reqThirdLogin(bool isRetry,int type)
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
				//重连
				paramsValObj.m_checkID = checkID;
				paramsValObj.m_isRetry = 0;
				paramsValObj.m_type = type;
				paramsValObj.m_mac = InfoUtil.getMac();
				paramsValObj.m_account = GameObject.Find("input_user").GetComponent<InputField>().text;
				paramsValObj.m_password = GameObject.Find ("input_passwd").GetComponent<InputField> ().text;

				paramsValObj.m_password = Md5Util.GetMd5FromStr(paramsValObj.m_password);

				//保存数据
				PlayerPrefs.SetString("account", paramsValObj.m_account);
				PlayerPrefs.SetString("password", paramsValObj.m_password);
			}

			string url = SavedContext.getApiUrl(api);
			Debug.Log (url);
			m_netHttp.postParamsValAsync(url,paramsValObj, REQ_THIRD_LOGIN, checkID);

		}





		public virtual void onHttpOk(DataNeedOnResponse data, ResponseData respData)
		{
			Debug.Log ("onHttpOk");
			switch (data.m_reqTag) {
			case REQ_THIRD_LOGIN:
				{
					RespThirdLogin resp = Utils.bytesToObject<RespThirdLogin> (respData.m_protobufBytes);
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
							client.onLogin (SavedData.s_instance.s_clientUrl, SavedData.s_instance.s_clientPort);
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




