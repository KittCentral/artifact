using UnityEngine;
using System.Collections;
using Algorithms.CustomTypes;

namespace MazeCreator
{
	public abstract class MazeCellEdge : MonoBehaviour 
	{
		public MazeCell cell, neighborCell;

		public Direction direction;

		public virtual void Initialize (MazeCell cell, MazeCell otherCell, Direction direction) 
		{
			this.cell = cell;
			this.neighborCell = otherCell;
			this.direction = direction;
			cell.SetEdge(direction, this);
			transform.parent = cell.transform;
			transform.localPosition = Vector3.zero;
			transform.localRotation = direction.ToRotation();
		}

        public virtual void OnPlayerEntered() { }

        public virtual void OnPlayerExited() { }
    }
}