using UnityEngine;
using System.Collections;

public class Splatter : MonoBehaviour
{
    public float maxSize;
    float size;

	// Update is called once per frame
	void Update ()
    {
        size = Mathf.Lerp(size, maxSize, 0.1f);
        transform.localScale = new Vector3(size,size,size);
	}
}
