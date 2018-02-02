using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**************************************
*FileName: GameUI.cs
*User:ysr
*Data: 2017/11/30
*Describe: 资源载入
**************************************/

public class GameUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LoadImg();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LoadImg()
    {
		Sprite[] sprites = Resources.LoadAll<Sprite>(PathObj.define_ui) ;

        Transform[] touch;
        touch = GetComponentsInChildren<Transform>();
        foreach (Transform child in touch)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                if (child.name == sprites[i].name)
                {

                    //child = (Sprite)sprites[0];
                    child.GetComponent<Image>().sprite = (Sprite)sprites[i];

                }
                if(child.name == "list_img" || child.name == "info_img")
                {
                    //九宫格属性
                    child.GetComponent<Image>().type = Image.Type.Sliced;
                }
            }
        }

        
    }
         
      
}
