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
	// 플레이어 웨이팅 피스 리스트
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
		// 진행시간 감소
		curTime += Time.deltaTime;
		int time = 0;

		// 게임 상태에 따라 게임 진행 카운트 변경
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
		// 진행시간 텍스트 연동
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
	/// 준비단계 함수
	/// </summary>
	void Preparing()
	{
		_data.Round++;

		BM_instance.resetBoard();

		AI_instance.ActToRound(_data.Round);

		
		TM_instance.RoundText(_data.Round);

		// 경험치 1 증가
		_data.EXP += 1;
		// 레벨업 여부 체크
		levelUp();

		// 상점 리롤
		ShopUI.SetActive(true);
		ShopUI.GetComponentInChildren<Shop>().resetShop();

		// 이자 계산에 맞춰서 골드 획득
		getGold();

		Debug.Log("준비");

		// 현재 진행시간 초기화
		curTime = 0;

		// 플레이어의 모든 기물들을 준비단계 상태로
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].Preparing();
		}

		// 게임 상태를 준비 단계 상태로 변경
		_GameState = GameState.Prepare;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// 전투 준비 함수
	/// </summary>
	void Battle()
	{
		Debug.Log("전장배치");
		
		// 아군 유닛 최초 배치상태 저장
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].Fun_oriPos = BM_instance._Friends[i].nowBoard.getBoardNum();
		}

		// 에너미 유닛 최초 배치상태 저장
		AI_instance.ActToBattle();

		// 에너미 유닛 전장에 배치
		setBattleField();

		// 현재 진행시간 초기화
		curTime = 0;

		_GameState = GameState.Start;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// 에너미 전장 배치 함수
	/// </summary>
	void setBattleField()
	{
		// 크립 라운드가 아닐 때
		if (AI_instance._cripList.Count <= 0)
		{
			// 에너미 리스트 안의 모든 적들의 위치정보를 확인
			for (int i = 0; i < AI_instance._enemyList.Count; i++)
			{
				ChessPiece script = AI_instance._enemyList[i].GetComponent<ChessPiece>();

				// 위치에 따라 에너미들을 보드에 배치
				script.InBoard(script.nowBoard.getBoardNum(), script.Type);

				// 에너미들 물리적 위치를 보드에 배치
				AI_instance._enemyList[i].transform.position = script.nowBoard.transform.position;
			}
		}
		// 크립 라운드 일 때
		else
		{
			// 에너미 리스트 안의 모든 적들의 위치정보를 확인
			for (int i = 0; i < AI_instance._cripList.Count; i++)
			{
				ChessPiece script = AI_instance._cripList[i].GetComponent<ChessPiece>();

				// 위치에 따라 에너미들을 보드에 배치
				script.InBoard(script.nowBoard.getBoardNum(), script.Type);

				// 에너미들 물리적 위치를 보드에 배치
				AI_instance._cripList[i].transform.position = script.nowBoard.transform.position;
			}
		}
	}

	/// <summary>
	/// 전투
	/// </summary>
	void BattleStart()
	{
		BM_instance.activeSynergy();

		Debug.Log("전투시작");

		// 현재 진행시간 초기화
		curTime = 0;

		// 모든 아군 기물들을 전투 상태로 변경
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].BattleStart();
		}

		// 만약 필드 위에 아군 기물이 하나도 없을 시
		if (BM_instance._Friends.Count <= 0)
		{
			// 배틀 종료
			_GameState = GameState.End;
			TM_instance.StateText(_GameState);

			return;
		}

		// 모든 에너미 기물들을 전투 상태로 변경
		for (int i = 0; i < BM_instance._Enemies.Count; i++)
		{
			BM_instance._Enemies[i].BattleStart();
		}

		// 만약 필드 위에 에너미 기물이 하나도 없을 시
		if (BM_instance._Enemies.Count <= 0)
		{
			// 배틀 종료
			_GameState = GameState.End;
			TM_instance.StateText(_GameState);

			return;
		}

		_GameState = GameState.Battle;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// 전투 종료 함수
	/// </summary>
	public void BattleEnd()
	{
		Debug.Log("전투종료");

		// 데드 카운트
		int deadCount = 0;

		if (BM_instance._Friends.Count > 0)
		{
			// 현재 아군들의 상태를 검사
			for (int i = 0; i < BM_instance._Friends.Count; i++)
			{
				// 아군 기물이 죽을 수록 카운터 증가
				if (BM_instance._Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
					deadCount++;

				// 리스트 내의 모든 아군 기물이 죽었다면 패배
				if (BM_instance._Friends.Count - 1 == deadCount)
				{
					// 연승 중인 상태일 때
					if (_data.Win > 0)
						// 초기화
						_data.Win = 0;
					// 연패 중이거나 승점이 0일 때
					else
						//패배 누적
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
				// 리스트 내의 모든 아군 기물이 하나라도 존재한다면
				else
				{
					// 연패 중인 상태일 때
					if (_data.Win < 0)
						// 초기화
						_data.Win = 0;
					// 연승 중이거나 승점이 0일 때
					else
						// 승리 누적
						_data.Win++;

					// 승리 시 1골드 즉시 지불
					_data.Gold++;
					TM_instance.GoldText(_data.Gold++);
				}
			}
		}
		else
		{
			// 연승 중인 상태일 때
			if (_data.Win > 0)
				// 초기화
				_data.Win = 0;
			// 연패 중이거나 승점이 0일 때
			else
				//패배 누적
				_data.Win--;
		}

		// 현재 진행시간 초기화
		curTime = 0;

		//리스트 내의 모든 아군들의 상태를 전투 종료 상태로
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			BM_instance._Friends[i].BattleEnd();
		}
		//리스트 내의 모든 에너미들의 상태를 전투 종료 상태로
		for (int i = 0; i < BM_instance._Enemies.Count; i++)
		{
			BM_instance._Enemies[i].BattleEnd();
		}

		_GameState = GameState.End;
		TM_instance.StateText(_GameState);
	}

	/// <summary>
	/// 상점 열고 닫는 함수
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
	/// 상점 열고 닫는 함수
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
	/// 상점에서 피스 구매
	/// </summary>
	/// <param name="name">구매할 피스</param>
	public void BuyPiece(ChessPiece.PieceName name)
	{
		// 대기석과 골드에 여유가 있다면 피스 구매
		GameObject piece = Pool_Instance.poolPiece(name, ChessPiece.CripName.None, PieceType.Friend);

		//  대기석 체크
		for (int i = 1; i < 10; i++)
		{
			// 현재 체크하는 웨이팅 보드
			int num = 90 + i;
			Board board = GameObject.Find("WaitingBoard" + num).GetComponent<Board>();
			
			// 빈 보드를 찾아
			if (!board.getFriend())
			{
				// 피스를 위치시킨다
				piece.transform.position = board.gameObject.transform.position;
				board.setFriend(true, piece);
				piece.GetComponent<ChessPiece>().
					InWaitBoard(board.getBoardNum(), piece.GetComponent<ChessPiece>().Type);

				// 가격만큼 현재 소지금 감소
				int price = PiecePrice(name);
				_data.Gold -= price;

				TM_instance.GoldText(_data.Gold);

				piece.SetActive(true);

				// 1성 승급 체크(1 -> 2)
				StarUpgradeOne(name);

				break;
			}
		}

	}

	/// <summary>
	/// 상점 구매 골드 체크
	/// </summary>
	/// <param name="name">구매하려는 기물</param>
	/// <returns>구매 가능 여부</returns>
	public bool GoldCheck(ChessPiece.PieceName name)
	{
		int price = PiecePrice(name);

		if (_data.Gold < price)
			return false;

		return true;
	}

	/// <summary>
	/// 상점 가격 책정
	/// </summary>
	/// <param name="name">책정 측정할 기물</param>
	/// <returns>가격</returns>
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
	/// 상점 구매 대기석 체크
	/// </summary>
	/// <returns>구매 가능 여부</returns>
	public bool BoardCheck()
	{
		if (_waitFriends.Count > 8)
			return false;
		else
			return true;
	}

	/// <summary>
	/// 성 승급 확인 함수 (1 -> 2)
	/// </summary>
	/// <param name="name">확인할 종류의 기물</param>
	void StarUpgradeOne(ChessPiece.PieceName name)
	{
		List<ChessPiece> list = new List<ChessPiece>();

		// 필드의 기물을 확인
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			// 필드의 기물 중 구매한 기물과 같은 종류, 같은 성인 기물이 있을 때
			if (BM_instance._Friends[i]._Name == name &&
				BM_instance._Friends[i].Star == Pieces<ChessPiece>.PieceStar.oneStar)
			{
				// 승급 리스트에 추가
				list.Add(BM_instance._Friends[i]);
			}
		}
		// 대기석의 기물을 확인
		for (int i = 0; i < GM_instance._waitFriends.Count; i++)
		{
			// 대기석의 기물 중 구매한 기물과 같은 종류, 같은 성인 기물이 있을 때
			if (GM_instance._waitFriends[i]._Name == name &&
				GM_instance._waitFriends[i].Star == Pieces<ChessPiece>.PieceStar.oneStar)
			{
				// 승급 리스트에 추가
				list.Add(GM_instance._waitFriends[i]);
			}
		}

		// 승급 카운터가 충분하지 않으면 함수 탈출
		if (list.Count < 3)
			return;

		// 승급 카운터가 충분할 시
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[0].inStorage(list[i]);

			if (list[i].nowBoard.getBoardNum() / 10 >= 9)
			{
				// 보드판에서 기물 제거
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutWaitBoard(list[i].Type);
			}
			else
			{
				// 보드판에서 기물 제거
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutBoard(list[i].Type);
			}
			list[i].gameObject.SetActive(false);
		}

		// 제일 먼저 대기석에 자리한 기물을 기준으로 승급
		list[0].StarSet(Pieces<ChessPiece>.PieceStar.twoStar);

		// 2성 기물이 추가됐으니 2성 기준으로 승급 체크 시작
		// 2성 승급 체크 (2 -> 3)
		StarUpgradeTwo(name);
	}
	/// <summary>
	/// 성 승급 확인 함수 (2 -> 3)
	/// </summary>
	/// <param name="name">확인할 종류의 기물</param>
	void StarUpgradeTwo(ChessPiece.PieceName name)
	{
		List<ChessPiece> list = new List<ChessPiece>();

		// 필드의 기물을 확인
		for (int i = 0; i < BM_instance._Friends.Count; i++)
		{
			// 필드의 기물 중 구매한 기물과 같은 종류, 같은 성인 기물이 있을 때
			if (BM_instance._Friends[i]._Name == name &&
				BM_instance._Friends[i].Star == Pieces<ChessPiece>.PieceStar.twoStar)
			{
				// 승급 리스트에 추가
				list.Add(BM_instance._Friends[i]);
			}
		}
		// 대기석의 기물을 확인
		for (int i = 0; i < GM_instance._waitFriends.Count; i++)
		{
			// 대기석의 기물 중 구매한 기물과 같은 종류, 같은 성인 기물이 있을 때
			if (GM_instance._waitFriends[i]._Name == name &&
				GM_instance._waitFriends[i].Star == Pieces<ChessPiece>.PieceStar.twoStar)
			{
				// 승급 리스트에 추가
				list.Add(GM_instance._waitFriends[i]);
			}
		}

		// 승급 카운터가 충분하지 않으면 함수 탈출
		if (list.Count < 3)
			return;

		// 승급 카운터가 충분할 시
		for (int i = list.Count - 1; i > 0; i--)
		{
			list[0].inStorage(list[i]);
			for (int j = 0; j < list[i].storageCount(); i++)
			{
				list[0].inStorage(list[i].giveStorage(j));
			}

			if (list[i].nowBoard.getBoardNum() / 10 >= 9)
			{
				// 보드판에서 기물 제거
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutWaitBoard(list[i].Type);
			}
			else
			{
				// 보드판에서 기물 제거
				list[i].nowBoard.setFriend(false, list[i].gameObject);
				list[i].OutBoard(list[i].Type);
			}

			list[i].gameObject.SetActive(false);
		}

		// 제일 먼저 대기석에 자리한 기물을 기준으로 승급
		list[0].StarSet(Pieces<ChessPiece>.PieceStar.threeStar);

		// 2성 기물이 추가됐으니 2성 기준으로 승급 체크 시작
		// 2성 승급 체크 (2 -> 3)
		StarUpgradeTwo(name);
	}

	/// <summary>
	/// 라운드 당 골드 획득 함수
	/// </summary>
	void getGold()
	{
		// 라운드가 3 이후일 때
		if (_data.Round > 3)
		{
			// 라운드 당 골드 5
			int gold = 5;

			// 현재 소지금이 10 단위로 끊어질 때
			if (_data.Gold / 10 > 0)
			{
				// 현재 소지금의 10 단위를 기준으로 이자 지급
				float interest = _data.Gold / 10;
				// 이자는 0~5원 한정
				Mathf.Clamp((float)interest, 0, 5);
				gold += (int)interest;
			}
			// 현재 연승 중일 때
			if (_data.Win > 0)
			{
				// 연승 횟수를 기준으로 이자 지급
				float interest = _data.Win;
				// 이자는 3/5/7 기준으로 최대 3까지 지급
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
			// 현재 연패 중일 때
			else if (_data.Win < 0)
			{
				// 연패 횟수를 기준으로 이자 지급
				float interest = -_data.Win;
				// 이자는 3/5/7 기준으로 최대 3까지 지급
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

			// 계산한 총 골드를 현재 골드에 추가한다
			_data.Gold += gold;

			TM_instance.GoldText(_data.Gold);
		}
		// 라운드가 3 이전일 때
		else if (_data.Round <= 3)
		{
			// 라운드 당 골드
			int gold = 0;

			// 라운드가 2라운드일 때 2골드
			if (_data.Round == 2)
			{
				gold = 2;
			}
			// 라운드가 3라운드일 때 4골드
			else if (_data.Round == 3)
			{
				gold = 4;
			}

			// 현재 소지금이 10 단위로 끊어질 때
			if (_data.Gold / 10 > 0)
			{
				// 현재 소지금의 10 단위를 기준으로 이자 지급
				float interest = _data.Gold / 10;
				// 이자는 0~5원 한정
				Mathf.Clamp((float)interest, 0, 5);
				gold += (int)interest;
			}

			_data.Gold += gold;

			TM_instance.GoldText(_data.Gold);
		}
	}

	// 경험치 구매
	public void buyEXP()
	{
		// 레벨이 10 이하 일때 경험치 구매 가능
		if (_data.Lv < 10)
		{
			_data.Gold -= 4;
			_data.EXP += 4;
		}

		// 경험치 구매 후 경험치 확인
		levelUp();
	}

	/// <summary>
	/// 레벨업 재귀함수
	/// </summary>
	void levelUp()
	{
		// 현재 플레이어의 레벨을 기준으로 경험치 계산
		switch(_data.Lv)
		{
			case 1:
				// lv1. 경험치가 1이상 일 때
				if (_data.EXP >= 1)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 2;
					_data.EXP -= 1;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 2:
				// lv2. 경험치가 1이상 일 때
				if (_data.EXP >= 1)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 3;
					_data.EXP -= 1;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 3:
				// lv3. 경험치가 2이상 일 때
				if (_data.EXP >= 2)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 4;
					_data.EXP -= 2;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 4:
				// lv4. 경험치가 4이상 일 때
				if (_data.EXP >= 4)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 5;
					_data.EXP -= 4;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 5:
				// lv5. 경험치가 8이상 일 때
				if (_data.EXP >= 8)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 6;
					_data.EXP -= 8;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 6:
				// lv6. 경험치가 16이상 일 때
				if (_data.EXP >= 16)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 7;
					_data.EXP -= 16;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 7:
				// lv7. 경험치가 24이상 일 때
				if (_data.EXP >= 24)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 8;
					_data.EXP -= 24;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 8:
				// lv8. 경험치가 36이상 일 때
				if (_data.EXP >= 36)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 9;
					_data.EXP -= 32;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 9:
				// lv9. 경험치가 48이상 일 때
				if (_data.EXP >= 48)
				{
					// 레벨 상승 후 경험치 삭제
					_data.Lv = 10;
					_data.EXP -= 48;
					// 혹시나 남는 경험치가 있을 수도 있기에 레벨업 재호출
					levelUp();
				}
				break;
			case 10:
				// lv10. 더 이상 레벨업이 불가능
				break;
		}

		TM_instance.LevelText(_data.Lv);
		TM_instance.ExpText(_data.Lv, _data.EXP);
	}
}
