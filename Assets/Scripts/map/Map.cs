using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;
using System;
using tpgm;
using UnityEngine.UI;

/**************************************
 *FileName: Map.cs
 *Author: Star
 *Data: 2017/12/2
 *Describe: 创建地图及地图上的事件实现
**************************************/

public class Map : MonoBehaviour
{

    public GameObject backgroupObj;

    public GameObject playerObj;
    public GameObject itemObj;
    public GameObject bulletObj;
    public GameObject skillObj;

    public GameObject Canvas;

    public GameObject Item;

    public SkillManage skillManage;

    //控制添加物体
    private bool isAdd = false;

    private int x = 0;
    private int y = 0;
    private string name;
    private int type = 0;

    //在线玩家
    public GameObject[] playerObjs = new GameObject[ConfigT.playerMax];




    void Start()
    {
        LoadObj();
        Canvas = GameObject.Find("Canvas");
        Item = GameObject.Find("Item");
        skillManage = GameObject.Find("SkillManage").GetComponent<SkillManage>() as SkillManage;
        Initbackgroup(1);

    }

    void LoadObj()
    {
        playerObj = (GameObject)Resources.Load("Prefabs/heros/roles/role1");
        bulletObj = (GameObject)Resources.Load("Prefabs/skill/bullet");
        itemObj = (GameObject)Resources.Load("Prefabs/rewards/icon_100002");
        skillObj = (GameObject)Resources.Load("game_res/test/player/bomb");

    }



    void Initbackgroup(int type)
    {
		backgroupObj = (GameObject)Resources.Load(PathObj.map + type);
        backgroupObj = Instantiate(backgroupObj, transform.position, transform.rotation);
        backgroupObj.transform.parent = Canvas.transform;
        backgroupObj.transform.localScale = Vector3.one;

        ItemVal item = new ItemVal();
        item.rehp = 5;
        item.score = 10;
        item.exp = 50;
		#if false
		TextAsset jsonFile = Resources.Load("data/val_item0", typeof(TextAsset)) as TextAsset;
		List<ItemJs> items = SimpleJson.SimpleJson.DeserializeObject<List<ItemJs>>(jsonFile.ToString());

		Debug.Log (items[10].x);
        for (int i = 0; i < items.Count; i++)
        {
            //Debug.Log("" + items[i].x);
            GameObject newitem;
            newitem = (GameObject)Instantiate(itemObj);
            newitem.transform.parent = Item.transform;
            
            newitem.transform.localPosition = new Vector3(items[i].x, items[i].y, 0);
            newitem.transform.localScale = 100*Vector3.one;
            newitem.GetComponent<Item>().init(item);

        }
		#endif
       


    }

	//设置添加玩家的数据
	public void setAddData(string name , int x , int y, int type)
	{	
		isAdd = true;
		this.name = name;
		this.type = type;
		this.x = x;
		this.y = y;
	}	

	//获取指定玩家的id 
	int getPlayerID(PlayerVal entite)
	{
		int id = 0;
		for(int i =0; i< 25;i++)
		{
            if(null !=playerObjs[i])
            {
                if (playerObjs[i].name == entite.uid)
                    id = i;
            }
			
		}
		return id;
	}


    //添加对象
   public void addGameObj(int i, string name, int x, int y , int type)
	{
        
		Debug.Log ("addGameObj" + name);
        GameObject newObj;
       
        switch (type)
		{
			case ObjType.player :
			{
				//newObj = playerObj;
			}
			break;
			case ObjType.item :
			{
				//newObj = itemObj;
			}
				break;
			case ObjType.skill :
			{
				//newObj = skillObj;
			}
				break;
		}
       
        Vector3 pos = new Vector3(x, y, 0);
        newObj = (GameObject)Instantiate(playerObj);
        newObj.transform.parent = Canvas.transform;
		newObj.name = name;
        newObj.transform.localScale = Vector3.one;
        newObj.transform.localPosition = pos;
        playerObjs[i] = newObj;
        //newObj.transform.localScale = Vector3.one;
    }		

	
	//删除对象
	public void delGameObj(int i)
	{
        Destroy(playerObjs[i]);
	}

	

	//接收Event并在场景里执行
	public void onEvent(int id, PlayerVal playerVal)
	{
       
        if (null != playerObjs[id] )
		{
			iTween.MoveTo(playerObjs[id], iTween.Hash("x", playerVal.x,
                                           "y", playerVal.y,
                                           "z", 0,
                                           "time", 1.0,
                                           "islocal", true
             ));
            //释放技能
          
            if(playerVal.skill != 0)
			{
                if(null != playerObjs[id])
                {
				   skillManage.onFire(playerVal.skill, 1, findChild(id, "port").position);
                }

			}
            
            //SetkillData(playerVal.x, playerVal.y, playerObjs[id].GetComponent<ATKAndDamage>().score);

        }
	}
    //查找玩家身上的子物体更新状态
    Transform findChild(int id, string name)
    {
        //ransform[] transforms;


        Transform[] tranchilds = playerObjs[id].GetComponentsInChildren<Transform>();
 

        foreach (Transform child in tranchilds)
        {
            if (child.name == name)
            {
                //child.GetComponent<>(). 脚步调用
                return child;
            }
        }
        return null;
    }


    void SetkillData(int kill, int die, float assist)
    {
        Text kill_tx = GameObject.Find("kill_tx").GetComponent<Text>();
        Text die_tx = GameObject.Find("die_tx").GetComponent<Text>();
        Text assist_tx = GameObject.Find("assist_tx").GetComponent<Text>();

        kill_tx.text = "" + kill;
        die_tx.text = "" + die;
        assist_tx.text = "" + assist;

    }


}



