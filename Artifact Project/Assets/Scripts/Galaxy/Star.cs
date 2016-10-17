using UnityEngine;
using System.Collections;

namespace Galaxy
{
    public class Star : MonoBehaviour
    {
        public string name;
        GalaxyCamera cam;

        void OnMouseDown()
        {
            cam = GameObject.Find("Main Camera").GetComponent<GalaxyCamera>();
            cam.focusStar = transform;
            cam.Focus = transform.position;
            cam.Distance = 1;
            cam.AlignView(transform.position);
            print(name);
        }

        public void Inside()
        {
            cam.focusStar.GetChild(0).gameObject.SetActive(false);
            cam.focusStar.GetChild(1).gameObject.SetActive(true);
        }

        public void Outside()
        {
            cam.focusStar.GetChild(1).gameObject.SetActive(false);
            cam.focusStar.GetChild(0).gameObject.SetActive(true);
        }
    }
}
