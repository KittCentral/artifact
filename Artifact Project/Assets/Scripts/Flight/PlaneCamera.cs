using UnityEngine;
using System.Collections;

public class PlaneCamera : MonoBehaviour
{
    public GameObject enemyLock;
    Vector3 waypointPos;
    bool waypointFocus;
    float zRot = 0;
    float xRot = 0;
    float yPot = 0;
    Flight.Plane plane;

    void Start()
    {
        waypointPos = enemyLock.transform.position;
        plane = transform.parent.GetComponent<Flight.Plane>();
    }
    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            waypointFocus = !waypointFocus;
        }
        if(!waypointFocus)
        {
            zRot = Mathf.Lerp(zRot, Input.GetAxis("Horizontal") * 90, .05f);
            xRot = Mathf.Lerp(xRot, Input.GetAxis("Vertical") * 15, .05f);
            yPot = Mathf.Abs(Mathf.Lerp(yPot, Input.GetAxis("Horizontal") * 3, .05f));
            Quaternion rot = Quaternion.Euler(new Vector3(10 + xRot, 0, zRot));
            transform.localRotation = rot;
            transform.localPosition = new Vector3(0, 7 + yPot, -10 - 5 * plane.MaxVelocity / 200);
        }
        else
        {
            transform.LookAt(waypointPos);
            Vector3 dirVector = transform.position - waypointPos;
            transform.position = transform.parent.transform.position + dirVector.normalized * 20;
        }
	}
}
