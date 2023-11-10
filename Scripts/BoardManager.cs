using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameManager;
using static ScrollIcon;
using static AIplayer;

public class BoardManager : MonoBehaviour
{
	static public BoardManager BM_instance = null;

	// ���콺 ��ġ�� ���� ��ġ ��ǥ ������Ʈ
	public GameObject TouchDownVec;
	public GameObject TouchUpVec;
	// ���̸� ������ MeshCollider
	public MeshCollider mesh;
	// ����ĳ��Ʈ
	Ray mouseRay;

	// ������ �ǽ�
	[SerializeField]
	GameObject _Piece;
	// ���� �������� �ǽ�
	public ChessPiece _PieceData;

	// �ǽ� UI
	public PieceData _DataUI;

	private void Awake()
	{
		if (BM_instance == null)
			BM_instance = this;
	}

	// �ǽ� Ÿ��
	public enum PieceType
	{
		Friend,
		Enemy,
		Crip
	}

	// ������ ����Ʈ
	public List<Board> Boards;
	// �����ǿ� ���� �����ϱ� ���� Dictonary
	Dictionary<int, int> boardStature = new Dictionary<int, int>();

	// ���ʹ� �ǽ� ����Ʈ
	public List<ChessPiece> _Enemies;
	// �Ʊ� �ǽ� ����Ʈ
	public List<ChessPiece> _Friends;

	// �Ʊ� �ó��� Ȱ��ȭ ���� �⹰ ����Ʈ
	List<ChessPiece.PieceName> _FriendSynergyName;
	// �ó��� Ȱ��ȭ ��ųʸ�
	Dictionary<Pieces<ChessPiece>.PieceSynergy, int> _FriendSynergy; 

	// ���� ���۽� ������ ����Ʈ �ʱ�ȭ
	private void Start()
	{
		Boards = new List<Board>();
		List<int> nums = new List<int>();

		// ������ (���� �࿡ ���ڿ� ���� 11~18/21~28 - 71~78/81~88���� ����Ʈ�� �߰�)
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				int num = i * 10 + j;

				// �̸� : FightBoard + ����
				string boardName = "FightBoard" + num.ToString();

				// �����ǿ� �߰�
				Boards.Add(GameObject.Find(boardName).GetComponent<Board>());
				nums.Add(num);
			}
		}

		for (int i = 0; i < Boards.Count; i++)
		{
			boardStature.Add(nums[i], i);
		}

		_FriendSynergyName = new List<ChessPiece.PieceName>();
		_FriendSynergy = new Dictionary<Pieces<ChessPiece>.PieceSynergy, int>();
	}

	/// <summary>
	/// ���� ������ �Ʊ� ��ġ �ʱ�ȭ �Լ�
	/// </summary>
	public void resetBoard()
	{
		// ����Ʈ ���� ��� �Ʊ� �ǽ��� �������
		for (int i = 0; i < _Friends.Count; i++)
		{
			_Friends[i].gameObject.SetActive(true);
			// ���� ��ġ�� ���带 �����ϰ� ���� ������ �ʱ�ȭ�Ѵ�
			_Friends[i].RebornPiece(_Friends[i].Fun_oriPos, _Friends[i].Type);
			_Friends[i].transform.position = _Friends[i].nowBoard.transform.position;
			_Friends[i].PieceRotate(0);
			_Friends[i].Fun_curHP = _Friends[i].Fun_maxHP;
			_Friends[i].Fun_curMP = 0;
		}
	}

	/// <summary>
	/// ���� ����
	/// </summary>
	/// <param name="boardName">������ ������</param>
	/// <param name="InOut">�⹰ ��ġ, ���� ����</param>
	/// <param name="piece">������ �⹰</param>
	/// <param name="type">�⹰�� Ÿ��</param>
	public void setBoard(string boardName, bool InOut, GameObject piece, PieceType type)
	{
		if (type == PieceType.Friend)
		{
			// �ش� ���忡 �Ʊ��� ��ġ/����
			GameObject.Find(boardName).GetComponent<Board>().setFriend(InOut, piece);
		}
		else if (type == PieceType.Enemy)
		{
			// �ش� ���忡 �Ʊ��� ��ġ/����
			GameObject.Find(boardName).GetComponent<Board>().setEnemy(InOut, piece);
		}
		else if (type == PieceType.Crip)
		{
			// �ش� ���忡 �Ʊ��� ��ġ/����
			GameObject.Find(boardName).GetComponent<Board>().setEnemy(InOut, piece);
		}
	}

	/// <summary>
	/// �⹰ ����Ʈ ����
	/// </summary>
	/// <param name="boardNum">������ ����</param>
	/// <param name="piece">����Ʈ�� �߰��� �⹰</param>
	/// <param name="InOut">�⹰ ����Ʈ�� �߰�, ���� ����</param>
	/// <param name="type">�⹰ Ÿ��</param>
	public void setList(int boardNum, ChessPiece piece, bool InOut, PieceType type)
	{
		if (type == PieceType.Friend)
		{
			if (boardNum / 10 < 9)
			{
				if (InOut)
				{
					_Friends.Add(piece);
					setFrinedSynergy(piece, true);
				}
				else
				{
					_Friends.Remove(piece);
					setFrinedSynergy(piece, false);
				}
			}
			else
			{
				if (InOut)
					GM_instance._waitFriends.Add(piece);
				else
					GM_instance._waitFriends.Remove(piece);
			}
		}
		else
		{
			if (boardNum / 10 < 9)
			{
				if (InOut)
				{
					_Enemies.Add(piece);
				}
				else
				{
					_Enemies.Remove(piece);
				}
			}
			else
			{
				if (InOut)
					AI_instance._waitEnemy.Add(piece);
				else
					AI_instance._waitEnemy.Remove(piece);
			}
		}
	}

	/// <summary>
	/// �ó��� ���� �Լ�
	/// </summary>
	/// <param name="piece">���忡 �߰��� �⹰</param>
	/// <param name="InOut">�ó��� �߰� ����</param>
	void setFrinedSynergy(ChessPiece piece, bool InOut)
	{
		// �ó����� �߰��� ��
		if (InOut)
		{
			// ���� �ó��� ��Ͽ� �ߺ��Ǵ� �⹰�� �ִ��� Ȯ��
			for (int i = 0; i < _FriendSynergyName.Count; i++)
			{
				// �ߺ� �⹰�� ���� ��� �Լ� ����
				if (piece._Name == _FriendSynergyName[i])
					return;
			}

			// �ߺ� �⹰�� ���� ��� �ó��� �߰�
			_FriendSynergyName.Add(piece._Name);

			// ���� Ȱ�� �ó��� ����� 0 �̻��� ��
			if (_FriendSynergy.Count > 0)
			{
				// �ߺ��� �ó����� ���� ���
				if (_FriendSynergy.ContainsKey(piece.Attribute))
				{
					// �ó��� 1 ����
					_FriendSynergy[piece.Attribute] += 1;
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
				// �ߺ��� �ó����� ���� ��� �ó��� �߰�
				else
				{
					_FriendSynergy.Add(piece.Attribute, 1);
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
					
				// �ߺ��� �ó����� ���� ���
				if (_FriendSynergy.ContainsKey(piece.Class))
				{
					// �ó��� 1 ����
					_FriendSynergy[piece.Class] += 1;
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
				// �ߺ��� �ó����� ���� ��� �ó��� �߰�
				else
				{
					_FriendSynergy.Add(piece.Class, 1);
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
					
			}
			// ���� Ȱ�� �ó��� ����� 0 ������ ��
			else
			{
				// �ó��� �߰�
				_FriendSynergy.Add(piece.Attribute, 1);
				icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				_FriendSynergy.Add(piece.Class, 1);
				icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
			}
		}
		// �ó����� �� ��
		else
		{
			// ���� ������ ���� ������ �⹰�� �ִ� �� Ȯ��
			for (int i = 0; i < _Friends.Count; i++)
			{
				if (_Friends[i]._Name == piece._Name)
					return;
			}

			// ������ ���� ������ �̸��� ��� �⹰�� ������� ��� �ó��� ��Ͽ��� ����
			_FriendSynergyName.Remove(piece._Name);

			// �ó��� ����� 0 �̻��� ��
			if (_FriendSynergy.Count > 0)
			{
				// �ߺ��� �ó����� ������ ���
				if (_FriendSynergy.ContainsKey(piece.Attribute))
				{
					// �ó��� 1 ����
					_FriendSynergy[piece.Attribute] -= 1;
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
				// �ߺ��� �ó����� ������ ���
				if (_FriendSynergy.ContainsKey(piece.Class))
				{
					// �ó��� 1 ����
					_FriendSynergy[piece.Class] -= 1;
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
					
			}
			
			// ���� �ó��� ��Ͽ� Ư�� �ó����� ���� ������� ���
			if (_FriendSynergy[piece.Attribute] <= 0)
				// �ó��� ��Ͽ��� �ش� �ó��� ����
				_FriendSynergy.Remove(piece.Attribute);
			// ���� �ó��� ��Ͽ� Ư�� �ó����� ���� ������� ���
			if (_FriendSynergy[piece.Class] <= 0)
				// �ó��� ��Ͽ��� �ش� �ó��� ����
				_FriendSynergy.Remove(piece.Class);
		}
	}

	// �ó��� ����
	public void activeSynergy()
	{
		foreach(var synergy in _FriendSynergy)
		{
			switch(synergy.Key)
			{
				case Pieces<ChessPiece>.PieceSynergy.Steel:
					if (3 <= synergy.Value && synergy.Value < 6)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Steel)
							{
								Debug.Log(_Friends[i].Fun_nowArmor);
								_Friends[i].Fun_nowArmor = 3;
								Debug.Log(_Friends[i].Fun_nowArmor);
							}
						}
					}
					else if (6 <= synergy.Value)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							_Friends[i].Fun_nowArmor = 5;
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Nature:
					if (2 <= synergy.Value && synergy.Value < 4)
					{
						for (int i = 0; i < _Enemies.Count; i++)
						{
							Debug.Log(_Enemies[i].Fun_nowAttack);
							_Enemies[i].Fun_nowAttack = -2;
							Debug.Log(_Enemies[i].Fun_nowAttack);
						}
					}
					else if (4 <= synergy.Value)
					{
						for (int i = 0; i < _Enemies.Count; i++)
						{
							_Enemies[i].Fun_nowAttack = -4;
							_Friends[i].Fun_nowAttack = 2;
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Undead:
					if (2 <= synergy.Value && synergy.Value < 4)
					{
						for (int i = 0; i < _Enemies.Count; i++)
						{
							Debug.Log(_Enemies[i].Fun_nowArmor);
							_Enemies[i].Fun_nowArmor = -2;
							Debug.Log(_Enemies[i].Fun_nowArmor);
						}
					}
					else if (4 <= synergy.Value)
					{
						for (int i = 0; i < _Enemies.Count; i++)
						{
							_Enemies[i].Fun_nowArmor = -4;
							_Enemies[i].Fun_nowAttack = -2;
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Demon:
					if (2 <= synergy.Value && synergy.Value < 4)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Demon)
							{
								Debug.Log(_Friends[i].Fun_nowAttack);
								_Friends[i].Fun_nowAttack = 2;
								_Friends[i].Fun_nowArmor = -2;
								Debug.Log(_Friends[i].Fun_nowAttack);
							}
						}
					}
					else if (4 <= synergy.Value && synergy.Value < 6)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Demon)
							{
								_Friends[i].Fun_nowAttack = 3;
								_Friends[i].Fun_nowArmor = -3;
							}
						}
					}
					else if (6 <= synergy.Value)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Demon)
							{
								_Friends[i].Fun_nowAttack = 4;
							}
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Warrior:
					if (3 <= synergy.Value && synergy.Value < 6)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Warrior)
							{
								Debug.Log(_Friends[i].Fun_maxHP);
								_Friends[i].Fun_maxHP += 10;
								Debug.Log(_Friends[i].Fun_maxHP);
							}
						}
					}
					else if (6 <= synergy.Value)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							_Friends[i].Fun_maxHP += 15;
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Ranger:
					if (2 <= synergy.Value && synergy.Value < 4)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Ranger)
							{
								Debug.Log(_Friends[i].Fun_nowDelay);
								_Friends[i].Fun_nowDelay *= 0.8f;
								Debug.Log(_Friends[i].Fun_nowDelay);
							}
						}
					}
					if (4 <= synergy.Value && synergy.Value < 6)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Ranger)
							{
								_Friends[i].Fun_nowDelay += 0.7f;
							}
						}
					}
					else if (6 <= synergy.Value)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Attribute == Pieces<ChessPiece>.PieceSynergy.Ranger)
							{
								_Friends[i].Fun_nowDelay += 0.75f;
								_Friends[i].Fun_nowAttack = 2;
							}
						}
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Knight:
					if (3 <= synergy.Value && synergy.Value < 6)
					{
						List<ChessPiece> list = new List<ChessPiece>();

						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Class == Pieces<ChessPiece>.PieceSynergy.Knight)
							{
								Debug.Log(_Friends[i]._Name);
								list.Add(_Friends[i]);
								Debug.Log(_Friends[i]._Name);
							}
						}

						ChessPiece knight = list[Random.RandomRange(0, list.Count)];
						Debug.Log(knight.Fun_nowArmor);
						knight.Fun_nowArmor = 2;
						knight.Fun_nowAttack = 2;
						knight.Fun_maxHP = 5;
						Debug.Log(knight.Fun_nowArmor);
					}
					else if (6 <= synergy.Value)
					{
						List<ChessPiece> list = new List<ChessPiece>();

						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Class == Pieces<ChessPiece>.PieceSynergy.Knight)
							{
								list.Add(_Friends[i]);
								_Friends[i].Fun_nowArmor = 2;
								_Friends[i].Fun_nowAttack = 2;
								_Friends[i].Fun_maxHP = 5;
							}
						}

						ChessPiece knight = list[Random.RandomRange(0, list.Count)];
						knight.Fun_nowArmor = 2;
						knight.Fun_nowAttack = 2;
						knight.Fun_maxHP = 3;
					}
					break;
				case Pieces<ChessPiece>.PieceSynergy.Mage:
					if (2 <= synergy.Value && synergy.Value < 4)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Class == Pieces<ChessPiece>.PieceSynergy.Mage)
							{
								Debug.Log(_Friends[i].Fun_curMP);
								_Friends[i].Fun_curMP = 10;
								Debug.Log(_Friends[i].Fun_curMP);
							}
						}
					}
					else if (4 <= synergy.Value)
					{
						for (int i = 0; i < _Friends.Count; i++)
						{
							if (_Friends[i].Class == Pieces<ChessPiece>.PieceSynergy.Mage)
							{
								_Friends[i].Fun_curMP = 20;
								_Friends[i].Fun_nowArmor = 2;
							}
						}
					}
					break;
			}
		}
	}

	/// <summary>
	/// ��ġ �Լ�
	/// </summary>
	/// <param name="nowPos">���� Ž���� �ǽ��� �ǽ��� ���� ��ġ</param>
	/// <returns>���� �ǽ��� ������ ��ġ</returns>
	public int SearchPieces(int nowPos, PieceType type)
	{
		// ��ȯ��
		int target = 0;
		// �ٻ簪
		int AppValue = 1000;

		switch (type)
		{
			case PieceType.Friend:
				// ���ʹ� �迭���� �� ���ʹ̿� �ǽ� ���̿� �Ÿ��� ���Ѵ�
				for (int i = 0; i < _Enemies.Count; i++)
				{
					if (_Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// �Ÿ� ����
					int Distance = 0;

					// �� �ǽ� ���̿� ���� �Ÿ��� ���Ѵ�
					int column = (nowPos % 10) - (_Enemies[i].nowBoard.getBoardNum() % 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (column < 0)
						column *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += column;

					// �� �ǽ� ������ ���� �Ÿ��� ���Ѵ�
					int row = (nowPos / 10) - (_Enemies[i].nowBoard.getBoardNum() / 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (row < 0)
						row *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += row;

					// ���� �Ÿ��� ���� �ٻ簪�� ���� ª�� ��� �ٻ簪�� �ֽ�ȭ �Ѵ�.
					if (Distance < AppValue)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Enemies[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Enemies[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Enemies[i].nowBoard.getBoardNum();
					}
				}

				return target;

			case PieceType.Enemy:
				// ���ʹ� �迭���� �� ���ʹ̿� �ǽ� ���̿� �Ÿ��� ���Ѵ�
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// �Ÿ� ����
					int Distance = 0;

					// �� �ǽ� ���̿� ���� �Ÿ��� ���Ѵ�
					int column = (nowPos % 10) - (_Friends[i].nowBoard.getBoardNum() % 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (column < 0)
						column *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += column;

					// �� �ǽ� ������ ���� �Ÿ��� ���Ѵ�
					int row = (nowPos / 10) - (_Friends[i].nowBoard.getBoardNum() / 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (row < 0)
						row *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += row;

					// ���� �Ÿ��� ���� �ٻ簪�� ���� ª�� ��� �ٻ簪�� �ֽ�ȭ �Ѵ�.
					if (Distance < AppValue)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Friends[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Friends[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Friends[i].nowBoard.getBoardNum();
					}
				}

				return target;

			case PieceType.Crip:
				// ���ʹ� �迭���� �� ���ʹ̿� �ǽ� ���̿� �Ÿ��� ���Ѵ�
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// �Ÿ� ����
					int Distance = 0;

					// �� �ǽ� ���̿� ���� �Ÿ��� ���Ѵ�
					int column = (nowPos % 10) - (_Friends[i].nowBoard.getBoardNum() % 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (column < 0)
						column *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += column;

					// �� �ǽ� ������ ���� �Ÿ��� ���Ѵ�
					int row = (nowPos / 10) - (_Friends[i].nowBoard.getBoardNum() / 10);
					// ���� ���� ������ ���� ��� ���밪���� ��ȯ�Ѵ�
					if (row < 0)
						row *= -1;

					// ���� �� �Ÿ��� ���� �Ÿ� ������ ���Ѵ�
					Distance += row;

					// ���� �Ÿ��� ���� �ٻ簪�� ���� ª�� ��� �ٻ簪�� �ֽ�ȭ �Ѵ�.
					if (Distance < AppValue)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Friends[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Friends[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// ���� �Ÿ��� �ٻ簪�� ����
						AppValue = Distance;
						// Ÿ���� ���� ��ǥ�� �����Ѵ�
						target = _Friends[i].nowBoard.getBoardNum();
					}
				}

				return target;
		}

		return 0;
	}

	/// <summary>
	/// Ÿ���� �Լ�
	/// </summary>
	/// <param name="boardNum">�ǽ��� �ڸ����� ���� �ѹ�</param>
	/// <returns>Ÿ���� �� �ǽ� ������ ��ȯ</returns>
	public ChessPiece EnemyTargetting(int boardNum, PieceType type)
	{
		switch(type)
		{
			case PieceType.Friend:
				// ���ʹ� ����Ʈ���� ��Ī�� ���� �ѹ��� ���� ���ʹ̸� ã�Ƽ� ��ȯ
				for (int i = 0; i < _Enemies.Count; i++)
				{
					if (_Enemies.Count > 0)
					{
						if (_Enemies[i].nowBoard == null)
							continue;
						if (_Enemies[i].nowBoard.getBoardNum() == boardNum)
						{
							return _Enemies[i];
						}
					}
					else
						break;
				}
				break;
			case PieceType.Enemy:
				// ���ʹ� ����Ʈ���� ��Ī�� ���� �ѹ��� ���� ���ʹ̸� ã�Ƽ� ��ȯ
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends.Count > 0)
					{
						if (_Friends[i].nowBoard == null)
							continue;
						if (_Friends[i].nowBoard.getBoardNum() == boardNum)
						{
							return _Friends[i];
						}
					}
					else
						break;
				}
				break;
			case PieceType.Crip:
				// ���ʹ� ����Ʈ���� ��Ī�� ���� �ѹ��� ���� ���ʹ̸� ã�Ƽ� ��ȯ
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends.Count > 0)
					{
						if (_Friends[i].nowBoard == null)
							continue;
						if (_Friends[i].nowBoard.getBoardNum() == boardNum)
						{
							return _Friends[i];
						}
					}
					else
						break;
				}
				break;
		}

		return null;
	}

	// ���� �ȿ� ��� ����
	public bool IsBlock(int boardNum)
	{
		if (Boards[boardStature[boardNum]].getEnemy() || Boards[boardStature[boardNum]].getFriend())
		{
			return true;
		}

		return false;
	}

	// ��ġ�� ���� ���� �⹰�� ����
	public void SetPiece(GameObject piece)
	{
		_Piece = piece;
		_PieceData = piece.GetComponent<ChessPiece>();

		_DataUI.PickPiece(_PieceData);
	}

	// ���� �����ϰ� �ִ� �ǽ��� ��ȯ�ϰ� �ʱ�ȭ
	public GameObject GetPiece()
	{
		GameObject piece = _Piece;
		_Piece = null;
		return piece;
	}

	// ���� �����ϰ� �ִ� �ǽ� ����
	public void DeletePieceData()
	{
		_PieceData = null;

		_DataUI.hideUI();
	}

	private void Update()
	{
		if (GM_instance.ShopUI.activeSelf)
			return;

		// ���콺 ��Ŭ�� ����
		if (Input.GetMouseButtonDown(0))
		{
			// ��Ŭ�� �� ���� ���콺 ����Ʈ�� ���� ��ǥ ��ġ�� ����ĳ��Ʈ �߻� 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// ���̿� ���� ������Ʈ ����ü
			RaycastHit hit;

			// ���� ���̰����� Plane�� ����ĳ��Ʈ �浹 ��
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// ���콺 ������ ������Ʈ�� ��ġ�� ����ĳ��Ʈ �浹 ��ġ�� ����
				TouchDownVec.transform.position = hit.point;
				// �� �̺�Ʈ �浹 ���� ������Ʈ�� �浹 �������� ������� ����
				TouchUpVec.transform.position =
					new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// ��Ŭ�� �� ���� ���콺 ����Ʈ�� ���� ��ǥ ��ġ�� ����ĳ��Ʈ �߻� 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// ���̿� ���� ������Ʈ ����ü
			RaycastHit hit;

			// ���� ���̰����� Plane�� ����ĳ��Ʈ �浹 ��
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// ���콺 ������ ������Ʈ�� ��ġ�� ����ĳ��Ʈ �浹 ��ġ�� ����
				TouchUpVec.transform.position = hit.point;
				// �ٿ� �̺�Ʈ �浹 ���� ������Ʈ�� �浹 �������� ������� ����
				TouchDownVec.transform.position =
					new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}

		if (Input.GetMouseButton(0) && _Piece != null)
		{
			// ��Ŭ�� �� ���� ���콺 ����Ʈ�� ���� ��ǥ ��ġ�� ����ĳ��Ʈ �߻� 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// ���̿� ���� ������Ʈ ����ü
			RaycastHit hit;

			// ���� ���̰����� Plane�� ����ĳ��Ʈ �浹 ��
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// ���콺 ������ ������Ʈ�� ��ġ�� ����ĳ��Ʈ �浹 ��ġ�� ����
				_Piece.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}
	}
}
