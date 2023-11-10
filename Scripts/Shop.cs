using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecePooling;

public class Shop : MonoBehaviour
{
	// ������ ��ġ�� �⹰��
    public GameObject[] ShopPiece;

	// ������ ��ġ�� �⹰�� ����
	public ChessPiece.PieceName[] piece;

	// Ƽ�� �� ���� ���� Ȯ��
    public class TierAppear
    {
		// Ȯ�� 
		public int[] Tear;

		// �ʱ�ȭ
		public TierAppear(int i = 100)
		{
			// 100% ���� Ȯ�� �ʱ�ȭ
			Tear = new int[i];
		}

		// ���� Ȯ���� �°� Ƽ�� ��ġ
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

	// ���� ���� Ȯ��
    TierAppear Probability;

	private void Start()
	{
		// ���� ���� Ȯ�� �ʱ�ȭ
		Probability = new TierAppear();

		// ���� ����
		resetShop();
	}

	/// <summary>
	/// ���� ���� �Լ�
	/// </summary>
    public void resetShop()
    {
		// ������ ���� ���� Ȯ�� Ȯ��
		setAppearToLevel();

		// �⹰ ��ġ
		for (int i = 0; i < 5; i++)
        {
			// 0~99 ������ ���ڸ� ���Ƿ� ���� (Ƽ��� �Ҵ�� �������� �ٸ�)
			int r = Random.RandomRange(0, 100);

			// ������ ���ڰ� �Ҵ�� Ƽ�� �� �ȿ� ������ ���
			switch(Probability.Tear[r])
			{
				case 1:
					// 1Ƽ�� ���� ������Ʈ Ǯ
					piece[i] = Pool_Instance.setPieceT1();
					break;
				case 2:
					// 2Ƽ�� ���� ������Ʈ Ǯ
					piece[i] = Pool_Instance.setPieceT2();
					break;
				case 3:
					// 3Ƽ�� ���� ������Ʈ Ǯ
					piece[i] = Pool_Instance.setPieceT3();
					break;
				case 4:
					// 4Ƽ�� ���� ������Ʈ Ǯ
					piece[i] = Pool_Instance.setPieceT4();
					break;
				case 5:
					// 5Ƽ�� ���� ������Ʈ Ǯ
					piece[i] = Pool_Instance.setPieceT5();
					break;
			}
		}

		// ��ġ Ȯ���� �⹰�� ���� ������ ���
		settingShop();
	}

	/// <summary>
	/// ��ġ�� �⹰ ���� ��� �Լ�
	/// </summary>
	void settingShop()
	{
		// 5���� �⹰�� ������ ���
		for (int i = 0; i < 5; i++)
		{
			ShopPiece[i].GetComponent<ShopPiece>().SetVis(piece[i]);
		}
	}

	/// <summary>
	/// ������ Ƽ�� ����Ȯ�� Ȯ�� �Լ�
	/// </summary>
	void setAppearToLevel()
    {
		// �÷��̾� ������ ���� Ȯ�� ���ġ
        switch(GameManager.GM_instance._data.Lv)
        {
            case 1:
				// 1���� Ȯ�� (100%, 0%, 0%, 0%, 0%)
                Probability.SetAppear(100, 0, 0, 0, 0);
				break;
			case 2:
				// 2���� Ȯ�� (70%, 30%, 0%, 0%, 0%)
				Probability.SetAppear(70, 100, 0, 0, 0);
				break;
			case 3:
				// 3���� Ȯ�� (60%, 35%, 5%, 0%, 0%)
				Probability.SetAppear(60, 95, 100, 0, 0);
				break;
			case 4:
				// 4���� Ȯ�� (50%, 35%, 15%, 0%, 0%)
				Probability.SetAppear(50, 85, 100, 0, 0);
				break;
			case 5:
				// 5���� Ȯ�� (40%, 35%, 18%, 2%, 0%)
				Probability.SetAppear(40, 75, 98, 100, 0);
				break;
			case 6:
				// 6���� Ȯ�� (33%, 30%, 30%, 7%, 0%)
				Probability.SetAppear(33, 63, 93, 100, 0);
				break;
			case 7:
				// 7���� Ȯ�� (30%, 30%, 30%, 10%, 0%)
				Probability.SetAppear(30, 60, 90, 100, 0);
				break;
			case 8:
				// 8���� Ȯ�� (23%, 30%, 30%, 15%, 2%)
				Probability.SetAppear(23, 53, 83, 98, 100);
				break;
			case 9:
				// 9���� Ȯ�� (21%, 30%, 25%, 20%, 4%)
				Probability.SetAppear(21, 51, 76, 96, 100);
				break;
			case 10:
				// 10���� Ȯ�� (19%, 25%, 25%, 25%, 6%)
				Probability.SetAppear(19, 44, 69, 94, 100);
				break;
		}
    }
}
