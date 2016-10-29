using UnityEngine;
using System.Collections;

namespace MazeCreator
{
	public class Manager : MonoBehaviour 
	{
		public KeyCode reset;

		public MazePlayer playerPrefab;
        public MazeGoal goalPrefab;
		public Maze mazePrefab;
		MazePlayer playerInstance;
        MazeGoal goalInstance;
		Maze mazeInstance;

		// Use this for initialization
		void Start () 
		{
			BeginGame();
		}
		
		// Update is called once per frame
		void Update () 
		{
			if(Input.GetKeyUp(reset))
				RestartGame();
            if (playerInstance.CurrentCell == goalInstance.CurrentCell)
                EndGame();

		}

		void BeginGame() 
		{
			Camera.main.clearFlags = CameraClearFlags.Skybox;
			Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
			mazeInstance = Instantiate (mazePrefab) as Maze;
			mazeInstance.Generate2D();
			playerInstance = Instantiate(playerPrefab) as MazePlayer;
			playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
            goalInstance = Instantiate(goalPrefab) as MazeGoal;
            goalInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
            Camera.main.clearFlags = CameraClearFlags.Depth;
			Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
		}

        void EndGame()
        {
            RestartGame();
        }

		void RestartGame() 
		{
			StopAllCoroutines();
			Destroy(mazeInstance.gameObject);
			if (playerInstance != null) {
				Destroy(playerInstance.gameObject);
			}
            if (goalInstance != null)
            {
                Destroy(goalInstance.gameObject);
            }
            BeginGame ();
		}
	}
}
