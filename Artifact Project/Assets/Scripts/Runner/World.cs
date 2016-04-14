using UnityEngine;
using System.Collections.Generic;

namespace Runner
{
    public class World : MonoBehaviour
    {
        public enum Dimension { Second, Third };
        public Dimension dim;
        public float courseSpeed;

        public GameObject flat2DPrefab;

        List<GameObject> pieces = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            if(dim == Dimension.Second)
            {
                Transform camTran = transform.FindChild("Main Camera");
                camTran.position = new Vector3(0, 10, 8);
                camTran.rotation = Quaternion.Euler(90, 0, 0);
                camTran.gameObject.GetComponent<Camera>().orthographic = true;
            }
            else
            {
                Transform camTran = transform.FindChild("Main Camera");
                camTran.position = new Vector3(0, 10, -8);
                camTran.rotation = Quaternion.Euler(30, 0, 0);
                camTran.gameObject.GetComponent<Camera>().orthographic = false;
            }
            for (int i = 0; i < 3; i++)
            {
                GameObject ground = Instantiate(flat2DPrefab, new Vector3(0, 0, 30 * i), Quaternion.identity) as GameObject;
                ground.transform.parent = transform;
                pieces.Add(ground);
            }
        }

        // Update is called once per frame
        void Update()
        {
            foreach (GameObject obj in pieces)
            {
                obj.transform.Translate(Vector3.forward * -Time.deltaTime * courseSpeed);
                if (obj.transform.position.z < -30f)
                    obj.transform.position = new Vector3(transform.position.x, transform.position.y, 60f);
            }
        }
    }
}
