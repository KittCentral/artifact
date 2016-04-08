using UnityEngine;
using System.Collections;

public class MenuButtonScript : MonoBehaviour {

	public void Start()
    {
        Application.LoadLevel(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
