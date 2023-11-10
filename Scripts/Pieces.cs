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


// ��� �ǽ��鿡 ����� �θ� Ŭ����
public class Pieces<T> : MonoBehaviour where T : MonoBehaviour
{
	#region _���� ������ ������

	// Ÿ��
	[SerializeField]
	PieceType _Type;

	// �ó���
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

	// Ư�� �ó���
	PieceSynergy _Attribute;
	// ���� �ó���
	PieceSynergy _Class;

	// �ǽ� Ƽ��
	public enum PieceTier
	{
		ONE,
		TWO,
		THREE,
		FOUR,
		FIVE
	}

	// Ƽ��
	PieceTier _Tier;

	// �ǽ� ��
	public enum PieceStar
	{
		oneStar,
		twoStar,
		threeStar
	}

	// ���� ���
	public PieceStar _Star;

	#endregion

	// ���� ����
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

	// �⹰�� �ൿ ����
	public NowState _State;

	#region _�������ͽ�

	// �⺻ ü��
	float oriHP;
	// ���� ü��
	float curHP;
	// �ִ� ü��
	float maxHP;

	// �⺻ ���ݷ�
	float oriAttack;
	// �߰� ���ݷ�
	float nowAttack;
	// ��ų ���ݷ�
	float skillAttack;

	// �⺻ ����
	float oriArmor;
	// �߰� ����
	float nowArmor;

	// �⺻ ���� �ӵ�
	float oriDelay;
	// �߰� ���� �ӵ�
	float nowDelay;

	// ���� ����
	float curMP;
	// �ִ� ����
	float maxMP;
	// ��ų Ÿ��
	float skillTime;
	// ��ų �ߵ� ��
	bool isSkill;
	// ���� ����
	int atkRange;

	#endregion

	// ���� ����
	public Board nowBoard;
	// �ǽ��� ���� ��ġ
	int OriPos;

	// �̵� �ӵ�
	float moveSpeed = 3.0f;
	// �̵� �� �Ǻ�
	public bool isMove;
	// �̵� ��� ���
	[SerializeField]
	Navigation _navi;

	// �ִϸ�����
	public Animator _anim;

	// �ƿ�����
	[SerializeField]
	Outline[] outlines;
	// �ӽ÷� ������ �� �±� ��� �⹰
	List<ChessPiece> pieceStorage;

	#region _Get/Set �Լ�

	// �⺻ ü�� get/set
	public float Fun_oriHP
	{
		set { oriHP = value; }
		get { return oriHP; }
	}
	// ���� ü�� get/set
	public float Fun_curHP
	{
		set { curHP += value; }
		get { return curHP; }
	}
	// ��ü ü�� get/set
	public float Fun_maxHP
	{
		set { maxHP = value; }
		get { return maxHP; }
	}

	// �⺻ ���ݷ� get/set
	public float Fun_oriAttack
	{
		set { oriAttack = value; }
		get { return oriAttack; }
	}
	// ���� ���ݷ� get/set
	public float Fun_nowAttack
	{
		set { nowAttack = oriAttack + value; }
		get { return nowAttack; }
	}
	// ���� ���ݷ� get/set
	public float Fun_skillAttack
	{
		set { skillAttack = oriAttack + value; }
		get { return skillAttack; }
	}

	// �⺻ ���� get/set
	public float Fun_oriArmor
	{
		set { oriArmor = value; }
		get { return oriArmor; }
	}
	// ���� ���� get/set
	public float Fun_nowArmor
	{
		set { nowArmor = oriArmor + value; }
		get { return nowArmor; }
	}

	// �⺻ ������ get/set
	public float Fun_oriDelay
	{
		set { oriDelay = value; }
		get { return oriDelay; }
	}
	// ���� ������ get/set
	public float Fun_nowDelay
	{
		set { nowDelay = oriDelay + value; }
		get { return nowDelay; }
	}

	// ���� ���� get/set
	public float Fun_curMP
	{
		set { curMP = value; }
		get { return curMP; }
	}
	// ��ü ���� get/set
	public float Fun_maxMP
	{
		set { maxMP = value; }
		get { return maxMP; }
	}
	// ��ų Ÿ�� get/set
	public float Fun_skillTime
	{
		set { skillTime = value; }
		get { return skillTime; }
	}
	// ��ų ��� �� get/set
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
	// ���ݹ��� get/set
	public int Fun_attackRange
	{
		set { atkRange = value; }
		get { return atkRange; }
	}

	// ���� Ÿ�� get/set
	public PieceType Type
	{
		set { _Type = value; }
		get { return _Type; }
	}

	// ���� Ƽ�� get/set
	public PieceTier Tier
	{
		set { _Tier = value; }
		get { return _Tier; }
	}

	// ���� �� get/set
	public PieceStar Star
	{
		set { _Star = value; }
		get { return _Star; }
	}

	// ���� �� get/set
	public PieceSynergy Attribute
	{
		set { _Attribute = value; }
		get { return _Attribute; }
	}

	// ���� �� get/set
	public PieceSynergy Class
	{
		set { _Class = value; }
		get { return _Class; }
	}

	// ���� �ʱ�ȭ �Լ�
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

	// �� �±� �Լ�
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

	// �� ��ȭ �Լ�
	void upgradeStar(float Scale)
	{
		maxHP *= Scale;
		oriAttack *= Scale;
		oriArmor *= Scale;
	}

	// �������� ��ġ get/set
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

	// ���丮���� ����Ʈ ������ ��ȯ
	public int storageCount()
	{
		return pieceStorage.Count;
	}
	// ���丮������ ��� �⹰ ��ȯ
	public ChessPiece giveStorage(int i)
	{
		return pieceStorage[i];
	}
	// ���丮���� �⹰ �߰�
	public void inStorage(ChessPiece obj)
	{
		pieceStorage.Add(obj);
	}
	// ���丮�� ������ ��� �⹰ Ǯ�� ��ȯ
	public void returnStorage()
	{
		for (int i = 0; i < pieceStorage.Count; i++)
		{
			// ������ �⹰ ���� (Ǯ�� ��ȯ)
			Pool_Instance.resetPieceList(pieceStorage[i]);
			
		}

		pieceStorage.Clear();
	}

	/// <summary>
	/// �⹰ ��ġ �Լ�
	/// </summary>
	/// <param name="num">��ġ�� ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void InBoard(int num, PieceType type)
	{
		// ��ġ�� ���� �˻�
		string boardName = "FightBoard" + num;

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		switch(type)
		{
			case PieceType.Friend:
				// �ش� �����ǿ� �Ʊ� �⹰�� ��ġ
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// �������� �Ʊ� ����Ʈ�� �Ʊ� �⹰�� ����
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
			case PieceType.Enemy:
				// �ش� �����ǿ� ���ʹ� �⹰�� ��ġ
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// �������� ���ʹ� ����Ʈ�� ���ʹ� �⹰�� ����
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
			case PieceType.Crip:
				// �ش� �����ǿ� ���ʹ� �⹰�� ��ġ
				BM_instance.setBoard(boardName, true, gameObject, Type);
				// �������� ���ʹ� ����Ʈ�� ���ʹ� �⹰�� ����
				BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
				break;
		}
	}

	/// <summary>
	/// �⹰ ���� �Լ�
	/// </summary>
	/// <param name="num">������ ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void OutBoard(PieceType type)
	{
		// ������ ���� �˻�
		string boardName = "FightBoard" + nowBoard.getBoardNum();

		// �ش� �����ǿ� �⹰�� ����
		BM_instance.setBoard(boardName, false, gameObject, Type);
		// �������� �� ����Ʈ�� �⹰�� ����
		BM_instance.setList(nowBoard.getBoardNum(),
							GetComponentInChildren<ChessPiece>(), false, Type);

		// ���� �⹰�� ���� ���� ����
		nowBoard = null;
	}

	/// <summary>
	/// ��� ���� �⹰ ��ġ �Լ�
	/// </summary>
	/// <param name="num">��ġ�� ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void InWaitBoard(int num, PieceType type)
	{
		// ��ġ�� ���� �˻�
		string boardName = "WaitingBoard" + num;

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		// �ش� �����ǿ� �⹰�� ��ġ
		BM_instance.setBoard(boardName, true, gameObject, Type);
		// �������� ����Ʈ�� �⹰�� ����
		BM_instance.setList(num, GetComponentInChildren<ChessPiece>(), true, Type);
	}

	/// <summary>
	/// �⹰ ���� �Լ�
	/// </summary>
	/// <param name="num">������ ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void OutWaitBoard(PieceType type)
	{
		// ��ġ�� ���� �˻�
		string boardName = "WaitingBoard" + nowBoard.getBoardNum();

		// �ش� �����ǿ� �⹰�� ��ġ
		BM_instance.setBoard(boardName, false, gameObject, Type);
		// �������� ����Ʈ�� �⹰�� ����
		BM_instance.setList(nowBoard.getBoardNum(),
							GetComponentInChildren<ChessPiece>(), false, Type);

		// ���� �⹰�� ���� ���� ����
		nowBoard = null;
	}

	/// <summary>
	/// �̵��� ������ ��ȯ �Լ�
	/// </summary>
	/// <param name="now">���� ��ġ</param>
	/// <param name="next">�̵��� ��ġ</param>
	/// <param name="type">�⹰�� Ÿ��</param>
	public void MoveBoard(int next,PieceType type)
	{
		// ����&��ġ�� ���� �˻�
		string nowName = "FightBoard" + nowBoard.getBoardNum();
		string nextName = "FightBoard" + next;

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = GameObject.Find("FightBoard" + next).GetComponent<Board>();

		// ���� �����ǿ� �Ʊ� ����
		BM_instance.setBoard(nowName, false, gameObject, Type);
		// �ش� �����ǿ� �Ʊ� �⹰�� ��ġ
		BM_instance.setBoard(nextName, true, gameObject, Type);
	}

	/// <summary>
	/// �⹰ ����� ������ ��ȯ �Լ�
	/// </summary>
	/// <param name="now">���� ��ġ</param>
	/// <param name="type">�⹰�� Ÿ��</param>
	public void DeadPiece(int now, PieceType type)
	{
		// ��ġ�� ���� �˻�
		string nowName = "FightBoard" + nowBoard.getBoardNum();

		// ��� ī����
		int deadCount = 0;

		switch(type)
		{
			case PieceType.Friend:
				// �ش� �����ǿ� �Ʊ� �⹰�� ����
				BM_instance.setBoard(nowName, false, gameObject, type);
				// ���� �Ʊ����� ���¸� �˻�
				for (int i = 0; i < BM_instance._Friends.Count; i++)
				{
					// �Ʊ� �⹰�� ���� ���� ī���� ����
					if (BM_instance._Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// ����Ʈ ���� ��� �Ʊ� �⹰�� �׾��ٸ� ��Ʋ ����
					if (BM_instance._Friends.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
			case PieceType.Enemy:
				// �ش� �����ǿ� ���ʹ� �⹰�� ����
				BM_instance.setBoard(nowName, false, gameObject, type);
				// ���� ���ʹ̵��� ���¸� �˻�
				for (int i = 0; i < BM_instance._Enemies.Count; i++)
				{
					// ���ʹ� �⹰�� ���� ���� ī���� ����
					if (BM_instance._Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// ����Ʈ ���� ��� ���ʹ� �⹰�� �׾��ٸ� ��Ʋ ����
					if (BM_instance._Enemies.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
			case PieceType.Crip:
				// �ش� �����ǿ� ���ʹ� �⹰�� ����
				BM_instance.setBoard(nowName, false, gameObject, type);
				// ���� ���ʹ̵��� ���¸� �˻�
				for (int i = 0; i < BM_instance._Enemies.Count; i++)
				{
					// ���ʹ� �⹰�� ���� ���� ī���� ����
					if (BM_instance._Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						deadCount++;

					// ����Ʈ ���� ��� ���ʹ� �⹰�� �׾��ٸ� ��Ʋ ����
					if (BM_instance._Enemies.Count == deadCount)
						GM_instance.BattleEnd();
				}
				break;
		}

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = null;
	}

	/// <summary>
	/// �⹰ ����� ��Ȱ �Լ�
	/// </summary>
	/// <param name="now">��Ȱ�� ��ġ</param>
	/// <param name="type">�⹰�� Ÿ��</param>
	public void RebornPiece(int num, PieceType type)
	{
		// ��ġ�� ���� �˻�
		string boardName = "FightBoard" + num.ToString();

		if (nowBoard != null)
		{
			// ��ġ�� ���� �˻�
			string nowName = "FightBoard" + nowBoard.getBoardNum();

			BM_instance.setBoard(nowName, false, gameObject, type);
		}

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		switch (type)
		{
			case PieceType.Friend:
				// �ش� �����ǿ� �Ʊ� �⹰�� ��Ȱ
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
			case PieceType.Enemy:
				// �ش� �����ǿ� ���ʹ� �⹰�� ��Ȱ
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
			case PieceType.Crip:
				// �ش� �����ǿ� ���ʹ� �⹰�� ��Ȱ
				BM_instance.setBoard(boardName, true, gameObject, type);
				break;
		}
	}

	/// <summary>
	/// ���ʹ� ���忡 �⹰ ��ġ �Լ�
	/// </summary>
	/// <param name="num">��ġ�� ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void InEnemyBoard(int num)
	{
		// ��ġ�� ���� �˻�
		string boardName = "EnemyBoard" + num;

		// ���� �⹰�� ��ġ�� ���带 ����
		nowBoard = GameObject.Find(boardName).GetComponent<Board>();

		transform.position = nowBoard.transform.position;
	}

	/// <summary>
	/// ���ʹ� ���忡 �⹰ ���� �Լ�
	/// </summary>
	/// <param name="num">������ ����</param>
	/// <param name="type">�ǽ��� Ÿ��</param>
	public void OutEnemyBoard()
	{	
		// ���� �⹰�� ���� ���� ����
		nowBoard = null;
	}

	/// <summary>
	/// �̵� �ڷ�ƾ
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
		
		// �̵� ��ǥ
		Vector3 movePos = nextPos.transform.position;

		MoveBoard(nextPos.getBoardNum(), Type);

		_anim.SetTrigger("IsWalk");

		/*
		
		float timer = 0;
		
		// ���� ����
		while (transform.position.y >= movePos.y)
		{
			// �ð��� ���� ������ �̵�
			timer += Time.deltaTime;
			transform.position = Parabola(nowBoard.transform.position, movePos, 2, timer);

			// �ڷ�ƾ ������ Ÿ��
			yield return null;
		}
		*/

		// ���� ����
		while (true)
		{
			transform.position = Vector3.MoveTowards(transform.position, movePos,
				moveSpeed * Time.deltaTime);

			if (transform.position / 1 == movePos / 1)
				break;

			// �ڷ�ƾ ������ Ÿ��
			yield return null;
		}

		_anim.SetTrigger("GoIdle");

		yield return new WaitForSeconds(0.5f);

		transform.position = new Vector3(transform.position.x, -0.5f, transform.position.z);

		isMove = false;

		_State = NowState.IDLE;
	}

	/// <summary>
	/// ������ �̵� �Լ�
	/// </summary>
	/// <param name="start">���� ��ġ</param>
	/// <param name="end">���� ��ġ</param>
	/// <param name="height">����</param>
	/// <param name="t">�ð�</param>
	/// <returns></returns>
	Vector3 Jump(Vector3 start, Vector3 end, float height, float t)
	{
		Func<float, float> f = x => -3 * height * x * x + 3 * height * x;

		var mid = Vector3.Lerp(start, end, t);

		return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
	}

	/// <summary>
	/// ���� ������ üũ
	/// </summary>
	/// <param name="now">���� ��ġ</param>
	/// <param name="target">��ǥ ��ġ</param>
	int RangeCheck(int now, int target)
	{
		// �Ÿ� ����
		int Distance = 0;

		// �� �ǽ� ���̿� ���� �Ÿ��� ���Ѵ�
		int column = (now % 10) - (target % 10);
		// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
		if (column < 0)
			column *= -1;

		// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
		Distance += column;

		// �� �ǽ� ������ ���� �Ÿ��� ���Ѵ�
		int row = (now / 10) - (target / 10);
		// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
		if (row < 0)
			row *= -1;

		// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
		Distance += row;

		return Distance;
	}

	/// <summary>
	/// ������ ��ġ ���� �Լ�
	/// </summary>
	/// <param name="boardNum"> ��ġ�� ���� ����</param>
	/// <param name="changePice"> ��ġ�� ���忡 ���� �ǽ�</param>
	public void ChangeBoard(int boardNum, GameObject changePice = null)
	{
		if (changePice != null)
		{
			// �ǽ��� ��ũ���͸� �����´�
			ChessPiece script = changePice.GetComponent<ChessPiece>();

			// �ǽ��� ��ġ&���ġ �Ϸ��� ������ ������ �ľ�(��Ʋ ����� ������ ���� �Ǻ�)
			if (nowBoard.getBoardNum() / 10 < 9)
			{
				// ��ġ�� ���� ���带 ��ġ
				// ������ ���忡 �ڸ����� �ǽ��� �����ϰ� �̵���ų �ǽ��� ������ ����
				if (boardNum / 10 >= 9)
				{
					// �ǽ� ��ü (��ü�� ����ü)
					script.OutWaitBoard(script.Type);
					GM_instance._waitFriends.Remove(script);
					script.InBoard(nowBoard.getBoardNum(), script.Type);

					// �ǽ� ��ü (��ü�� ��ü)
					OutBoard(Type);
					InWaitBoard(boardNum, Type);

					// �ǽ��� ��ġ���� ����
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// ������ ���忡 �ڸ����� �ǽ��� �����ϰ� �̵���ų �ǽ��� ������ ����
				else
				{
					// �ǽ� ��ü (��ü�� ����ü)
					script.OutBoard(script.Type);
					script.InBoard(nowBoard.getBoardNum(), script.Type);

					// �ǽ� ��ü (��ü�� ��ü)
					OutBoard(Type);
					InBoard(boardNum, Type);

					// �ǽ��� ��ġ���� ����
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// ���忡 �ǽ� ���� ����
				script.nowBoard.GetComponent<Board>().setFriend(true, changePice);
			}
			// �ǽ��� ��ġ&���ġ �Ϸ��� ������ ������ �ľ�(��Ʋ ����� ������ ���� �Ǻ�)
			else
			{
				// ��ġ�� ���� ���带 ��ġ
				// ������ ���忡 �ڸ����� �ǽ��� �����ϰ� �̵���ų �ǽ��� ������ ����
				if (boardNum / 10 >= 9)
				{
					// �ǽ� ��ü (��ü�� ����ü)
					script.OutWaitBoard(script.Type);
					script.InWaitBoard(nowBoard.getBoardNum(), script.Type);

					// �ǽ� ��ü (��ü�� ��ü)
					OutWaitBoard(Type);
					InWaitBoard(boardNum, Type);

					// �ǽ��� ��ġ���� ����
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				// ������ ���忡 �ڸ����� �ǽ��� �����ϰ� �̵���ų �ǽ��� ������ ����
				else
				{
					// �ǽ� ��ü (��ü�� ����ü)
					script.OutBoard(script.Type);
					script.InWaitBoard(nowBoard.getBoardNum(), script.Type);

					// ������ ���忡 �ڸ����� �ǽ��� �����ϰ� �̵���ų �ǽ��� ������ ����
					OutWaitBoard(Type);
					InBoard(boardNum, Type);

					// �ǽ��� ��ġ���� ����
					changePice.transform.position = new Vector3(script.nowBoard.transform.position.x,
															script.nowBoard.transform.position.y,
															script.nowBoard.transform.position.z);
				}
				
				// ���忡 �ǽ� ���� ����
				script.nowBoard.GetComponent<Board>().setFriend(true, changePice);
			}
		}
		else
		{
			// �ǽ��� ��ġ&���ġ �Ϸ��� ������ ������ �ľ�(��Ʋ ����� ������ ���� �Ǻ�)
			if (nowBoard.getBoardNum() / 10 < 9)
			{
				// ���� ������ ������ ����
				OutBoard(Type);
				// ��ġ�� ���� ���带 ��ġ
				if (boardNum / 10 >= 9)
				{
					InWaitBoard(boardNum, Type);
				}
				else
				{
					InBoard(boardNum, Type);
				}
			}
			// �ǽ��� ��ġ&���ġ �Ϸ��� ������ ������ �ľ�(��Ʋ ����� ������ ���� �Ǻ�)
			else
			{
				// ���� ������ ������ ����
				OutWaitBoard(Type);
				// ��ġ�� ���� ���带 ��ġ
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
	/// �⹰ �Ǹ� �Լ�
	/// </summary>
	public void SellPiece()
	{
		// ���� ���尡 ��Ʋ ���� �� ��
		if (nowBoard.getBoardNum() / 10 < 9)
		{
			OutBoard(Type);
		}
		// ���� ���尡 ��⼮ �� ��
		else
		{
			OutWaitBoard(Type);
		}

		// ���� ���� 1�� �̻��� ��
		if (pieceStorage.Count > 0)
		{
			// ���丮�� ���� ��� �⹰ ��ȯ
			returnStorage();
		}

		// ��ü ������Ʈ ������ƮǮ�� ��ȯ
		Pool_Instance.resetPieceList(gameObject.GetComponent<ChessPiece>());
	}
}
