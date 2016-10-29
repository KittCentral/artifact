using UnityEngine;
using System.Collections;
using Algorithms.CustomTypes;

namespace MazeCreator
{
	public class MazeDoor : MazePassage 
	{
		public Transform hinge;

        static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f), mirroredRotation = Quaternion.Euler(0f, 90f, 0f);
        bool isMirrored;



		MazeDoor OtherSideOfDoor {
			get {
				return neighborCell.GetEdge(direction.GetOpposite()) as MazeDoor;
			}
		}

        public override void Initialize(MazeCell primary, MazeCell other, Direction direction)
        {
            base.Initialize(primary, other, direction);

            if (OtherSideOfDoor != null)
            {
                isMirrored = true;
                hinge.localScale = new Vector3(-1f, 1f, 1f);
                Vector3 p = hinge.localPosition;
                p.x = -p.x;
                hinge.localPosition = p;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child != hinge)
                {
                    child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
                }
            }
        }

        public override void OnPlayerEntered()
        {
            OtherSideOfDoor.hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
            OtherSideOfDoor.cell.room.Show();
        }

        public override void OnPlayerExited()
        {
            OtherSideOfDoor.hinge.localRotation = hinge.localRotation = Quaternion.identity;
            OtherSideOfDoor.cell.room.Hide();
        }
	}
}