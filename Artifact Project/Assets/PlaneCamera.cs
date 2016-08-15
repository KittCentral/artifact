using UnityEngine;

public class PlaneCamera : MonoBehaviour
{
    float zRot = 0;
    float xRot = 0;
    float yPot = 0;
    void Update ()
    {
        zRot = Mathf.Lerp(zRot, Input.GetAxis("Horizontal") * 90, .05f);
        xRot = Mathf.Lerp(xRot, Input.GetAxis("Vertical") * 30, .05f);
        yPot = Mathf.Abs(Mathf.Lerp(yPot, Input.GetAxis("Horizontal") * 3, .05f));
        Quaternion rot = Quaternion.Euler(new Vector3(10 + xRot, 0, zRot));
        transform.localRotation = rot;
        transform.localPosition = new Vector3(0, 3 + yPot, -5 - 5 * transform.parent.GetComponent<Rigidbody>().velocity.magnitude / 200);
	}
}
