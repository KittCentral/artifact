using UnityEngine;
using System.Collections;

namespace MazeCreator
{
	public class MazeWall : MazeCellEdge 
	{
		public Transform wall;

		public override void Initialize (MazeCell cell, MazeCell otherCell, Algorithms.CustomTypes.Direction direction)
		{
			base.Initialize (cell, otherCell, direction);
			wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
		}
	}
}
