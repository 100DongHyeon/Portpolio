using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static BoardManager;
using static GameManager;
using static PiecePooling;
using static AIplayer;


// 모든 피스들에 사용할 부모 클래스
public class Pieces<T> : MonoBehaviour where T : MonoBehaviour
{
	#region _공용 열거형 데이터

	// 타입
	[SerializeField]
	PieceType _Type;

	// 시너지
	public enum PieceSynergy
	{
		Steel,
		Nature,
		Undead,
		Demon,
		Warrior,
		Ranger,
		Mage,
		Knight
	}

	// 특성 시너지
	PieceSynergy _Attribute;
	// 직업 시너지
	PieceSynergy _Class;

	// 피스 티어
	public enum PieceTier
	{
		ONE,
		TWO,
		THREE,
		FOUR,
		FIVE
	}

	// 티어
	PieceTier _Tier;

	// 피스 성
	public enum PieceStar
	{
		oneStar,
		twoStar,
		threeStar
	}

	// 현재 등급
	public PieceStar _Star;

	#endregion

	// 현재 상태
	public enum NowState
	{
		START,
		IDLE,
		MOVE,
		BATTLE,
		SKILL,
		DEAD,
		END
	}

	// 기물들 행동 상태
	public NowState _State;

	#region _스테이터스

	// 기본 체력
	float oriHP;
	// 현재 체력
	float curHP;
	// 최대 체력
	float maxHP;

	// 기본 공격력
	float oriAttack;
	// 추가 공격력
	float nowAttack;
	// 스킬 공격력
	float skillAttack;

	// 기본 방어력
	float oriArmor;
	// 추가 방어력
	float nowArmor;

	// 기본 공격 속도
	float oriDelay;
	// 추가 공격 속도
	float nowDelay;

	// 현재 마나
	float curMP;
	// 최대 마나
	float maxMP;
	// 스킬 타임
	float skillTime;
	// 스킬 발동 중
	bool isSkill;
	// 공격 범위
	int atkRange;

	#endregion

	// 현재 보드
	public Board nowBoard;
	// 피스의 원래 배치
	int OriPos;

	// 이동 속도
	float moveSpeed = 3.0f;
	// 이동 중 판별
	public bool isMove;
	// 이동 경로 계산
	[SerializeField]
	Navigation _navi;

	// 애니메이터
	public Animator _anim;

	// 아웃라인
	[SerializeField]
	Outline[] outlines;
	// 임시로 보관할 성 승급 재료 기물
	List<ChessPiece> pieceStorage;

	#region _Get/Set 함수

	// 기본 체력 get/set
	public float Fun_oriHP
	{
		set { oriHP = value; }
		get { return oriHP; }
	}
	// 현재 체력 get/set
	public float Fun_curHP
	{
		set { curHP += value; }
		get { return curHP; }
	}
	// 전체 체력 get/set
	public float Fun_maxHP
	{
		set { maxHP = value; }
		get { return maxHP; }
	}

	// 기본 공격력 get/set
	public float Fun_oriAttack
	{
		set { oriAttack = value; }
		get { return oriAttack; }
	}
	// 현재 공격력 get/set
	public float Fun_nowAttack
	{
		set { nowAttack = oriAttack + value; }
		get { return nowAttack; }
	}
	// 현재 공격력 get/set
	public float Fun_skillAttack
	{
		set { skillAttack = oriAttack + value; }
		get { return skillAttack; }
	}

	// 기본 방어력 get/set
	public float Fun_oriArmor
	{
		set { oriArmor = value; }
		get { return oriArmor; }
	}
	// 현재 방어력 get/set
	public float Fun_nowArmor
	{
		set { nowArmor = oriArmor + value; }
		get { return nowArmor; }
	}

	// 기본 딜레이 get/set
	public float Fun_oriDelay
	{
		set { oriDelay = value; }
		get { return oriDelay; }
	}
	// 현재 딜레이 get/set
	public float Fun_nowDelay
	{
		set { nowDelay = oriDelay + value; }
		get { return nowDelay; }
	}

	// 현재 마력 get/set
	public float Fun_curMP
	{
		set { curMP = value; }
		get { return curMP; }
	}
	// 전체 마력 get/set
	public float Fun_maxMP
	{
		set { maxMP = value; }
		get { return maxMP; }
	}
	// 스킬 타임 get/set
	public float Fun_skillTime
	{
		set { skillTime = value; }
		get { return skillTime; }
	}
	// 스킬 사용 중 get/set
	public void setIsSkill(bool TorF)
	{
		if (TorF)
			isSkill = true;
		else
			isSkill = false;
	}
	public bool getIsSkill()
	{
		return isSkill;
	}
	// 공격범위 get/set
	public int Fun_attackRange
	{
		set { atkRange = value; }
		get { return atkRange; }
	}

	// 유닛 타입 get/set
	public PieceType Type
	{
		set { _Type = value; }
		get { return _Type; }
	}

	// 유닛 티어 get/set
	public PieceTier Tier
	{
		set { _Tier = value; }
		get { return _Tier; }
	}

	// 유닛 성 get/set
	public PieceStar Star
	{
		set { _Star = value; }
		get { return _Star; }
	}

	// 유닛 성 get/set
	public PieceSynergy Attribute
	{
		set { _Attribute = value; }
		get { return _Attribute; }
	}

	// 유닛 성 get/set
	public PieceSynergy Class
	{
		set { _Class = value; }
		get { return _Class; }
	}

	// 스탯 초기화 함수
	public void PieceInit(float hp, float mp, float atk, float amr, float delay, float skillt, 
						  int arange, PieceType type, PieceTier tier, PieceStar star)
	{
		maxHP = hp;
		oriHP = hp;
		curHP = maxHP;
		maxMP = mp;
		curMP = 0;
		oriAttack = atk;
		nowAttack = oriAttack;
		oriArmor = amr;
		nowArmor = oriArmor;
		oriDelay = delay;
		nowDelay = oriDelay;
		skillTime = skillt;
		atkRange = arange;
		_Type = type;
		_Tier = tier;
		_Star = star;
	}

	// 성 승급 함수
	public void StarSet(PieceStar star)
	{
		switch(star)
		{
			case PieceStar.oneStar:
				_Star = PieceStar.oneStar;
				transform.localScale = Vector3.one;
				for (int i = 0; i < outlines.Length; i++)
				{
					outlines[i].outlineWhite();
				}
				break;
			case PieceStar.twoStar:
				_Star = PieceStar.twoStar;
				transform.localScale = Vector3.one * 1.15f;
				for (int i = 0; i < outlines.Length; i++)
				{
					outlines[i].outlineBlue();
				}
				upgradeStar(1.4f);
				break;
			case PieceStar.threeStar:
				_Star = PieceStar.threeStar;
				transform.localScale = Vector3.one * 1.3f;
				for (int i = 0; i < outlines.Length; i++)
				{
					outlines[i].outlineOrange();
				}
				upgradeStar(2f);
				break;
		}
	}

	// 성 강화 함수
	void upgradeStar(float Scale)
	{
		maxHP *= Scale;
		oriAttack *= Scale;
		oriArmor *= Scale;
	}

	// 오리지널 배치 get/set
	public int Fun_oriPos
	{
		set { OriPos = value; }
		get { return OriPos; }
	}

	#endregion

	private void Start()
	{
		_anim = GetComponent<Animator>();
		_navi = GetComponent<Navigation>();
		pieceStorage = new List<ChessPiece>();
	}

	// 스토리지의 리스트 갯수를 반환
	public int storageCount()
	{
		return pieceStorage.Count;
	}
	// 스토리지에서 재료 기물 반환
	public ChessPiece giveStorage(int i)
	{
		return pieceStorage[i];
	}
	// 스토리지에 기물 추가
	public void inStorage(ChessPiece obj)
	{
		pieceStorage.Add(obj);
	}
	// 스토리지 내부의 모든 기물 풀에 반환
	public void returnStorage()
	{
		for (int i = 0; i < pieceStorage.Count; i++)
		{
			// 동일한 기물 제거 (풀에 반환)
			Pool_Instance.resetPieceList(pieceStorage[i]);
			
		}

		pieceStorage.Clear();
	}

	/// <summary>
	/// 기물 배치 함수
	/// </summary>
	/// <param name="num">배치할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void InBoard(int num, PieceType type)
	{
		// 배치할 보드 검색
		string boardName = "FightBoard" + num;

		// 현재 기물에 배치할 보드를 저장
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		switch(type)
		{
			case PieceType.Friend:
				// 해당 보드판에 아군 기물을 배치
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// 보드판의 아군 리스트에 아군 기물을 저장
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
			case PieceType.Enemy:
				// 해당 보드판에 에너미 기물을 배치
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// 보드판의 에너미 리스트에 에너미 기물을 저장
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
			case PieceType.Crip:
				// 해당 보드판에 에너미 기물을 배치
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// 보드판의 에너미 리스트에 에너미 기물을 저장
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
		}
	}

	/// <summary>
	/// 기물 해제 함수
	/// </summary>
	/// <param name="num">해제할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void OutBoard(PieceType type)
	{
		// 해제할 보드 검색
		string boardName = "FightBoard" + nowBoard.getBoardNum();

		// 해당 보드판에 기물을 해제
		BM_instance.setBoard(boardName, false, gameObject, Type);
		// 보드판의 각 리스트에 기물을 삭제
		BM_instance.setList(nowBoard.getBoardNum(),
							GetComponentInChildren<ChessPiece>(), false, Type);

		// 현재 기물에 보드 정보 삭제
		nowBoard = null;
	}

	/// <summary>
	/// 대기 보드 기물 배치 함수
	/// </summary>
	/// <param name="num">배치할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void InWaitBoard(int num, PieceType type)
	{
		// 배치할 보드 검색
		string boardName = "WaitingBoard" + num;

		// 현재 기물에 배치할 보드를 저장
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		// 해당 보드판에 기물을 배치
		BM_instance.setBoard(boardName, true, gameObject, Type);
		// 보드판의 리스트에 기물을 저장
		BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
	}

	/// <summary>
	/// 기물 해제 함수
	/// </summary>
	/// <param name="num">해제할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void OutWaitBoard(PieceType type)
	{
		// 배치할 보드 검색
		string boardName = "WaitingBoard" + nowBoard.getBoardNum();

		// 해당 보드판에 기물을 배치
		BM_instance.setBoard(boardName, false, gameObject, Type);
		// 보드판의 리스트에 기물을 저장
		BM_instance.setList(nowBoard.getBoardNum(),
							GetComponentInChildren<ChessPiece>(), false, Type);

		// 현재 기물에 보드 정보 삭제
		nowBoard = null;
	}

	/// <summary>
	/// 이동시 데이터 변환 함수
	/// </summary>
	/// <param name="now">이전 위치</param>
	/// <param name="next">이동한 위치</param>
	/// <param name="type">기물의 타입</param>
	public void MoveBoard(int next,PieceType type)
	{
		// 해제&배치할 보드 검색
		string nowName = "FightBoard" + nowBoard.getBoardNum();
		string nextName = "FightBoard" + next;

		// 현재 기물에 배치할 보드를 저장
		nowBoard = GameObject.Find("FightBoard" + next).GetComponent<Board>();

		// 이전 보드판에 아군 해제
		BM_instance.setBoard(nowName, false, gameObject, Type);
		// 해당 보드판에 아군 기물을 배치
		BM_instance.setBoard(nextName, true, gameObject, Type);
	}

	/// <summary>
	/// 기물 사망시 데이터 변환 함수
	/// </summary>
	/// <param name="now">현재 위치</param>
	/// <param name="type">기물의 타입</param>
	public void DeadPiece(int now, PieceType type)
	{
		// 배치할 보드 검색
		string nowName = "FightBoard" + nowBoard.getBoardNum();

		// 사망 카운터
		int deadCount = 0;

		switch(type)
		{
			case PieceType.Friend:
				// 해당 보드판에 아군 기물을 해제
				BM_instance.setBoard(nowName, false, gameObject, type);
				// 현재 아군들의 상태를 검사
				for (int i = 0; i < BM_instance._Friends.Count; i++)
				{
					// 아군 기물이 죽을 수록 카운터 증가
					if (BM_instance._Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// 리스트 내의 모든 아군 기물이 죽었다면 배틀 종료
					if (BM_instance._Friends.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
			case PieceType.Enemy:
				// 해당 보드판에 에너미 기물을 해제
				BM_instance.setBoard(nowName, false, gameObject, type);
				// 현재 에너미들의 상태를 검사
				for (int i = 0; i < BM_instance._Enemies.Count; i++)
				{
					// 에너미 기물이 죽을 수록 카운터 증가
					if (BM_instance._Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// 리스트 내의 모든 에너미 기물이 죽었다면 배틀 종료
					if (BM_instance._Enemies.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
			case PieceType.Crip:
				// 해당 보드판에 에너미 기물을 해제
				BM_instance.setBoard(nowName, false, gameObject, type);
				// 현재 에너미들의 상태를 검사
				for (int i = 0; i < BM_instance._Enemies.Count; i++)
				{
					// 에너미 기물이 죽을 수록 카운터 증가
					if (BM_instance._Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// 리스트 내의 모든 에너미 기물이 죽었다면 배틀 종료
					if (BM_instance._Enemies.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
		}

		// 현재 기물에 배치할 보드를 저장
		nowBoard = null;
	}

	/// <summary>
	/// 기물 사망시 부활 함수
	/// </summary>
	/// <param name="now">부활할 위치</param>
	/// <param name="type">기물의 타입</param>
	public void RebornPiece(int num, PieceType type)
	{
		// 배치할 보드 검색
		string boardName = "FightBoard" + num.ToString();

		if (nowBoard != null)
		{
			// 배치할 보드 검색
			string nowName = "FightBoard" + nowBoard.getBoardNum();

			BM_instance.setBoard(nowName, false, gameObject, type);
		}

		// 현재 기물에 배치할 보드를 저장
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		switch (type)
		{
			case PieceType.Friend:
				// 해당 보드판에 아군 기물을 부활
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
			case PieceType.Enemy:
				// 해당 보드판에 에너미 기물을 부활
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
			case PieceType.Crip:
				// 해당 보드판에 에너미 기물을 부활
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
		}
	}

	/// <summary>
	/// 에너미 보드에 기물 배치 함수
	/// </summary>
	/// <param name="num">배치할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void InEnemyBoard(int num)
	{
		// 배치할 보드 검색
		string boardName = "EnemyBoard" + num;

		// 현재 기물에 배치할 보드를 저장
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		transform.position = nowBoard.transform.position;
	}

	/// <summary>
	/// 에너미 보드에 기물 해제 함수
	/// </summary>
	/// <param name="num">해제할 보드</param>
	/// <param name="type">피스의 타입</param>
	public void OutEnemyBoard()
	{	
		// 현재 기물에 보드 정보 삭제
		nowBoard = null;
	}

	/// <summary>
	/// 이동 코루틴
	/// </summary>
	/// <returns></returns>
	public IEnumerator Move(Board piece)
	{
		int DisCheck = RangeCheck(nowBoard.getBoardNum(), piece.getBoardNum());

		if (DisCheck <= atkRange)
		{
			_State = NowState.BATTLE;
			yield break;
		}

		isMove = true;

		int num = _navi.navigation(nowBoard.getBoardNum(), piece.getBoardNum(), atkRange);

		if (num == 0)
		{
			isMove = false;
			_State = NowState.IDLE;
			yield break;
		}

		Board nextPos = GameObject.Find("FightBoard" + num).GetComponent<Board>();
		
		// 이동 좌표
		Vector3 movePos = nextPos.transform.position;

		MoveBoard(nextPos.getBoardNum(), Type);

		_anim.SetTrigger("IsWalk");

		/*
		
		float timer = 0;
		
		// 무브 시작
		while (transform.position.y >= movePos.y)
		{
			// 시간에 따른 포물선 이동
			timer += Time.deltaTime;
			transform.position = Parabola(nowBoard.transform.position, movePos, 2, timer);

			// 코루틴 딜레이 타임
			yield return null;
		}
		*/

		// 무브 시작
		while (true)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos,
				moveSpeed * Time.deltaTime);

			if (transform.position / 1 == movePos / 1)
				break;

			// 코루틴 딜레이 타임
			yield return null;
		}

		_anim.SetTrigger("GoIdle");

		yield return new WaitForSeconds(0.5f);

		transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);

		isMove = false;

		_State = NowState.IDLE;
	}

	/// <summary>
	/// 포물선 이동 함수
	/// </summary>
	/// <param name="start">시작 위치</param>
	/// <param name="end">도착 위치</param>
	/// <param name="height">높이</param>
	/// <param name="t">시간</param>
	/// <returns></returns>
	Vector3 Jump(Vector3 start, Vector3 end, float height, float t)
	{
		Func<float, float> f = x => -3 * height * x * x + 3 * height * x;

		var mid = Vector3.Lerp(start, end, t);

		return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
	}

	/// <summary>
	/// 어택 레인지 체크
	/// </summary>
	/// <param name="now">현재 위치</param>
	/// <param name="target">목표 위치</param>
	int RangeCheck(int now, int target)
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

		return Distance;
	}

	/// <summary>
	/// 보드판 배치 변경 함수
	/// </summary>
	/// <param name="boardNum"> 배치할 보드 정보</param>
	/// <param name="changePice"> 배치할 보드에 기존 피스</param>
	public void ChangeBoard(int boardNum, GameObject changePice = null)
	{
		if (changePice != null)
		{
			// 피스의 스크립터를 가져온다
			ChessPiece script = changePice.GetComponent<ChessPiece>();

			// 피스를 배치&재배치 하려는 보드의 정보를 파악(배틀 보드와 웨이팅 보드 판별)
			if (nowBoard.getBoardNum() / 10 < 9)
			{
				// 위치에 따라 보드를 배치
				// 기존에 보드에 자리잡은 피스를 해제하고 이동시킬 피스의 정보를 저장
				if (boardNum / 10 >= 9)
				{
					// 피스 교체 (교체의 수동체)
					script.OutWaitBoard(script.Type);
					GM_instance._waitFriends.Remove(script);
					script.InBoard(nowBoard.getBoardNum(), script.Type);

					// 피스 교체 (교체의 주체)
					OutBoard(Type);
					InWaitBoard(boardNum, Type);

					// 피스의 위치정보 변경
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// 기존에 보드에 자리잡은 피스를 해제하고 이동시킬 피스의 정보를 저장
				else
				{
					// 피스 교체 (교체의 수동체)
					script.OutBoard(script.Type);
					script.InBoard(nowBoard.getBoardNum(), script.Type);

					// 피스 교체 (교체의 주체)
					OutBoard(Type);
					InBoard(boardNum, Type);

					// 피스의 위치정보 변경
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// 보드에 피스 정보 저장
				script.nowBoard.GetComponent<Board>().setFriend(true, changePice);
			}
			// 피스를 배치&재배치 하려는 보드의 정보를 파악(배틀 보드와 웨이팅 보드 판별)
			else
			{
				// 위치에 따라 보드를 배치
				// 기존에 보드에 자리잡은 피스를 해제하고 이동시킬 피스의 정보를 저장
				if (boardNum / 10 >= 9)
				{
					// 피스 교체 (교체의 수동체)
					script.OutWaitBoard(script.Type);
					script.InWaitBoard(nowBoard.getBoardNum(), script.Type);

					// 피스 교체 (교체의 주체)
					OutWaitBoard(Type);
					InWaitBoard(boardNum, Type);

					// 피스의 위치정보 변경
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// 기존에 보드에 자리잡은 피스를 해제하고 이동시킬 피스의 정보를 저장
				else
				{
					// 피스 교체 (교체의 수동체)
					script.OutBoard(script.Type);
					script.InWaitBoard(nowBoard.getBoardNum(), script.Type);

					// 기존에 보드에 자리잡은 피스를 해제하고 이동시킬 피스의 정보를 저장
					OutWaitBoard(Type);
					InBoard(boardNum, Type);

					// 피스의 위치정보 변경
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				
				// 보드에 피스 정보 저장
				script.nowBoard.GetComponent<Board>().setFriend(true, changePice);
			}
		}
		else
		{
			// 피스를 배치&재배치 하려는 보드의 정보를 파악(배틀 보드와 웨이팅 보드 판별)
			if (nowBoard.getBoardNum() / 10 < 9)
			{
				// 현재 보드의 정보를 해제
				OutBoard(Type);
				// 위치에 따라 보드를 배치
				if (boardNum / 10 >= 9)
				{
					InWaitBoard(boardNum, Type);
				}
				else
				{
					InBoard(boardNum, Type);
				}
			}
			// 피스를 배치&재배치 하려는 보드의 정보를 파악(배틀 보드와 웨이팅 보드 판별)
			else
			{
				// 현재 보드의 정보를 해제
				OutWaitBoard(Type);
				// 위치에 따라 보드를 배치
				if (boardNum / 10 >= 9)
				{
					InWaitBoard(boardNum, Type);
				}
				else
				{
					InBoard(boardNum, Type);
				}
			}
		}
	}

	/// <summary>
	/// 기물 판매 함수
	/// </summary>
	public void SellPiece()
	{
		// 현재 보드가 배틀 보드 일 때
		if (nowBoard.getBoardNum() / 10 < 9)
		{
			OutBoard(Type);
		}
		// 현재 보드가 대기석 일 때
		else
		{
			OutWaitBoard(Type);
		}

		// 만약 성이 1성 이상일 때
		if (pieceStorage.Count > 0)
		{
			// 스토리지 내의 모든 기물 반환
			returnStorage();
		}

		// 본체 오브젝트 오브젝트풀에 반환
		Pool_Instance.resetPieceList(gameObject.GetComponent<ChessPiece>());
	}
}
