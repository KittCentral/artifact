using UnityEngine;
using System.Collections;

public class PositionArray : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 quantity;
    public Vector3 spacing;
    bool finished;

    public bool Finished{ get{ return finished; } set{ finished = value; } }

    // Use this for initialization
    void Start ()
    {
        for (int x = 0; x < quantity.x; x++)
        {
            for (int y = 0; y < quantity.y; y++)
            {
                for (int z = 0; z < quantity.z; z++)
                {
                    GameObject clone = Instantiate(prefab, Vector3.Scale(new Vector3(x, y, z), spacing), Quaternion.identity) as GameObject;
                    clone.transform.parent = transform;
                }
            }
        }
        Finished = true;
	}
}
