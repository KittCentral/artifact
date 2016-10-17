using UnityEngine;
using System.Collections;

namespace Galaxy
{
    public class GalaxyCamera : MonoBehaviour
    {
        Camera camera;
        public Transform focusStar;
        Vector3 focus; Vector3 interpFocus;
        float distance;
        float asc; float originalAsc;
        float rot; float originalRot;
        Vector3 originalMousePos;
        bool outside;

        public Vector3 Focus { get { return focus; } set { focus = value; }}
        public float Distance { get { return distance; } set { distance = value; } }
       

        // Use this for initialization
        void Start()
        {
            camera = GetComponent<Camera>();
            Focus = Vector3.zero;
            focusStar = GameObject.Find("Star").transform;
            Distance = 1;
            outside = true;
        }

        // Update is called once per frame
        void Update()
        {
            
            Vector3 relativePos = new Vector3(Distance * Mathf.Sin(asc) * Mathf.Cos(rot), Distance * Mathf.Cos(asc), Distance * Mathf.Sin(asc) * Mathf.Sin(rot));
            if ((interpFocus - Focus).magnitude >= 1)
                interpFocus -= (interpFocus - Focus).normalized;
            else
                interpFocus = Vector3.Lerp(interpFocus, Focus, .1f);
            transform.position = interpFocus + relativePos;
            if (Input.GetMouseButton(1))
            {
                asc = Mathf.Lerp(asc,(originalMousePos.y - Input.mousePosition.y) / 50 + originalAsc,.5f);
                rot = Mathf.Lerp(rot,(originalMousePos.x - Input.mousePosition.x) / 50 + originalRot,.5f);
            }
            else
            {
                originalMousePos = Input.mousePosition;
                originalAsc = asc;
                originalRot = rot;
            }
            Distance *= Mathf.Pow(1.1f,Input.mouseScrollDelta.y);
            transform.LookAt(Focus);
            if (outside && Distance < 1)
            {
                focusStar.GetComponent<Star>().Inside();
                outside = false;
            }
            else if(!outside && Distance >= 1)
            {
                focusStar.GetComponent<Star>().Outside();
                outside = true;
            }
        }

        public void AlignView (Vector3 target)
        {

            Vector3 dif = target - transform.position;
            rot = Mathf.Atan(dif.y / dif.x);
            asc = Mathf.Acos(dif.z / Mathf.Sqrt(Mathf.Pow(dif.x, 2) + Mathf.Pow(dif.y, 2) + Mathf.Pow(dif.z, 2)));
        }
    }
}
