using UnityEngine;

namespace PipeDream
{
    public abstract class PipeItemGenerator : MonoBehaviour
    {
        public abstract void GenerateItems(Pipe pipe);
    }
}
