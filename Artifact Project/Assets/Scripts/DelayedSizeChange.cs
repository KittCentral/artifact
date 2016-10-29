using UnityEngine;
using System.Collections;

namespace Flight
{
    public class DelayedSizeChange : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            StartCoroutine("HoldforInstances");
        }
        
        IEnumerator HoldforInstances()
        {
            while (!gameObject.GetComponent<PositionArray>().Finished)
                yield return null;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector3(10, 10, 10);
                transform.GetChild(i).position *= 2000;
            }
        }
    }
}
