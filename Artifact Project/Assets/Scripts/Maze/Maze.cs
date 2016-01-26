using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Algorithms.CustomTypes;

namespace MazeCreator
{
	public class Maze : MonoBehaviour 
	{
		
		public MazeCell cellPrefab;
		public MazePassage passagePrefab;
		public MazeWall wallPrefab;
		public MazeDoor doorPrefab;
		public MazeRoomSettings[] roomSettings;

		public IntVector2 size;
		MazeCell[,] cells;
		List<MazeRoom> rooms = new List<MazeRoom>();

		[Range (0f,1f)]
		public float doorProbability;

		public IEnumerator Generate2DSlow()
		{
			WaitForSeconds delay = new WaitForSeconds(0.1f);
			cells = new MazeCell[size.x, size.y];
			List<MazeCell> activeCells =  new List<MazeCell>();
			GenStart(activeCells);
			while(activeCells.Count > 0)
			{
				yield return delay;
				GenStep(activeCells);
			}
		}

		public void Generate2D()
		{
			cells = new MazeCell[size.x, size.y];
			List<MazeCell> activeCells =  new List<MazeCell>();
			GenStart(activeCells);
			while(activeCells.Count > 0)
			{
				GenStep(activeCells);
			}
		}

		void GenStart(List<MazeCell> activeCells)
		{
			MazeCell newCell = CreateCell(RandomCoordinates);
			newCell.Initialize(CreateRoom(-1));
			activeCells.Add(newCell);
		}

		void GenStep(List<MazeCell> activeCells)
		{
			int currentIndex = activeCells.Count - 1;
			MazeCell currentCell = activeCells[currentIndex];

			if(currentCell.IsFullyInitialized)
			{
				activeCells.RemoveAt(currentIndex);
				return;
			}
            
			Direction direction = currentCell.RandomUnitializedDirection;;
			IntVector2 coord = currentCell.coordinates + direction.ToIntVector2();

			if(ContainsCoordinates(coord))
			{
				MazeCell neighbor = GetCell(coord);
				if (neighbor == null)
				{
					neighbor = CreateCell(coord);
					CreatePassage(currentCell, neighbor, direction);
					activeCells.Add(neighbor);
				}
				else if(currentCell.room.settingsIndex == 1 && neighbor.room.settingsIndex == 1)
					CreatePassageInSameRoom(currentCell,neighbor,direction);
				else
					CreateWall(currentCell, neighbor, direction);
			}
			else
				CreateWall(currentCell, null, direction);
		}

		MazeCell CreateCell (IntVector2 coord)
		{
			MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
			cells[coord.x,coord.y] = newCell;
			newCell.coordinates = coord;
			newCell.name = "Maze Cell " + coord.x + ", " + coord.y;
			newCell.transform.parent = transform;
			newCell.transform.localPosition =  new Vector3(coord.x - size.x *0.5f + 0.5f, 0f, coord.y - size.y * 0.5f + 0.5f);
			return newCell;
		}

		void CreatePassage (MazeCell cell, MazeCell otherCell, Direction direction) {
			MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
			MazePassage passage = Instantiate(prefab) as MazePassage;
			passage.Initialize(cell, otherCell, direction);
			passage = Instantiate(prefab) as MazePassage;
			if(passage is MazeDoor)
				otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
			else
				otherCell.Initialize(cell.room);
			passage.Initialize(otherCell, cell, direction.GetOpposite());
		}

		void CreatePassageInSameRoom(MazeCell cell, MazeCell neighborCell, Direction direction)
		{
			MazePassage passage =  Instantiate(passagePrefab) as MazePassage;
			passage.Initialize(cell, neighborCell, direction);
			passage = Instantiate (passagePrefab) as MazePassage;
			passage.Initialize(neighborCell, cell, direction.GetOpposite());
			if (cell.room != neighborCell.room) {
				MazeRoom roomToAssimilate = neighborCell.room;
				cell.room.Assimilate(roomToAssimilate);
				rooms.Remove(roomToAssimilate);
				Destroy(roomToAssimilate);
			}
		}
		
		void CreateWall (MazeCell cell, MazeCell otherCell, Direction direction) {
			MazeWall wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(cell, otherCell, direction);
			if (otherCell != null) {
				wall = Instantiate(wall) as MazeWall;
				wall.Initialize(otherCell, cell, direction.GetOpposite());
			}
		}

		MazeRoom CreateRoom (int exclude)
		{
			MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
			newRoom.settingsIndex = Random.Range (0, roomSettings.Length);
			if (newRoom.settingsIndex == exclude)
			    newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
			newRoom.settings = roomSettings[newRoom.settingsIndex];
			rooms.Add (newRoom);
			return newRoom;
		}

		public MazeCell GetCell (IntVector2 coordinates)
		{
			MazeCell cell = cells[coordinates.x,coordinates.y];
			return cell;
		}

		public IntVector2 RandomCoordinates
		{
			get
			{
				return new IntVector2(Random.Range (0, size.x), Random.Range (0, size.y));
			}
		}

		public bool ContainsCoordinates (IntVector2 coord)
		{
			return coord.x >= 0 && coord.x < size.x && coord.y >= 0 && coord.y < size.y;
		}
	}
}