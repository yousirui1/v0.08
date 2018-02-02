using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tpgm;
public class Follow : MonoBehaviour {
    
  
    private bool isFind = false;
	// Use this for initialization
	void Start () {
        //map = GameObject.Find("map").GetComponent<Map>() as Map;
    }

    GameObject userObj ;

    //LateUpdate晚于所有Update执行
    void LateUpdate()
    {
     

        if (!isFind)
        {

			userObj = GameObject.Find(SavedData.s_instance.m_user.m_uid);
            if (null != userObj)
            {
                this.transform.localPosition = new Vector3(userObj.transform.localPosition.x, userObj.transform.localPosition.y, 0);
                isFind = true;
            }

        }
        else
        {
            //限制相机移动范围
            if (transform.localPosition.y <= 6330 && transform.localPosition.y >= 330 && transform.localPosition.x <= 6050 && transform.localPosition.x >= 610)
            {
                this.gameObject.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -400);
            }
            else
            {
                if (transform.localPosition.y >= 6330 && ((transform.localPosition.x <= 6050) && transform.localPosition.x >= 610))
                {
                    this.gameObject.transform.localPosition = new Vector3(transform.localPosition.x, 6330, -400);
                }
                else if (transform.localPosition.y <= 330 && ((transform.localPosition.x <= 6050) && transform.localPosition.x >= 610))
                {
                    this.gameObject.transform.localPosition = new Vector3(transform.localPosition.x, 330, -400);
                }
                if (transform.localPosition.x >= 6050 && ((transform.localPosition.y <= 6330) && transform.localPosition.y >= 330))
                {
                    this.gameObject.transform.localPosition = new Vector3(6050, transform.localPosition.y, -400);
                }
                else if (transform.localPosition.x <= 610 && ((transform.localPosition.y <= 6330) && transform.localPosition.y >= 330))
                {
                    this.gameObject.transform.localPosition = new Vector3(610, transform.localPosition.y, -400);
                }
               

            }
            this.transform.localPosition = new Vector3(userObj.transform.localPosition.x, userObj.transform.localPosition.y, 0);
         
        }
    }




}
