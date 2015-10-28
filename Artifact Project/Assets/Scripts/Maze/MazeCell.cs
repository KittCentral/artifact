using UnityEngine;
using System.Collections;
using Algorithms.CustomTypes;
using System;

namespace MazeCreator
{
	public class MazeCell : MonoBehaviour 
	{
		public IntVector2 coordinates;
		public MazeRoom room;
		int initializedEdgeCount;

		MazeCellEdge[] edges = new MazeCellEdge[MazeDirection.Count];

		public void Initialize (MazeRoom room)
		{
			room.Add(this);
			transform.GetChild (0).GetComponent<Renderer>().material = room.settings.floorMaterial;
		}

		public bool IsFullyInitialized
		{
			get
			{
				return initializedEdgeCount == MazeDirection.Count;
			}
		}

		public Direction RandomUnitializedDirection
		{
			get
			{
				int skips = UnityEngine.Random.Range(0, MazeDirection.Count - initializedEdgeCount);
				for (int i = 0; i < Enum.GetNames(typeof(Direction)).Length; i++) {
					if (edges[i] == null) 
					{
						if (skips == 0) 
						{
							return (Direction)i;
						}
						skips -= 1;
					}
				}
				throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
			}
		}
		
		public MazeCellEdge GetEdge (Direction direction) 
		{
			return edges[(int)direction];
		}
		
		public void SetEdge (Direction direction, MazeCellEdge edge) 
		{
			edges[(int)direction] = edge;
			initializedEdgeCount += 1;
		}
	}
}