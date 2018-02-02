using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserNameItem : MonoBehaviour {
	public Text Label;

	public string Name {get; set; }

	public int Score { get; set;}

	

	public void OnToggleItem(bool pressed)
	{
		EventManage eventManage = GameObject.Find("EventSystem").GetComponent<EventManage>();
        eventManage.OnUserItemSelect(this);
	}

	public void SetName(string name) //, Color color
    {
		Name = name;
        Score = 0;

    }

	public void SetColor(int type)
	{
        Label.text = "" + Name + "        "+ Score;
        //ScoreLabel.text = "2";
	}

	//
	/*
	public void SetStateActive()
	{
		NameLabel.text = "<color=#99ff00>" + Name + "</color>";
		ScoreLabel.text = "<color=#99ff00>" + 0 + "</color>";
	}
	//
	public void SetStateDeActive()
	{
		NameLabel.text = "<color=#111111>" + Name + "</color>";
		ScoreLabel.text = "<color=#111111>" + 0 + "</color>";
	}*/

		
}
