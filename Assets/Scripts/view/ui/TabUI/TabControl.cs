using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using tpgm;



public class TabControlEntry
{
	public GameObject panel;
	public GameObject tab;
}




public class TabControl : MonoBehaviour
{

	private List<TabControlEntry> entries = new List<TabControlEntry> ();


	public GameObject panelContainer ;

	public GameObject tabContainer ;


	public GameObject tabPrefab ;

	public GameObject panelPrefab ;


	void Start()
	{


	}

	public void AddEntry(TabControlEntry entry)
	{
		entries.Add(entry);
	}

	private void AddButtonListener(TabControlEntry entry)
	{
		entry.tab.GetComponent<Button>().onClick.AddListener(() => SelectTab(entry));
	}

	private void SelectTab(TabControlEntry selectedEntry)
	{
		foreach (TabControlEntry entry in entries)
		{
			bool isSelected = entry == selectedEntry;

			entry.tab.GetComponent<Button>().interactable = !isSelected;
			entry.panel.SetActive(isSelected);
		}
	}


	public void CreateTab(int id,string name,string panelPath)
	{
		TabControlEntry entrie = new TabControlEntry ();


		if (panelPath != null)
			panelPrefab = ResourceMgr.Instance ().Load<GameObject> (panelPath + id, false) as GameObject;
		else {
			
		}
		entrie.panel = Instantiate(panelPrefab) as GameObject;
		entrie.panel.transform.SetParent(panelContainer.transform);
		entrie.panel.transform.localScale = Vector3.one;
		entrie.panel.transform.name = "panel"+id;
		//entrie.panel.transform.localPosition = new Vector3(entrie.panel.transform.GetComponent<RectTransform> ().sizeDelta.y/2,0,0);
		entrie.panel.transform.localPosition = Vector3.zero;

		entrie.tab = Instantiate(tabPrefab) as GameObject;
		entrie.tab.transform.SetParent(tabContainer.transform);
		entrie.tab.transform.localScale = Vector3.one;
		entrie.tab.transform.localPosition = Vector3.zero;
		entrie.tab.transform.Find ("Text").GetComponent<Text>().text = name;
		entrie.tab.transform.name = "tab"+id;
		entries.Add (entrie);

		AddButtonListener (entrie);
		SelectTab(entries[0]);
	}



		




}
