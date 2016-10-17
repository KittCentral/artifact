using UnityEngine;

namespace Galaxy
{
    public class StarCloseUp : MonoBehaviour
    {
        public Color color;
        Renderer rend;

        // Use this for initialization
        void Start()
        {
            rend = GetComponent<Renderer>();
            rend.material.color = color;
            rend.material.SetColor("_EmissionColor", color);
            GetComponent<Light>().color = color;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
