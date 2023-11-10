using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;

public class Board : MonoBehaviour
{
	// 현재 보드의 정보
    [SerializeField]
    int BoardNum;

	public GameObject nowPiece;

	// 현재 보드와 이어진 보드
	public Board Nearby_1;
	public Board Nearby_2;
	public Board Nearby_3;
	public Board Nearby_4;

	// 보드의 기물 배치 여부
    bool isFriend;
    bool isEnemy;

	private void Start()
	{
		InitBoard();
	}

	// 보드의 위치 정보
	public int getBoardNum()
    {
        return BoardNum;
    }

    // 체스판 아군 배치
    public void setFriend(bool InOut, GameObject piece)
    {
        // 배치 할 때
        if (InOut)
		{
			isFriend = true;
			nowPiece = piece;
		}
        // 제거/이동 할 때
        else
		{
			isFriend = false;
			nowPiece = null;
		}
            
    }
	public bool getFriend()
	{
		return isFriend;
	}

	// 체스판 에너미 배치
	public void setEnemy(bool InOut, GameObject piece)
	{
		// 배치 할 때
		if (InOut)
		{
			isEnemy = true;
			nowPiece = piece;
		}
		// 제거/이동 할 때
		else
		{
			isEnemy = false;
			nowPiece = null;
		}
			
	}
	public bool getEnemy()
	{
		return isEnemy;
	}

	// 인근 보드 정보 구하기
	public Board getNearby(int i)
	{
		switch(i)
		{
			case 1:
				return Nearby_1;
			case 2:
				return Nearby_2;
			case 3:
				return Nearby_3;
			case 4:
				return Nearby_4;
		}
		return null;
	}

	// 보드판 Init
	void InitBoard()
	{
		for (int i = 2; i < 8; i++)
		{
			for (int j = 2; j < 8; j++)
			{
				int num = (i * 10) + j;
				if (BoardNum == num)
				{
					Nearby_1 = GameObject.Find("FightBoard" + (num - 1).ToString()).GetComponent<Board>();
					Nearby_2 = GameObject.Find("FightBoard" + (num + 1).ToString()).GetComponent<Board>();
					Nearby_3 = GameObject.Find("FightBoard" + (num + 10).ToString()).GetComponent<Board>();
					Nearby_4 = GameObject.Find("FightBoard" + (num - 10).ToString()).GetComponent<Board>();
				}
			}
		}

		for (int i = 2; i < 8; i++)
		{
			int num1 = (i * 10) + 1;
			if (BoardNum == num1)
			{
				Nearby_1 = GameObject.Find("FightBoard" + (num1 + 1).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (num1 + 10).ToString()).GetComponent<Board>();
				Nearby_3 = GameObject.Find("FightBoard" + (num1 - 10).ToString()).GetComponent<Board>();
			}

			int num2 = (i * 10) + 8;
			if (BoardNum == num2)
			{
				Nearby_1 = GameObject.Find("FightBoard" + (num2 - 1).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (num2 + 10).ToString()).GetComponent<Board>();
				Nearby_3 = GameObject.Find("FightBoard" + (num2 - 10).ToString()).GetComponent<Board>();
			}

			int num3 = 10 + i;
			if (BoardNum == num3)
			{
				Nearby_1 = GameObject.Find("FightBoard" + (num3 - 1).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (num3 + 1).ToString()).GetComponent<Board>();
				Nearby_3 = GameObject.Find("FightBoard" + (num3 + 10).ToString()).GetComponent<Board>();
			}

			int num4 = 80 + i;
			if (BoardNum == num4)
			{
				Nearby_1 = GameObject.Find("FightBoard" + (num4 - 1).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (num4 + 1).ToString()).GetComponent<Board>();
				Nearby_3 = GameObject.Find("FightBoard" + (num4 - 10).ToString()).GetComponent<Board>();
			}
		}
		
		switch (BoardNum)
		{
			case 11:
				Nearby_1 = GameObject.Find("FightBoard" + (12).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (21).ToString()).GetComponent<Board>();
				break;
			case 18:
				Nearby_1 = GameObject.Find("FightBoard" + (17).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (28).ToString()).GetComponent<Board>();
				break;
			case 81:
				Nearby_1 = GameObject.Find("FightBoard" + (82).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (71).ToString()).GetComponent<Board>();
				break;
			case 88:
				Nearby_1 = GameObject.Find("FightBoard" + (87).ToString()).GetComponent<Board>();
				Nearby_2 = GameObject.Find("FightBoard" + (78).ToString()).GetComponent<Board>();
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		// 충돌한 콜라이더의 이름이 TouchDownVec일 때
		if (other.gameObject.name.Contains("TouchDownVec"))
		{
			if (nowPiece != null)
			{
				BM_instance.SetPiece(nowPiece);
			}
		}
		// 충돌한 콜라이더의 이름이 TouchUpVec일 때
		if (other.gameObject.name.Contains("TouchUpVec"))
		{
			if (nowPiece != null)
			{
				// 현재 움직이고 있는 피스가 있을 경우
				GameObject piece = BM_instance.GetPiece();

				if (piece != null)
				{
					// 피스의 스크립터를 가져온다
					ChessPiece script = piece.GetComponent<ChessPiece>();

					if (script.nowBoard.getBoardNum() != BoardNum)
					{
						if (BoardNum / 10 < 5)
						{
							script.ChangeBoard(BoardNum, nowPiece);

							// 보드에 피스 정보 저장
							setFriend(true, piece);
						}
						else if (BoardNum / 10 == 9)
						{
							script.ChangeBoard(BoardNum, nowPiece);

							// 보드에 피스 정보 저장
							setFriend(true, piece);
						}
					}

					// 피스의 위치정보 변경
					piece.transform.position = script.nowBoard.transform.position;

				}
			}
			else
			{
				// 현재 움직이고 있는 피스가 있을 경우
				GameObject piece = BM_instance.GetPiece();

				if (piece != null)
				{
					// 피스의 스크립터를 가져온다
					ChessPiece script = piece.GetComponent<ChessPiece>();

					if (BoardNum / 10 < 5)
					{
						script.ChangeBoard(BoardNum);
						// 보드에 피스 정보 저장
						setFriend(true, piece);
					}
					else if (BoardNum / 10 == 9)
					{
						script.ChangeBoard(BoardNum);
						// 보드에 피스 정보 저장
						setFriend(true, piece);
					}

					// 피스의 위치정보 변경
					piece.transform.position = script.nowBoard.transform.position;

				}
			}
		}
	}
}
