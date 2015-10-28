using UnityEngine;
using System.Collections;

namespace Chess
{
	public class Move
	{
		public BoardPosition from;
		public BoardPosition to;

		public Move(BoardPosition f, BoardPosition t)
		{
			from = f;
			to = t;
		}
	}
}