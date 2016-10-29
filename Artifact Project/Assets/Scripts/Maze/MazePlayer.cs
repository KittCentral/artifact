using UnityEngine;
using Algorithms.CustomTypes;

namespace MazeCreator
{
	public class MazePlayer : MonoBehaviour 
	{
		MazeCell currentCell;
		Direction currentDirection;
		Quaternion tarRot;
		Vector3 tarPos;

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

        public void SetLocation (MazeCell cell) 
		{
            if (CurrentCell != null)
                CurrentCell.OnPlayerExited();
            CurrentCell = cell;
			tarPos = cell.transform.localPosition;
            CurrentCell.OnPlayerEntered();
		}
		
		void Move (Direction direction) 
		{
			MazeCellEdge edge = CurrentCell.GetEdge(direction);
			if (edge is MazePassage)
				SetLocation(edge.neighborCell);
		}
		
		void Rotate (Direction direction) 
		{
			tarRot = direction.ToRotation();
			currentDirection = direction;
		}

		void Start()
		{
			tarRot = currentDirection.ToRotation();
			tarPos = CurrentCell.transform.localPosition;
            transform.position = tarPos;
		}
		
		void Update () {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				Move(currentDirection);
			}
			else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				Move(currentDirection.GetNextClockwise());
			}
			else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				Move(currentDirection.GetOpposite());
			}
			else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				Move(currentDirection.GetNextCounterclockwise());
			}
			else if (Input.GetKeyDown(KeyCode.Q)) {
				Rotate(currentDirection.GetNextCounterclockwise());
			}
			else if (Input.GetKeyDown(KeyCode.E)) {
				Rotate(currentDirection.GetNextClockwise());
			}
			transform.localPosition = Vector3.Lerp (transform.localPosition, tarPos, .1f);
			transform.localRotation = Quaternion.Lerp(transform.localRotation, tarRot, .1f);
		}
	}
}