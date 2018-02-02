using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpgm;

/**************************************
*FileName: SkillManage.cs
*User: ysr 
*Data: 2017/12/12
*Describe: 技能管理脚本Event触发
**************************************/
public class SkillManage : MonoBehaviour {

    string skilljson ;
    int count = 0;  //剩余技能数
    SkillVal [] skills = new SkillVal[ConfigT.playerMax];    
    int id = 0;

    GameObject bulletObj;
    void Start()
    {
        //读入所以技能的数值表
        //skilljson = (string)Resources.Load(Path.skill_btn);
        //skills = JsonConvert.DeserializeObject<Skill []>(skilljson);

        //实例化技能物体
        bulletObj = (GameObject)Resources.Load("Prefabs/skill/bullet");
        SkillVal skill = new SkillVal();
        skill.id = 0;
        skill.name = "test";
        skill.hurt = 10;
        skill.duration = 3;
        skill.speed = -0.1f;
        skills[0] = skill;
       
    }
    //获得技能
    public void addSkill(int type , int level)
    {

        int id = type +(level - 1) * 10;
        //count = skils[id].count;

        //GameObject skillObj =  Resource.Load(Path.[]);
        //实例化技能

        //skillObj.GetComponent<>().init(skills[id]);
        //GameObject newbullet;
        //newbullet = (GameObject)Instantiate(bulletObj, findChild(id, "port").position, findChild(id, "port").rotation);
    }


    //释放技能
    public void onFire( int type, int level, Vector3 pos)
    {
        switch(type)
			{
				//普攻
				case 1:
				{
                    GameObject newbullet;
                    newbullet = (GameObject)Instantiate(bulletObj,pos , Quaternion.identity); // , pos, transform.rotation Quaternion.identity
                    newbullet.GetComponent<Skill>().init(skills[id]);
					
				}	
				break;
				//技能
				case 2:
				{
					//int id = type +(level - 1) * 10;
	
				}
					break;
				//闪现
				case 3:
				{
	
				}
				break;
			}
        
    }





}