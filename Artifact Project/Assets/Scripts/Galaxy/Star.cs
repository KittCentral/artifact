using UnityEngine;
using System.Collections;

namespace Galaxy
{
    public class Star : MonoBehaviour
    {
        public string name;

        void OnMouseDown()
        {
            GalaxyCamera cam = GameObject.Find("Main Camera").GetComponent<GalaxyCamera>();
            cam.Focus = transform.position;
            cam.Distance = 1;
            cam.AlignView(transform.position);
            print(name);
        }
    }
}
