// General Camera Control Script
// Works best with the Camera as a child of an Empty Object where the focus is

using UnityEngine;

public class CameraController : MonoBehaviour 
{
    public enum CameraStateNames { Follow, FollowVelocity, MultipleFixed, Rotating, Plane }

    public CameraStateNames state;
	public Transform target;
	public bool targetFocus = true;
	public Vector3 followPosition;
	public Vector3[] setPositions =  new Vector3[1];
	public Vector3[] setRotations =  new Vector3[1];
	public Vector3 rotation;
	delegate void Control();
	Control control;

	int setIndex;
	public int SetIndex
	{
		get{return setIndex;}

		set{setIndex = value % setPositions.Length;}
	}

	void Start ()
	{
        if (target == null) {target = transform.parent.transform;}
		else {transform.parent.transform.position = target.position;}

		if(target.GetComponent<Rigidbody>() == null) {target.gameObject.AddComponent<Rigidbody>();}
		target.gameObject.GetComponent<Rigidbody>().useGravity = false;

		if(state == CameraStateNames.Follow) {control = Follow;}
		else if(state == CameraStateNames.FollowVelocity) {control = FollowVelocity;}
		else if(state == CameraStateNames.MultipleFixed) {control = MultipleFixed;}
        else if (state == CameraStateNames.Rotating) { control = Rotating; }
        else {control = Plane;}
	}

	void Update () 
	{
		control();
		if(targetFocus) transform.LookAt(target.position);
	}

	void Follow()
	{
		transform.localPosition = followPosition;
		return;
	}

	void FollowVelocity()
	{
		Vector2 unitVel = new Vector2(target.GetComponent<Rigidbody>().velocity.x, target.GetComponent<Rigidbody>().velocity.z).normalized;
		transform.localPosition = new Vector3(followPosition.x * unitVel.x, followPosition.y, followPosition.z * unitVel.y);
	}

	void MultipleFixed()
	{
		transform.localPosition = Vector3.Lerp(transform.localPosition, setPositions[setIndex], .1f);
		Quaternion tempRot = transform.localRotation;
		if(setRotations[setIndex] != null)
			 tempRot.eulerAngles = setRotations[setIndex];
		else
		{
			Debug.Log ("There is no rotation value for this index");
			tempRot.eulerAngles = new Vector3(0,0,0);
		}
		transform.parent.transform.localRotation = Quaternion.Lerp(transform.parent.transform.localRotation, tempRot, .1f);
	}

	void Rotating()
	{
		transform.parent.transform.Rotate(rotation,Space.Self);
	}

    void Plane()
    {
        Vector3 noYLocalPos = target.transform.forward * followPosition.z + target.transform.right * followPosition.x;
        Vector3 localPos = new Vector3(noYLocalPos.x, followPosition.y, noYLocalPos.z);
        transform.position = localPos + target.transform.position;// = Vector3.Lerp(transform.position, localPos + target.transform.position, .5f);
    }
}

