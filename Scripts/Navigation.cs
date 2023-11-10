using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;


// A* 알고리즘 변형, 응용 코드
public class Navigation : MonoBehaviour
{
	// 시작 지점
	int nowPos;
 	// 목표 지점
	int targetPos;

 	// 최소 경로 탐색을 위한 변수
	int iter = 0;

	// 알고리즘에 사용될 노드 구도
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

	// 길찾기 위한 가상의 보드판
	List<BoardNode> Boards;
	// 보드판에 직접 접근하기 위한 Dictonary
	Dictionary<int, int> boardStature = new Dictionary<int, int>();

	// 시작 노드
	BoardNode startNode;

	// 가상의 보드를 Init, 시작 노드 설정
	private void Start()
	{
		Boards = new List<BoardNode>();
		InitBoard();

		startNode = new BoardNode(0);
	}

	// 알고리즘 탐색 함수
	public int navigation(int nowpos, int targetpos, int range)
	{
 		// 시작 지점 설정
		nowPos = nowpos;
  		// 목표 지점 설정
		targetPos = targetpos;

		// 경로 설정을 위한 리스트
		List<int> result = new List<int>();
  		// Open리스트와 Close리스트
		List<BoardNode> openList = new List<BoardNode>();
		List<BoardNode> closeList = new List<BoardNode>();

		// 시작 노드의 데이터 입력
		Boards[boardStature[nowPos]].G_n = 0;
		Heuristic(Boards[boardStature[nowPos]], nowPos, targetPos);
		Boards[boardStature[nowPos]].F_n = Boards[boardStature[nowPos]].G_n + 
										   Boards[boardStature[nowPos]].H_n;
		Boards[boardStature[nowPos]].isClose = true;
		Boards[boardStature[nowPos]].Parent = startNode;

		openList.Add(Boards[boardStature[nowPos]]);
		closeList.Add(Boards[boardStature[nowPos]]);

		// 혹시나 있을 수도 있는 무한 루프를 대비한 탈출용 변수
		float outTime = 0;

		// 알고리즘 탐색 시작, 최대 128번 검사 이 이후 검사를 계속할 시(무한루프) 반복문 탈출
		while (outTime < Time.deltaTime * 128f)
		{
  			// Close리스트의 가장 마지막 노드(저번 오픈 리스트에서 최소 거리로 선택된 노드)를 기준으로 검사 시작
			for (int i = 0; i < closeList[closeList.Count - 1].nierNode.Count; i++)
			{
   				// 주변의 노드 중 Close리스트 내의 노드는 탐색 제외
				if (!closeList[closeList.Count - 1].nierNode[i].isClose)
				{
    					// 주변 노드 중 장애물이 있으면 탐색 제외
					if (!BoardManager.BM_instance.
						IsBlock(closeList[closeList.Count - 1].nierNode[i].BoardNum)
      					// 주변 노드 중 목표 노드가 있으면 검사 종료
					|| closeList[closeList.Count - 1].nierNode[i].BoardNum == targetPos)
					{
     						// 주변 노드에 Open리스트에 들어있지 않을 경우
						if (!closeList[closeList.Count - 1].nierNode[i].inOpen)
						{
      							// 새롭게 Open리스트에 추가
							openList.Add(closeList[closeList.Count - 1].nierNode[i]);
							closeList[closeList.Count - 1].nierNode[i].Parent =
								closeList[closeList.Count - 1];
							closeList[closeList.Count - 1].nierNode[i].inOpen = true;
						}
					}
				}
			}

			// 최소값 설정
			int Shortest = 10000;

			// Open리스트 내에서 모든 최소거리(f(n) = g(n) + h(h))를 구하고 비교
			for (int i = 0; i < openList.Count; i++)
			{
   				// Close 노드는 검사 제외
				if (openList[i].isClose)
					continue;

				Heuristic(openList[i], openList[i].BoardNum, targetPos);
    				// 대각선 이동이 존재하지 않기 때문에 순수 휴리스틱 거리만 사용하여 최소거리 판별
				openList[i].G_n = 1;
				openList[i].F_n = openList[i].G_n + openList[i].H_n;

				// 최솟값을 갱신하고 해당 노드의 첨차를 기억한다
				if (openList[i].F_n < Shortest)
				{
					Shortest = openList[i].F_n;
					iter = i;
				}
			}

  			// 만약 최소거리 노드로 선택된 Open리스트의 노드가 목표 지점일 경우
			if (openList[iter].BoardNum == targetPos)
   				// 반복문 탈출
				break;

			// 만약 최소거리 노드로 선택된 Open리스트의 노드가 목표 지점이 아닐 경우 다음 검사 시작
			openList[iter].isClose = true;
			closeList.Add(openList[iter]);

			outTime += Time.deltaTime;
		}

		// 무한루프에서 탈출했을 경우 목표지점까지 최소 경로 탐색이 불가함으로 임의의 null값을 반환
		if (outTime > Time.deltaTime * 128f)
		{
			return 0;
		}

		// 목표지점에 가장 가까운 노드에서 부터 경로 설정을 시작
		int nowNum = closeList[closeList.Count - 1].BoardNum;

		// 시작 노드(노드의 Num 값이0)에 도달할 때 까지 부모노드를 따라 최소 경로 탐색
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

		// 다음 탐색에 사용하기 위해 모든 Close리스트와 Open리스트르 초기화
		foreach(var node in closeList)
			node.isClose = false;
		foreach (var node in openList)
			node.inOpen = false;

		// 도착지점을 기준으로 경로로 획득한 노드들을 역방향 정렬을 이용해 시작지점 기준 경로로 변경
		result.Reverse();

  		// 다음으로 움직일 경로를 반환
		return result[1];
	}

	// 휴리스틱 거리를 구하기 위한 함수
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

	// 가상의 보드판 Init
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

		// 딕셔너리를 활용하기 위해 보드의 좌표정보로 리스트 내부에 해당 보드에 접근할 수 있도록 데이터 저장
		for (int i = 0; i < Boards.Count; i++)
		{
			boardStature.Add(nums[i], i);
		}

		// 모든 보드들에 이웃하고 있는 보드들의 노드를 저장시킴
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
