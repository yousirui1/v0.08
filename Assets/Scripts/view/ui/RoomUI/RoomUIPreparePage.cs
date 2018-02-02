using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using tpgm.UI;
using tpgm;
using DG.Tweening;
using SimpleJson;
using UnityEngine.SceneManagement;

public class RoomUIPreparePage : UIPage
{
	private const string TAG = "RoomUIPreparePage";
	Coroutine coroutine;

	GameObject tiemObj = null;
	public RoomUIPreparePage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.WithBg)
	{
		//布局预制体
		uiPath = "Prefab/UI/RoomUI/RoomUIPreparePage";

	}

	//秒定时器
	IEnumerator Timer() {
		while (true) {
			yield return new WaitForSeconds(2.0f);
			Refresh ();
		}
	}

	public override void Awake(GameObject go)
	{
		//ClientMgr.Instance ().Match ();
		JsonObject jsMsg = new JsonObject();
		jsMsg ["roomNum"] = ConectData.Instance.roomNum;


		//ClientMgr.Instance().onSend("area.gloryHandler.match",jsMsg);
		tiemObj = this.transform.Find("content/tx_time").gameObject;

		//定时器
		coroutine = UIRoot.Instance.StartCoroutine(Timer());

		this.gameObject.transform.Find("content/btn_close").GetComponent<Button>().onClick.AddListener(() =>
		{
			Hide();
			UIRoot.Instance.StopCoroutine(coroutine);
		});
		

	}

	private int rad =0;
	private int yellow = 0;
	private int green = 0;

	int count =0;
	int time =0 ;
	public override void Refresh()
	{
		//Debug.Log (count);
		count++;
		time++;
		tiemObj.GetComponent<Text> ().text = ""+time;
		if (ConectData.Instance.start_groups != null) {
			GameObject item;
			foreach (UDGroup.Group player in ConectData.Instance.start_groups) {
				bool isFind = false;
				for (int i = 0; i < 9; i++) {
					item = this.gameObject.transform.Find ("content/player_groups").transform.GetChild (i).gameObject;
					//Debug.Log ("count"+ConectData.Instance.start_groups.Count);
					//Debug.Log (item.name);
					//Debug.Log (player.uid);
					if (item.name == player.uid) {
						//Debug.Log ("isFind");
						isFind = true;	
						break;
					}
				}
				if (!isFind) {
					//Debug.Log ("is not Find");
					//Debug.Log (player.group);
					AddNewUserItem (player.uid, player.head, int.Parse (player.group.Substring (player.group.Length - 1)));


				}
			}
		}
		if (time == 5) {
			UIRoot.Instance.StopCoroutine(coroutine);
			//SceneManager.LoadScene("Game");
			Application.LoadLevel("Game");
		}


	}

	//
	private void AddNewUserItem(string uid, int imgID,int player_group)
	{

		switch (player_group) {
		case 1:
			{
				Debug.Log ("rad");
				GameObject item = this.gameObject.transform.Find ("content/player_groups").transform.GetChild (rad).gameObject;
				item.GetComponent<Image> ().sprite = TextureManage.getInstance().LoadAtlasSprite("Public/Atlases/Icon/General_icon","General_icon_"+imgID);
				item.name = uid;
				rad++;
				break;
			}
		case 2:
			{
				Debug.Log ("yellow" +(yellow+3));
				GameObject item = this.gameObject.transform.Find ("content/player_groups").transform.GetChild (yellow+3).gameObject;
				item.name = uid;
				item.GetComponent<Image> ().sprite = TextureManage.getInstance().LoadAtlasSprite("Public/Atlases/Icon/General_icon","General_icon_"+imgID);
				yellow++;
				break;
			}
		case 3:
			{
				Debug.Log ("green");
				GameObject item = this.gameObject.transform.Find ("content/player_groups").transform.GetChild (green+6).gameObject;
				item.name = uid;
				item.GetComponent<Image> ().sprite = TextureManage.getInstance().LoadAtlasSprite("Public/Atlases/Icon/General_icon","General_icon_"+imgID);
				green++;
				break;
			}
		}
	}


	public const int MSG_POMELO_READY = 1;
	public const int MSG_POMELO_INVITE = 2;
	public const int MSG_POMELO_ONCHAT = 3;
	public const int MSG_POMELO_MATCH = 4;
	public const int MSG_POMELO_ROOMADD = 5;
	public const int MSG_POMELO_GLORYADD = 6;

	protected override void onHandleMsg(HandlerMessage msg)
	{
		switch (msg.m_what) {
		case MSG_POMELO_READY:
			{

			}
			break;

		case MSG_POMELO_INVITE:
			{

			}
			break;

		case MSG_POMELO_ONCHAT:
			{

			}
			break;

		case MSG_POMELO_MATCH:
			{

			}
			break;

		case MSG_POMELO_ROOMADD:
			{

			}
			break;

		case MSG_POMELO_GLORYADD:
			{

			}
			break;
		default :
			{

			}
			break;
		}
	}






	protected override void loadRes(TexCache texCache, ValTableCache valCache)
	{
	}

	protected override void unloadRes(TexCache texCache, ValTableCache valCache)
	{

	}

	class Controller 
	{
		
	}

}
