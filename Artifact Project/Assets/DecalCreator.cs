using UnityEngine;
using System.Collections;

public class DecalCreator : MonoBehaviour
{
    public GameObject splatOriginal;
    GameObject clone;

    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            clone = Instantiate(splatOriginal, new Vector3(0, 0.5f, -2), Quaternion.Euler(new Vector3(90, 0, 0))) as GameObject;
        }
	}
}
