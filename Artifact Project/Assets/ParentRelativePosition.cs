using UnityEngine;

public class ParentRelativePosition : MonoBehaviour
{
    public Vector3 followPosition;

	// Update is called once per frame
	void Update ()
    {
        transform.position = transform.parent.position + followPosition;
	}
}
