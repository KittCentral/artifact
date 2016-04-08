using UnityEngine;
using System.Collections;

public class CylRotator : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate(new Vector3(0, -150, 0) * Time.deltaTime);
    }
}