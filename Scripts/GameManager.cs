using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecePooling;
using static BoardManager;
using static TextManager;
using UnityEngine.Rendering;
using static GameManager;
using Unity.VisualScripting;
using static AIplayer;

public class GameManager : MonoBehaviour
{
	// �÷��̾� ������ �ǽ� ����Ʈ
	public List<ChessPiece> _waitFriends;

	public struct PlayerData
	{
		public int Lv;
		public int EXP;
		public int HP;
		public int Gold;
		public int Win;
		public int Round;
	}

	public PlayerData _data = new PlayerData();

	static public GameManager GM_instance = null;

	public GameObject ShopUI;

	float curTime;

	float battleTime = 61.0f;
	float startTime = 6.0f;
	float EndTime = 6.0f;
	float prepareTime = 31.0f;

	public enum GameState
	{
		Prepare,
		Start,
		Battle,
		End
	}

	GameState _GameState;

	private void Awake()
	{
		if (GM_instance == null)
			GM_instance = this;
	}

	private void Start()
	{
		_data.Lv = 9;
		_data.HP = 100;
		_data.EXP = 0;
		_data.Gold = 112341235;
		_data.Round = 1;

		_GameState = GameState.Prepare;

		TM_instance.StateText(_GameState);
		TM_instance.LevelText(_data.Lv);
		TM_instance.ExpText(_data.Lv, _data.EXP);
		TM_instance.HpText(_data.HP);
		TM_instance.GoldText(_data.Gold);
		TM_instance.RoundText(_data.Round);
		
		AI_instance.ActToRound(_data.Round);
	}

	void Update()
    {
		// ����ð� ����
		curTime += Time.deltaTime;
		int time = 0;

		// ���� ���¿� ���� ���� ���� ī��Ʈ ����
		switch(_GameState)
		{
			case GameState.Prepare:
				time = (int)(prepareTime - curTime);
				break;
			case GameState.Start:
				time = (int)(startTime - curTime);
				break;
			case GameState.Battle:
				time = (int)(battleTime - curTime);
				break;
			case GameState.End:
				time = (int)(EndTime - curTime);
				break;
		}
		// ����ð� �ؽ�Ʈ ����
		TM_instance.TimeText(time);

		if (curTime > prepareTime && _GameState == GameState.Prepare)
			Battle();

		if (curTime > startTime && _GameState == GameState.Start)
			BattleStart();

		if (curTime > battleTime && _GameState == GameState.Battle)
			BattleEnd();

		if (curTime > EndTime && _GameState == GameState.End)
			Preparing();

		/*
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ShopKeyOnOff();
		}
		*/

		if (ShopUI.activeSelf && Input.GetKeyDown(KeyCode.D))
		{
			ShopUI.GetComponentInChildren<Shop>().resetShop();
		}
		
		if (_GameState == GameState.Prepare)
		{
			if (BM_instance._PieceData != null && Input.GetKeyDown(KeyCode.E))
			{
				BM_instance._PieceData.SellPiece();
			}
		}
	}

	/// <summary>
	/// �غ�ܰ� �Լ�
	/// </summary>
	void Preparing()
	{
		_data.Round++;

		BM_instance.resetBoard();

		AI_instance.ActToRound(_data.Round);

		
		TM_instance.RoundText(_data.Round);

		// ����ġ 1 ����
		_data.EXP += 1;
		// ������ ���� üũ
		levelUp();

		// ���� ����
		ShopUI.SetActive(true);
		ShopUI.GetComponentInChildren<Shop>().resetShop();

		// ���� ��꿡 ���缭 ��� ȹ��
		getGold();

		Debug.Log("�غ�");

		// ���� ����ð� �ʱ�ȭ
		curTime = 0;

		// �÷��̾��� ��� �⹰���� �غ�ܰ� ���·�
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].Preparing();
		}

		// ���� ���¸� �غ� �ܰ� ���·� ����
		_GameState = GameState.Prepare;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// ���� �غ� �Լ�
	/// </summary>
	void Battle()
	{
		Debug.Log("�����ġ");
		
		// �Ʊ� ���� ���� ��ġ���� ����
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].Fun_oriPos = BM_instance._Friends[i].nowBoard.getBoardNum();
		}

		// ���ʹ� ���� ���� ��ġ���� ����
		AI_instance.ActToBattle();

		// ���ʹ� ���� ���忡 ��ġ
		setBattleField();

		// ���� ����ð� �ʱ�ȭ
		curTime = 0;

		_GameState = GameState.Start;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// ���ʹ� ���� ��ġ �Լ�
	/// </summary>
	void setBattleField()
	{
		// ũ�� ���尡 �ƴ� ��
		if (AI_instance._cripList.Count <= 0)
		{
			// ���ʹ� ����Ʈ ���� ��� ������ ��ġ������ Ȯ��
			for (int i = 0; i < AI_instance._enemyList.Count; i++)
			{
				ChessPiece script = AI_instance._enemyList[i].GetComponent<ChessPiece>();

				// ��ġ�� ���� ���ʹ̵��� ���忡 ��ġ
				script.InBoard(script.nowBoard.getBoardNum(), script.Type);

				// ���ʹ̵� ������ ��ġ�� ���忡 ��ġ
				AI_instance._enemyList[i].transform.position = script.nowBoard.transform.position;
			}
		}
		// ũ�� ���� �� ��
		else
		{
			// ���ʹ� ����Ʈ ���� ��� ������ ��ġ������ Ȯ��
			for (int i = 0; i < AI_instance._cripList.Count; i++)
			{
				ChessPiece script = AI_instance._cripList[i].GetComponent<ChessPiece>();

				// ��ġ�� ���� ���ʹ̵��� ���忡 ��ġ
				script.InBoard(script.nowBoard.getBoardNum(), script.Type);

				// ���ʹ̵� ������ ��ġ�� ���忡 ��ġ
				AI_instance._cripList[i].transform.position = script.nowBoard.transform.position;
			}
		}
	}

	/// <summary>
	/// ����
	/// </summary>
	void BattleStart()
	{
		BM_instance.activeSynergy();

		Debug.Log("��������");

		// ���� ����ð� �ʱ�ȭ
		curTime = 0;

		// ��� �Ʊ� �⹰���� ���� ���·� ����
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].BattleStart();
		}

		// ���� �ʵ� ���� �Ʊ� �⹰�� �ϳ��� ���� ��
		if (BM_instance._Friends.Count <= 0)
		{
			// ��Ʋ ����
			_GameState = GameState.End;
			TM_instance.StateText(_GameState);

			return;
		}

		// ��� ���ʹ� �⹰���� ���� ���·� ����
		for (int i = 0; i < BM_instance._Enemies.Count; i++)
		{
			BM_instance._Enemies[i].BattleStart();
		}

		// ���� �ʵ� ���� ���ʹ� �⹰�� �ϳ��� ���� ��
		if (BM_instance._Enemies.Count <= 0)
		{
			// ��Ʋ ����
			_GameState = GameState.End;
			TM_instance.StateText(_GameState);

			return;
		}

		_GameState = GameState.Battle;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// ���� ���� �Լ�
	/// </summary>
	public void BattleEnd()
	{
		Debug.Log("��������");

		// ���� ī��Ʈ
		int deadCount = 0;

		if (BM_instance._Friends.Count > 0)
		{
			// ���� �Ʊ����� ���¸� �˻�
			for (int i = 0; i < BM_instance._Friends.Count; i++)
			{
				// �Ʊ� �⹰�� ���� ���� ī���� ����
				if (BM_instance._Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
					deadCount++;

				// ����Ʈ ���� ��� �Ʊ� �⹰�� �׾��ٸ� �й�
				if (BM_instance._Friends.Count - 1 == deadCount)
				{
					// ���� ���� ������ ��
					if (_data.Win > 0)
						// �ʱ�ȭ
						_data.Win = 0;
					// ���� ���̰ų� ������ 0�� ��
					else
						//�й� ����
						_data.Win--;

					int damage = 0;

					foreach(var piece in BM_instance._Enemies)
					{
						if (piece._State == Pieces<ChessPiece>.NowState.DEAD)
							continue;

						if (piece._Star == Pieces<ChessPiece>.PieceStar.oneStar)
							damage += 1;
						else if (piece._Star == Pieces<ChessPiece>.PieceStar.twoStar)
							damage += 3;
						else if (piece._Star == Pieces<ChessPiece>.PieceStar.threeStar)
							damage += 5;
					}

					_data.HP -= damage;
					TM_instance.HpText(_data.HP);
				}
				// ����Ʈ ���� ��� �Ʊ� �⹰�� �ϳ��� �����Ѵٸ�
				else
				{
					// ���� ���� ������ ��
					if (_data.Win < 0)
						// �ʱ�ȭ
						_data.Win = 0;
					// ���� ���̰ų� ������ 0�� ��
					else
						// �¸� ����
						_data.Win++;

					// �¸� �� 1��� ��� ����
					_data.Gold++;
					TM_instance.GoldText(_data.Gold++);
				}
			}
		}
		else
		{
			// ���� ���� ������ ��
			if (_data.Win > 0)
				// �ʱ�ȭ
				_data.Win = 0;
			// ���� ���̰ų� ������ 0�� ��
			else
				//�й� ����
				_data.Win--;
		}

		// ���� ����ð� �ʱ�ȭ
		curTime = 0;

		//����Ʈ ���� ��� �Ʊ����� ���¸� ���� ���� ���·�
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].BattleEnd();
		}
		//����Ʈ ���� ��� ���ʹ̵��� ���¸� ���� ���� ���·�
		for (int i = 0; i < BM_instance._Enemies.Count; i++)
		{
			BM_instance._Enemies[i].BattleEnd();
		}

		_GameState = GameState.End;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// ���� ���� �ݴ� �Լ�
	/// </summary>
	public void ShopOnOff()
	{
		if (ShopUI.activeSelf)
		{
			ShopUI.SetActive(false);
		}
		else
		{
			ShopUI.SetActive(true);
		}
	}
	
	/// <summary>
	/// ���� ���� �ݴ� �Լ�
	/// </summary>
	void ShopKeyOnOff()
	{
		if (ShopUI.activeSelf)
		{
			ShopUI.SetActive(false);
		}
		else
		{
			ShopUI.SetActive(true);
		}
	}

	/// <summary>
	/// �������� �ǽ� ����
	/// </summary>
	/// <param name="name">������ �ǽ�</param>
	public void BuyPiece(ChessPiece.PieceName name)
	{
		// ��⼮�� ��忡 ������ �ִٸ� �ǽ� ����
		GameObject piece = Pool_Instance.poolPiece(name, ChessPiece.CripName.None, PieceType.Friend);

		//  ��⼮ üũ
		for (int i = 1; i < 10; i++)
		{
			// ���� üũ�ϴ� ������ ����
			int num = 90 + i;
			Board board = GameObject.Find("WaitingBoard" + num).GetComponent<Board>();
			
			// �� ���带 ã��
			if (!board.getFriend())
			{
				// �ǽ��� ��ġ��Ų��
				piece.transform.position = board.gameObject.transform.position;
				board.setFriend(true, piece);
				piece.GetComponent<ChessPiece>().
					InWaitBoard(board.getBoardNum(), piece.GetComponent<ChessPiece>().Type);

				// ���ݸ�ŭ ���� ������ ����
				int price = PiecePrice(name);
				_data.Gold -= price;

				TM_instance.GoldText(_data.Gold);

				piece.SetActive(true);

				// 1�� �±� üũ(1 -> 2)
				StarUpgradeOne(name);

				break;
			}
		}

	}

	/// <summary>
	/// ���� ���� ��� üũ
	/// </summary>
	/// <param name="name">�����Ϸ��� �⹰</param>
	/// <returns>���� ���� ����</returns>
	public bool GoldCheck(ChessPiece.PieceName name)
	{
		int price = PiecePrice(name);

		if (_data.Gold < price)
			return false;

		return true;
	}

	/// <summary>
	/// ���� ���� å��
	/// </summary>
	/// <param name="name">å�� ������ �⹰</param>
	/// <returns>����</returns>
	public int PiecePrice(ChessPiece.PieceName name)
	{
		int price = 0;

		switch (name)
		{
			case ChessPiece.PieceName.PootMan:
				price = 1;
				break;
			case ChessPiece.PieceName.SkelWarrior:
				price = 1;
				break;
			case ChessPiece.PieceName.BeeKnghit:
				price = 1;
				break;
			case ChessPiece.PieceName.DemonGuard:
				price = 1;
				break;
			case ChessPiece.PieceName.TraineeWizard:
				price = 1;
				break;
			case ChessPiece.PieceName.Archer:
				price = 2;
				break;
			case ChessPiece.PieceName.AxeWarrior:
				price = 2;
				break;
			case ChessPiece.PieceName.UndeadMage:
				price = 2;
				break;
			case ChessPiece.PieceName.EvilEye:
				price = 2;
				break;
			case ChessPiece.PieceName.PhantomKnight:
				price = 2;
				break;
			case ChessPiece.PieceName.BesatMan:
				price = 3;
				break;
			case ChessPiece.PieceName.Berserker:
				price = 3;
				break;
			case ChessPiece.PieceName.Imp:
				price = 3;
				break;
			case ChessPiece.PieceName.MetalKnight:
				price = 3;
				break;
			case ChessPiece.PieceName.OrcWarrior:
				price = 3;
				break;
			case ChessPiece.PieceName.DeathKnight:
				price = 4;
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				price = 4;
				break;
			case ChessPiece.PieceName.Druid:
				price = 4;
				break;
			case ChessPiece.PieceName.ElvenKnight:
				price = 4;
				break;
			case ChessPiece.PieceName.BloodLord:
				price = 5;
				break;
			case ChessPiece.PieceName.Golem:
				price = 5;
				break;
			case ChessPiece.PieceName.MagicShooter:
				price = 5;
				break;
		}

		return price;
	}

	/// <summary>
	/// ���� ���� ��⼮ üũ
	/// </summary>
	/// <returns>���� ���� ����</returns>
	public bool BoardCheck()
	{
		if (_waitFriends.Count > 8)
			return false;
		else
			return true;
	}

	/// <summary>
	/// �� �±� Ȯ�� �Լ� (1 -> 2)
	/// </summary>
	/// <param name="name">Ȯ���� ������ �⹰</param>
	void StarUpgradeOne(ChessPiece.PieceName name)
	{
		List<ChessPiece> list = new List<ChessPiece>();

		// �ʵ��� �⹰�� Ȯ��
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			// �ʵ��� �⹰ �� ������ �⹰�� ���� ����, ���� ���� �⹰�� ���� ��
			if (BM_instance._Friends[i]._Name == name &&
				BM_instance._Friends[i].Star == Pieces<ChessPiece>.PieceStar.oneStar)
			{
				// �±� ����Ʈ�� �߰�
				list.Add(BM_instance._Friends[i]);
			}
		}
		// ��⼮�� �⹰�� Ȯ��
		for (int i = 0; i < GM_instance._waitFriends.Count; i++)
		{
			// ��⼮�� �⹰ �� ������ �⹰�� ���� ����, ���� ���� �⹰�� ���� ��
			if (GM_instance._waitFriends[i]._Name == name &&
				GM_instance._waitFriends[i].Star == Pieces<ChessPiece>.PieceStar.oneStar)
			{
				// �±� ����Ʈ�� �߰�
				list.Add(GM_instance._waitFriends[i]);
			}
		}

		// �±� ī���Ͱ� ������� ������ �Լ� Ż��
		if (list.Count < 3)
			return;

		// �±� ī���Ͱ� ����� ��
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[0].inStorage(list[i]);

			if (list[i].nowBoard.getBoardNum() / 10 >= 9)
			{
				// �����ǿ��� �⹰ ����
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutWaitBoard(list[i].Type);
			}
			else
			{
				// �����ǿ��� �⹰ ����
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutBoard(list[i].Type);
			}
			list[i].gameObject.SetActive(false);
		}

		// ���� ���� ��⼮�� �ڸ��� �⹰�� �������� �±�
		list[0].StarSet(Pieces<ChessPiece>.PieceStar.twoStar);

		// 2�� �⹰�� �߰������� 2�� �������� �±� üũ ����
		// 2�� �±� üũ (2 -> 3)
		StarUpgradeTwo(name);
	}
	/// <summary>
	/// �� �±� Ȯ�� �Լ� (2 -> 3)
	/// </summary>
	/// <param name="name">Ȯ���� ������ �⹰</param>
	void StarUpgradeTwo(ChessPiece.PieceName name)
	{
		List<ChessPiece> list = new List<ChessPiece>();

		// �ʵ��� �⹰�� Ȯ��
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			// �ʵ��� �⹰ �� ������ �⹰�� ���� ����, ���� ���� �⹰�� ���� ��
			if (BM_instance._Friends[i]._Name == name &&
				BM_instance._Friends[i].Star == Pieces<ChessPiece>.PieceStar.twoStar)
			{
				// �±� ����Ʈ�� �߰�
				list.Add(BM_instance._Friends[i]);
			}
		}
		// ��⼮�� �⹰�� Ȯ��
		for (int i = 0; i < GM_instance._waitFriends.Count; i++)
		{
			// ��⼮�� �⹰ �� ������ �⹰�� ���� ����, ���� ���� �⹰�� ���� ��
			if (GM_instance._waitFriends[i]._Name == name &&
				GM_instance._waitFriends[i].Star == Pieces<ChessPiece>.PieceStar.twoStar)
			{
				// �±� ����Ʈ�� �߰�
				list.Add(GM_instance._waitFriends[i]);
			}
		}

		// �±� ī���Ͱ� ������� ������ �Լ� Ż��
		if (list.Count < 3)
			return;

		// �±� ī���Ͱ� ����� ��
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[0].inStorage(list[i]);
			for (int j = 0; j < list[i].storageCount(); i++)
			{
				list[0].inStorage(list[i].giveStorage(j));
			}

			if (list[i].nowBoard.getBoardNum() / 10 >= 9)
			{
				// �����ǿ��� �⹰ ����
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutWaitBoard(list[i].Type);
			}
			else
			{
				// �����ǿ��� �⹰ ����
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutBoard(list[i].Type);
			}

			list[i].gameObject.SetActive(false);
		}

		// ���� ���� ��⼮�� �ڸ��� �⹰�� �������� �±�
		list[0].StarSet(Pieces<ChessPiece>.PieceStar.threeStar);

		// 2�� �⹰�� �߰������� 2�� �������� �±� üũ ����
		// 2�� �±� üũ (2 -> 3)
		StarUpgradeTwo(name);
	}

	/// <summary>
	/// ���� �� ��� ȹ�� �Լ�
	/// </summary>
	void getGold()
	{
		// ���尡 3 ������ ��
		if (_data.Round > 3)
		{
			// ���� �� ��� 5
			int gold = 5;

			// ���� �������� 10 ������ ������ ��
			if (_data.Gold / 10 > 0)
			{
				// ���� �������� 10 ������ �������� ���� ����
				float interest = _data.Gold / 10;
				// ���ڴ� 0~5�� ����
				Mathf.Clamp((float)interest, 0, 5);
				gold += (int)interest;
			}
			// ���� ���� ���� ��
			if (_data.Win > 0)
			{
				// ���� Ƚ���� �������� ���� ����
				float interest = _data.Win;
				// ���ڴ� 3/5/7 �������� �ִ� 3���� ����
				if (3 <= interest && interest < 5)
				{
					interest = 1;
				}
				else if (interest < 7)
				{
					interest = 2;
				}
				else if (7 <= interest)
				{
					interest = 3;
				}
				gold += (int)interest;
			}
			// ���� ���� ���� ��
			else if (_data.Win < 0)
			{
				// ���� Ƚ���� �������� ���� ����
				float interest = -_data.Win;
				// ���ڴ� 3/5/7 �������� �ִ� 3���� ����
				if (3 <= interest && interest < 5)
				{
					interest = 1;
				}
				else if (interest < 7)
				{
					interest = 2;
				}
				else if (7 <= interest)
				{
					interest = 3;
				}
				gold += (int)interest;
			}

			// ����� �� ��带 ���� ��忡 �߰��Ѵ�
			_data.Gold += gold;

			TM_instance.GoldText(_data.Gold);
		}
		// ���尡 3 ������ ��
		else if (_data.Round <= 3)
		{
			// ���� �� ���
			int gold = 0;

			// ���尡 2������ �� 2���
			if (_data.Round == 2)
			{
				gold = 2;
			}
			// ���尡 3������ �� 4���
			else if (_data.Round == 3)
			{
				gold = 4;
			}

			// ���� �������� 10 ������ ������ ��
			if (_data.Gold / 10 > 0)
			{
				// ���� �������� 10 ������ �������� ���� ����
				float interest = _data.Gold / 10;
				// ���ڴ� 0~5�� ����
				Mathf.Clamp((float)interest, 0, 5);
				gold += (int)interest;
			}

			_data.Gold += gold;

			TM_instance.GoldText(_data.Gold);
		}
	}

	// ����ġ ����
	public void buyEXP()
	{
		// ������ 10 ���� �϶� ����ġ ���� ����
		if (_data.Lv < 10)
		{
			_data.Gold -= 4;
			_data.EXP += 4;
		}

		// ����ġ ���� �� ����ġ Ȯ��
		levelUp();
	}

	/// <summary>
	/// ������ ����Լ�
	/// </summary>
	void levelUp()
	{
		// ���� �÷��̾��� ������ �������� ����ġ ���
		switch(_data.Lv)
		{
			case 1:
				// lv1. ����ġ�� 1�̻� �� ��
				if (_data.EXP >= 1)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 2;
					_data.EXP -= 1;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 2:
				// lv2. ����ġ�� 1�̻� �� ��
				if (_data.EXP >= 1)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 3;
					_data.EXP -= 1;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 3:
				// lv3. ����ġ�� 2�̻� �� ��
				if (_data.EXP >= 2)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 4;
					_data.EXP -= 2;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 4:
				// lv4. ����ġ�� 4�̻� �� ��
				if (_data.EXP >= 4)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 5;
					_data.EXP -= 4;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 5:
				// lv5. ����ġ�� 8�̻� �� ��
				if (_data.EXP >= 8)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 6;
					_data.EXP -= 8;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 6:
				// lv6. ����ġ�� 16�̻� �� ��
				if (_data.EXP >= 16)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 7;
					_data.EXP -= 16;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 7:
				// lv7. ����ġ�� 24�̻� �� ��
				if (_data.EXP >= 24)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 8;
					_data.EXP -= 24;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 8:
				// lv8. ����ġ�� 36�̻� �� ��
				if (_data.EXP >= 36)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 9;
					_data.EXP -= 32;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 9:
				// lv9. ����ġ�� 48�̻� �� ��
				if (_data.EXP >= 48)
				{
					// ���� ��� �� ����ġ ����
					_data.Lv = 10;
					_data.EXP -= 48;
					// Ȥ�ó� ���� ����ġ�� ���� ���� �ֱ⿡ ������ ��ȣ��
					levelUp();
				}
				break;
			case 10:
				// lv10. �� �̻� �������� �Ұ���
				break;
		}

		TM_instance.LevelText(_data.Lv);
		TM_instance.ExpText(_data.Lv, _data.EXP);
	}
}
