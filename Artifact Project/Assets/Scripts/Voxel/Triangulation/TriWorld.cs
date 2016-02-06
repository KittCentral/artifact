using UnityEngine;
using System.Collections;

namespace Voxel
{
    public class TriWorld : MonoBehaviour
    {
        public GameObject chunk;

        // Use this for initialization
        void Start()
        {
            for (int x = -8; x < 8; x++)
            {
                for (int z = -8; z < 8; z++)
                {
                    GameObject newChunk = Instantiate(chunk, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)) as GameObject;
                    TriChunk chunkScript = newChunk.GetComponent<TriChunk>();
                    int w = chunkScript.chunkSize - 2;
                    chunkScript.posOffset = new Vector2(x * w, z * w);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
