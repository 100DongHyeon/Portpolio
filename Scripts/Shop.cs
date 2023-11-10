using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecePooling;

public class Shop : MonoBehaviour
{
	// 상점에 배치할 기물들
    public GameObject[] ShopPiece;

	// 상점에 배치할 기물의 종류
	public ChessPiece.PieceName[] piece;

	// 티어 당 상정 등장 확률
    public class TierAppear
    {
		// 확률 
		public int[] Tear;

		// 초기화
		public TierAppear(int i = 100)
		{
			// 100% 기준 확률 초기화
			Tear = new int[i];
		}

		// 등장 확률에 맞게 티어 배치
		public void SetAppear(int t1, int t2, int t3, int t4, int t5)
        {
			for (int i = 0; i < t1; i++)
			{
				Tear[i] = 1;
			}
			for (int i = t1; i < t2; i++)
			{
				Tear[i] = 2;
			}
			for (int i = t2; i < t3; i++)
			{
				Tear[i] = 3;
			}
			for (int i = t3; i < t4; i++)
			{
				Tear[i] = 4;
			}
			for (int i = t4; i < t5; i++)
			{
				Tear[i] = 5;
			}
		}
    }

	// 상점 등장 확률
    TierAppear Probability;

	private void Start()
	{
		// 상점 등장 확률 초기화
		Probability = new TierAppear();

		// 최초 리롤
		resetShop();
	}

	/// <summary>
	/// 상점 리롤 함수
	/// </summary>
    public void resetShop()
    {
		// 레벨에 따라 상점 확률 확정
		setAppearToLevel();

		// 기물 배치
		for (int i = 0; i < 5; i++)
        {
			// 0~99 사이의 숫자를 임의로 선택 (티어당 할당된 숫자폭이 다름)
			int r = Random.RandomRange(0, 100);

			// 임의의 숫자가 할당된 티어 폭 안에 존재할 경우
			switch(Probability.Tear[r])
			{
				case 1:
					// 1티어 유닛 오브젝트 풀
					piece[i] = Pool_Instance.setPieceT1();
					break;
				case 2:
					// 2티어 유닛 오브젝트 풀
					piece[i] = Pool_Instance.setPieceT2();
					break;
				case 3:
					// 3티어 유닛 오브젝트 풀
					piece[i] = Pool_Instance.setPieceT3();
					break;
				case 4:
					// 4티어 유닛 오브젝트 풀
					piece[i] = Pool_Instance.setPieceT4();
					break;
				case 5:
					// 5티어 유닛 오브젝트 풀
					piece[i] = Pool_Instance.setPieceT5();
					break;
			}
		}

		// 배치 확정된 기물을 실제 상점에 등록
		settingShop();
	}

	/// <summary>
	/// 배치된 기물 상점 등록 함수
	/// </summary>
	void settingShop()
	{
		// 5개의 기물을 상점에 등록
		for (int i = 0; i < 5; i++)
		{
			ShopPiece[i].GetComponent<ShopPiece>().SetVis(piece[i]);
		}
	}

	/// <summary>
	/// 레벨당 티어 등장확률 확정 함수
	/// </summary>
	void setAppearToLevel()
    {
		// 플레이어 레벨에 따라 확률 재배치
        switch(GameManager.GM_instance._data.Lv)
        {
            case 1:
				// 1레벨 확률 (100%, 0%, 0%, 0%, 0%)
                Probability.SetAppear(100, 0, 0, 0, 0);
				break;
			case 2:
				// 2레벨 확률 (70%, 30%, 0%, 0%, 0%)
				Probability.SetAppear(70, 100, 0, 0, 0);
				break;
			case 3:
				// 3레벨 확률 (60%, 35%, 5%, 0%, 0%)
				Probability.SetAppear(60, 95, 100, 0, 0);
				break;
			case 4:
				// 4레벨 확률 (50%, 35%, 15%, 0%, 0%)
				Probability.SetAppear(50, 85, 100, 0, 0);
				break;
			case 5:
				// 5레벨 확률 (40%, 35%, 18%, 2%, 0%)
				Probability.SetAppear(40, 75, 98, 100, 0);
				break;
			case 6:
				// 6레벨 확률 (33%, 30%, 30%, 7%, 0%)
				Probability.SetAppear(33, 63, 93, 100, 0);
				break;
			case 7:
				// 7레벨 확률 (30%, 30%, 30%, 10%, 0%)
				Probability.SetAppear(30, 60, 90, 100, 0);
				break;
			case 8:
				// 8레벨 확률 (23%, 30%, 30%, 15%, 2%)
				Probability.SetAppear(23, 53, 83, 98, 100);
				break;
			case 9:
				// 9레벨 확률 (21%, 30%, 25%, 20%, 4%)
				Probability.SetAppear(21, 51, 76, 96, 100);
				break;
			case 10:
				// 10레벨 확률 (19%, 25%, 25%, 25%, 6%)
				Probability.SetAppear(19, 44, 69, 94, 100);
				break;
		}
    }
}
