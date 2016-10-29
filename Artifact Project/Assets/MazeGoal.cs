using UnityEngine;
using Algorithms.CustomTypes;

namespace MazeCreator
{
    public class MazeGoal : MonoBehaviour
    {
        MazeCell currentCell;

        public MazeCell CurrentCell
        {
            get
            {
                return currentCell;
            }

            set
            {
                currentCell = value;
            }
        }

        public void SetLocation(MazeCell cell)
        {
            CurrentCell = cell;
            transform.position = cell.transform.position;
        }
    }
}
