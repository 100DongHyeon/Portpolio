using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Navigation : MonoBehaviour
{
	int nowPos;
	int targetPos;

	int iter = 0;

	class BoardNode
	{
		public int BoardNum;

		public BoardNode Parent;

		public List<BoardNode> nierNode;

		public int F_n;
		public int H_n;
		public int G_n;

		public bool isClose;
		public bool inOpen;

		public BoardNode(int num)
		{
			BoardNum = num;
			nierNode = new List<BoardNode>();
		}
	}

	List<BoardNode> Boards;
	// 보드판에 직접 접근하기 위한 Dictonary
	Dictionary<int, int> boardStature = new Dictionary<int, int>();

	BoardNode startNode;

	private void Start()
	{
		Boards = new List<BoardNode>();
		InitBoard();

		startNode = new BoardNode(0);
	}

	public int navigation(int nowpos, int targetpos, int range)
	{
		nowPos = nowpos;
		targetPos = targetpos;

		List<int> result = new List<int>();
		List<BoardNode> openList = new List<BoardNode>();
		List<BoardNode> closeList = new List<BoardNode>();

		Boards[boardStature[nowPos]].G_n = 0;
		Heuristic(Boards[boardStature[nowPos]], nowPos, targetPos);
		Boards[boardStature[nowPos]].F_n = Boards[boardStature[nowPos]].G_n + 
										   Boards[boardStature[nowPos]].H_n;
		Boards[boardStature[nowPos]].isClose = true;
		Boards[boardStature[nowPos]].Parent = startNode;

		openList.Add(Boards[boardStature[nowPos]]);
		closeList.Add(Boards[boardStature[nowPos]]);

		float outTime = 0;

		while (outTime < Time.deltaTime * 128f)
		{
			for (int i = 0; i < closeList[closeList.Count - 1].nierNode.Count; i++)
			{
				if (!closeList[closeList.Count - 1].nierNode[i].isClose)
				{
					if (!BoardManager.BM_instance.
						IsBlock(closeList[closeList.Count - 1].nierNode[i].BoardNum)
					|| closeList[closeList.Count - 1].nierNode[i].BoardNum == targetPos)
					{
						if (!closeList[closeList.Count - 1].nierNode[i].inOpen)
						{
							openList.Add(closeList[closeList.Count - 1].nierNode[i]);
							closeList[closeList.Count - 1].nierNode[i].Parent =
								closeList[closeList.Count - 1];
							closeList[closeList.Count - 1].nierNode[i].inOpen = true;
						}
					}
				}
			}

			int Shortest = 10000;

			for (int i = 0; i < openList.Count; i++)
			{
				if (openList[i].isClose)
					continue;

				Heuristic(openList[i], openList[i].BoardNum, targetPos);
				openList[i].G_n = 1;
				openList[i].F_n = openList[i].G_n + openList[i].H_n;

				if (openList[i].F_n < Shortest)
				{
					Shortest = openList[i].F_n;
					iter = i;
				}
			}

			if (openList[iter].BoardNum == targetPos)
				break;

			openList[iter].isClose = true;
			closeList.Add(openList[iter]);

			outTime += Time.deltaTime;
		}

		if (outTime > Time.deltaTime * 128f)
		{
			return 0;
		}

		int nowNum = closeList[closeList.Count - 1].BoardNum;

		while (nowNum > 0)
		{
			for (int i = 0; i < closeList.Count; i++)
			{
				if (closeList[i].BoardNum == nowNum)
				{
					result.Add(closeList[i].BoardNum);
					nowNum = closeList[i].Parent.BoardNum;
				}
			}
		}

		foreach(var node in closeList)
			node.isClose = false;
		foreach (var node in openList)
			node.inOpen = false;

		result.Reverse();

		return result[1];
	}

	void Heuristic(BoardNode node, int now, int target)
	{
		// 거리 변수
		int Distance = 0;

		// 두 피스 사이에 열의 거리를 구한다
		int column = (now % 10) - (target % 10);
		// 만약 열이 음수로 나올 경우 절대값으로 변환한다
		if (column < 0)
			column *= -1;

		// 나온 열 거리를 최종 거리 변수에 더한다
		Distance += column;

		// 두 피스 사이의 행의 거리를 구한다
		int row = (now / 10) - (target / 10);
		// 만약 행이 음수로 나올 경우 절대값으로 변환한다
		if (row < 0)
			row *= -1;

		// 나온 행 거리를 최종 거리 변수에 더한다
		Distance += row;

		node.H_n = Distance;
	}

	void InitBoard()
	{
		List<int> nums = new List<int>();

		// 네비게이션 보드판 (열과 행에 숫자에 따라 11~18/21~28 - 71~78/81~88까지 리스트에 추가)
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				int num = i * 10 + j;
				BoardNode node = new BoardNode(num);
				Boards.Add(node);
				nums.Add(num);
			}
		}

		for (int i = 0; i < Boards.Count; i++)
		{
			boardStature.Add(nums[i], i);
		}

		for (int i = 2; i < 8; i++)
		{
			for (int j = 2; j < 8; j++)
			{
				int num = (i * 10) + j;
				if (Boards[boardStature[num]].BoardNum == num)
				{
					Boards[boardStature[num]].nierNode.Add(Boards[boardStature[num + 10]]);
					Boards[boardStature[num]].nierNode.Add(Boards[boardStature[num - 10]]);
					Boards[boardStature[num]].nierNode.Add(Boards[boardStature[num + 1]]);
					Boards[boardStature[num]].nierNode.Add(Boards[boardStature[num - 1]]);
				}
			}
		}

		for (int i = 2; i < 8; i++)
		{
			int num1 = (i * 10) + 1;
			if (Boards[boardStature[num1]].BoardNum == num1)
			{
				Boards[boardStature[num1]].nierNode.Add(Boards[boardStature[num1 + 10]]);
				Boards[boardStature[num1]].nierNode.Add(Boards[boardStature[num1 - 10]]);
				Boards[boardStature[num1]].nierNode.Add(Boards[boardStature[num1 + 1]]);
			}

			int num2 = (i * 10) + 8;
			if (Boards[boardStature[num2]].BoardNum == num2)
			{
				Boards[boardStature[num2]].nierNode.Add(Boards[boardStature[num2 + 10]]);
				Boards[boardStature[num2]].nierNode.Add(Boards[boardStature[num2 - 10]]);
				Boards[boardStature[num2]].nierNode.Add(Boards[boardStature[num2 - 1]]);
			}

			int num3 = 10 + i;
			if (Boards[boardStature[num3]].BoardNum == num3)
			{
				Boards[boardStature[num3]].nierNode.Add(Boards[boardStature[num3 + 10]]);
				Boards[boardStature[num3]].nierNode.Add(Boards[boardStature[num3 + 1]]);
				Boards[boardStature[num3]].nierNode.Add(Boards[boardStature[num3 - 1]]);
			}

			int num4 = 80 + i;
			if (Boards[boardStature[num4]].BoardNum == num4)
			{
				Boards[boardStature[num4]].nierNode.Add(Boards[boardStature[num4 - 10]]);
				Boards[boardStature[num4]].nierNode.Add(Boards[boardStature[num4 + 1]]);
				Boards[boardStature[num4]].nierNode.Add(Boards[boardStature[num4 - 1]]);
			}
		}

		Boards[boardStature[11]].nierNode.Add(Boards[boardStature[12]]);
		Boards[boardStature[11]].nierNode.Add(Boards[boardStature[21]]);

		Boards[boardStature[18]].nierNode.Add(Boards[boardStature[17]]);
		Boards[boardStature[18]].nierNode.Add(Boards[boardStature[28]]);

		Boards[boardStature[81]].nierNode.Add(Boards[boardStature[82]]);
		Boards[boardStature[81]].nierNode.Add(Boards[boardStature[71]]);

		Boards[boardStature[88]].nierNode.Add(Boards[boardStature[87]]);
		Boards[boardStature[88]].nierNode.Add(Boards[boardStature[78]]);

	}
}
