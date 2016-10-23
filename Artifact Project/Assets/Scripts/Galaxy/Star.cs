using UnityEngine;
using UnityEngine.UI;

namespace Galaxy
{
    public class Star : MonoBehaviour
    {
        public string name;
        public string specType;
        public string distance;
        public string otherName;
        public string remarks;
        GalaxyCamera cam;
        public GameObject info;

        void Start()
        {
            info = GameObject.Find("Info");
        }

        void OnMouseDown()
        {
            cam = GameObject.Find("Main Camera").GetComponent<GalaxyCamera>();
            cam.focusStar = transform;
            cam.Focus = transform.position;
            cam.Distance = 1;
            cam.AlignView(transform.position);
            info.transform.GetChild(0).GetComponent<Text>().text = "Name: " + name + " " + otherName;
            info.transform.GetChild(1).GetComponent<Text>().text = "Spectral Type: " + specType;
            info.transform.GetChild(2).GetComponent<Text>().text = "Distance: " + distance;
            info.transform.GetChild(3).GetComponent<Text>().text = "Remarks: " + remarks;
        }
    }
}
