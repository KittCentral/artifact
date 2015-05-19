// Is this script used anywhere?

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelfScroll : MonoBehaviour 
{
	public GameObject messBox;
	public RectTransform textRect;
	private float height;
	private Vector3 val;

	void Start () 
	{
		textRect = messBox.GetComponent<RectTransform>();
	}

	void Update () 
	{
		height = textRect.offsetMax.y - textRect.offsetMin.y;
		val = new Vector3(0,height/2 - 50,0);
		messBox.transform.localPosition = val;
	}
}
