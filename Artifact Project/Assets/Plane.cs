using UnityEngine;

namespace Flight
{
    public class Plane : MonoBehaviour
    {
        public float speed = 50f;
        public float rollSpeed = 1f;
        public float pitchSpeed = 1f;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            speed += transform.forward.y * Time.deltaTime * 50.0f;
            if (speed < 0)
                speed = 0;
            else if (speed >= 100)
                speed = 100;
            if (speed < 15)
                transform.Rotate((15f - speed) * - 0.05f, 0.0f, 0.0f);
            transform.position += transform.forward * -1 * Time.deltaTime * speed;
            transform.Rotate(Input.GetAxis("Vertical") * pitchSpeed * -1, 0.0f, Input.GetAxis("Horizontal") * rollSpeed);
            float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
            if (terrainHeight > transform.position.y)
                transform.position = new Vector3(transform.position.x, terrainHeight, transform.position.z);
        }
    }
}
