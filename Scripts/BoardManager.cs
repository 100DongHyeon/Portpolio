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

	// 마우스 터치를 위한 터치 좌표 오브젝트
	public GameObject TouchDownVec;
	public GameObject TouchUpVec;
	// 레이를 감지할 MeshCollider
	public MeshCollider mesh;
	// 레이캐스트
	Ray mouseRay;

	// 조작할 피스
	[SerializeField]
	GameObject _Piece;
	// 현재 선택중인 피스
	public ChessPiece _PieceData;

	// 피스 UI
	public PieceData _DataUI;

	private void Awake()
	{
		if (BM_instance == null)
			BM_instance = this;
	}

	// 피스 타입
	public enum PieceType
	{
		Friend,
		Enemy,
		Crip
	}

	// 보드판 리스트
	public List<Board> Boards;
	// 보드판에 직접 접근하기 위한 Dictonary
	Dictionary<int, int> boardStature = new Dictionary<int, int>();

	// 에너미 피스 리스트
	public List<ChessPiece> _Enemies;
	// 아군 피스 리스트
	public List<ChessPiece> _Friends;

	// 아군 시너지 활성화 중인 기물 리스트
	List<ChessPiece.PieceName> _FriendSynergyName;
	// 시너지 활성화 딕셔너리
	Dictionary<Pieces<ChessPiece>.PieceSynergy, int> _FriendSynergy; 

	// 게임 시작시 보드판 리스트 초기화
	private void Start()
	{
		Boards = new List<Board>();
		List<int> nums = new List<int>();

		// 보드판 (열과 행에 숫자에 따라 11~18/21~28 - 71~78/81~88까지 리스트에 추가)
		for (int i = 1; i < 9; i++)
		{
			for (int j = 1; j < 9; j++)
			{
				int num = i * 10 + j;

				// 이름 : FightBoard + 숫자
				string boardName = "FightBoard" + num.ToString();

				// 보드판에 추가
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
	/// 전투 종료후 아군 배치 초기화 함수
	/// </summary>
	public void resetBoard()
	{
		// 리스트 내의 모든 아군 피스를 대상으로
		for (int i = 0; i < _Friends.Count; i++)
		{
			_Friends[i].gameObject.SetActive(true);
			// 최초 배치로 보드를 변경하고 보는 방향을 초기화한다
			_Friends[i].RebornPiece(_Friends[i].Fun_oriPos, _Friends[i].Type);
			_Friends[i].transform.position = _Friends[i].nowBoard.transform.position;
			_Friends[i].PieceRotate(0);
			_Friends[i].Fun_curHP = _Friends[i].Fun_maxHP;
			_Friends[i].Fun_curMP = 0;
		}
	}

	/// <summary>
	/// 보드 세팅
	/// </summary>
	/// <param name="boardName">세팅할 보드판</param>
	/// <param name="InOut">기물 배치, 해제 여부</param>
	/// <param name="piece">세팅할 기물</param>
	/// <param name="type">기물의 타입</param>
	public void setBoard(string boardName, bool InOut, GameObject piece, PieceType type)
	{
		if (type == PieceType.Friend)
		{
			// 해당 보드에 아군을 배치/제거
			GameObject.Find(boardName).GetComponent<Board>().setFriend(InOut, piece);
		}
		else if (type == PieceType.Enemy)
		{
			// 해당 보드에 아군을 배치/제거
			GameObject.Find(boardName).GetComponent<Board>().setEnemy(InOut, piece);
		}
		else if (type == PieceType.Crip)
		{
			// 해당 보드에 아군을 배치/제거
			GameObject.Find(boardName).GetComponent<Board>().setEnemy(InOut, piece);
		}
	}

	/// <summary>
	/// 기물 리스트 세팅
	/// </summary>
	/// <param name="boardNum">세팅할 보드</param>
	/// <param name="piece">리스트에 추가할 기물</param>
	/// <param name="InOut">기물 리스트에 추가, 해제 여부</param>
	/// <param name="type">기물 타입</param>
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
	/// 시너지 세팅 함수
	/// </summary>
	/// <param name="piece">보드에 추가된 기물</param>
	/// <param name="InOut">시너지 추가 여부</param>
	void setFrinedSynergy(ChessPiece piece, bool InOut)
	{
		// 시너지를 추가할 때
		if (InOut)
		{
			// 현재 시너지 목록에 중복되는 기물이 있는지 확인
			for (int i = 0; i < _FriendSynergyName.Count; i++)
			{
				// 중복 기물이 있을 경우 함수 종료
				if (piece._Name == _FriendSynergyName[i])
					return;
			}

			// 중복 기물이 없을 경우 시너지 추가
			_FriendSynergyName.Add(piece._Name);

			// 현재 활성 시너지 목록이 0 이상일 때
			if (_FriendSynergy.Count > 0)
			{
				// 중복된 시너지가 있을 경우
				if (_FriendSynergy.ContainsKey(piece.Attribute))
				{
					// 시너지 1 증가
					_FriendSynergy[piece.Attribute] += 1;
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
				// 중복된 시너지가 없을 경우 시너지 추가
				else
				{
					_FriendSynergy.Add(piece.Attribute, 1);
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
					
				// 중복된 시너지가 있을 경우
				if (_FriendSynergy.ContainsKey(piece.Class))
				{
					// 시너지 1 증가
					_FriendSynergy[piece.Class] += 1;
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
				// 중복된 시너지가 없을 경우 시너지 추가
				else
				{
					_FriendSynergy.Add(piece.Class, 1);
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
					
			}
			// 현재 활성 시너지 목록이 0 이하일 때
			else
			{
				// 시너지 추가
				_FriendSynergy.Add(piece.Attribute, 1);
				icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				_FriendSynergy.Add(piece.Class, 1);
				icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
			}
		}
		// 시너지를 뺄 때
		else
		{
			// 현재 보드판 위에 동일한 기물이 있는 지 확인
			for (int i = 0; i < _Friends.Count; i++)
			{
				if (_Friends[i]._Name == piece._Name)
					return;
			}

			// 보드판 위에 동일한 이름의 모든 기물이 사라졌을 경우 시너지 목록에서 뺀다
			_FriendSynergyName.Remove(piece._Name);

			// 시너지 목록이 0 이상일 때
			if (_FriendSynergy.Count > 0)
			{
				// 중복된 시너지가 존재할 경우
				if (_FriendSynergy.ContainsKey(piece.Attribute))
				{
					// 시너지 1 감소
					_FriendSynergy[piece.Attribute] -= 1;
					icon_instance.setIcon(piece.Attribute, _FriendSynergy[piece.Attribute]);
				}
				// 중복된 시너지가 존재할 경우
				if (_FriendSynergy.ContainsKey(piece.Class))
				{
					// 시너지 1 감소
					_FriendSynergy[piece.Class] -= 1;
					icon_instance.setIcon(piece.Class, _FriendSynergy[piece.Class]);
				}
					
			}
			
			// 만약 시너지 목록에 특정 시너지가 전부 사라졌을 경우
			if (_FriendSynergy[piece.Attribute] <= 0)
				// 시너지 목록에서 해당 시너지 삭제
				_FriendSynergy.Remove(piece.Attribute);
			// 만약 시너지 목록에 특정 시너지가 전부 사라졌을 경우
			if (_FriendSynergy[piece.Class] <= 0)
				// 시너지 목록에서 해당 시너지 삭제
				_FriendSynergy.Remove(piece.Class);
		}
	}

	// 시너지 적용
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
	/// 서치 함수
	/// </summary>
	/// <param name="nowPos">현재 탐색을 실시할 피스의 보드 위치</param>
	/// <returns>적대 피스의 보드의 위치</returns>
	public int SearchPieces(int nowPos, PieceType type)
	{
		// 반환값
		int target = 0;
		// 근사값
		int AppValue = 1000;

		switch (type)
		{
			case PieceType.Friend:
				// 에너미 배열에서 각 에너미와 피스 사이에 거리를 구한다
				for (int i = 0; i < _Enemies.Count; i++)
				{
					if (_Enemies[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// 거리 변수
					int Distance = 0;

					// 두 피스 사이에 열의 거리를 구한다
					int column = (nowPos % 10) - (_Enemies[i].nowBoard.getBoardNum() % 10);
					// 만약 열이 음수로 나올 경우 절대값으로 변환한다
					if (column < 0)
						column *= -1;

					// 나온 열 거리를 최종 거리 변수에 더한다
					Distance += column;

					// 두 피스 사이의 행의 거리를 구한다
					int row = (nowPos / 10) - (_Enemies[i].nowBoard.getBoardNum() / 10);
					// 만약 행이 음수로 나올 경우 절대값으로 변환한다
					if (row < 0)
						row *= -1;

					// 나온 행 거리를 최종 거리 변수에 더한다
					Distance += row;

					// 최종 거리가 현재 근사값과 비교해 짧을 경우 근사값을 최신화 한다.
					if (Distance < AppValue)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Enemies[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Enemies[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Enemies[i].nowBoard.getBoardNum();
					}
				}

				return target;

			case PieceType.Enemy:
				// 에너미 배열에서 각 에너미와 피스 사이에 거리를 구한다
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// 거리 변수
					int Distance = 0;

					// 두 피스 사이에 열의 거리를 구한다
					int column = (nowPos % 10) - (_Friends[i].nowBoard.getBoardNum() % 10);
					// 만약 열이 음수로 나올 경우 절대값으로 변환한다
					if (column < 0)
						column *= -1;

					// 나온 열 거리를 최종 거리 변수에 더한다
					Distance += column;

					// 두 피스 사이의 행의 거리를 구한다
					int row = (nowPos / 10) - (_Friends[i].nowBoard.getBoardNum() / 10);
					// 만약 행이 음수로 나올 경우 절대값으로 변환한다
					if (row < 0)
						row *= -1;

					// 나온 행 거리를 최종 거리 변수에 더한다
					Distance += row;

					// 최종 거리가 현재 근사값과 비교해 짧을 경우 근사값을 최신화 한다.
					if (Distance < AppValue)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Friends[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Friends[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Friends[i].nowBoard.getBoardNum();
					}
				}

				return target;

			case PieceType.Crip:
				// 에너미 배열에서 각 에너미와 피스 사이에 거리를 구한다
				for (int i = 0; i < _Friends.Count; i++)
				{
					if (_Friends[i]._State == Pieces<ChessPiece>.NowState.DEAD)
						continue;

					// 거리 변수
					int Distance = 0;

					// 두 피스 사이에 열의 거리를 구한다
					int column = (nowPos % 10) - (_Friends[i].nowBoard.getBoardNum() % 10);
					// 만약 열이 음수로 나올 경우 절대값으로 변환한다
					if (column < 0)
						column *= -1;

					// 나온 열 거리를 최종 거리 변수에 더한다
					Distance += column;

					// 두 피스 사이의 행의 거리를 구한다
					int row = (nowPos / 10) - (_Friends[i].nowBoard.getBoardNum() / 10);
					// 만약 행이 음수로 나올 경우 절대값으로 변환한다
					if (row < 0)
						row *= -1;

					// 나온 행 거리를 최종 거리 변수에 더한다
					Distance += row;

					// 최종 거리가 현재 근사값과 비교해 짧을 경우 근사값을 최신화 한다.
					if (Distance < AppValue)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Friends[i].nowBoard.getBoardNum();
					}
					else if (Distance == AppValue && (_Friends[i].nowBoard.getBoardNum() / 10) < target / 10)
					{
						// 현재 거리의 근사값을 대입
						AppValue = Distance;
						// 타겟의 보드 좌표를 변경한다
						target = _Friends[i].nowBoard.getBoardNum();
					}
				}

				return target;
		}

		return 0;
	}

	/// <summary>
	/// 타겟팅 함수
	/// </summary>
	/// <param name="boardNum">피스가 자리잡은 보드 넘버</param>
	/// <returns>타겟이 될 피스 정보를 반환</returns>
	public ChessPiece EnemyTargetting(int boardNum, PieceType type)
	{
		switch(type)
		{
			case PieceType.Friend:
				// 에너미 리스트에서 서칭한 보드 넘버를 가진 에너미를 찾아서 반환
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
				// 에너미 리스트에서 서칭한 보드 넘버를 가진 에너미를 찾아서 반환
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
				// 에너미 리스트에서 서칭한 보드 넘버를 가진 에너미를 찾아서 반환
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

	// 보드 안에 블록 여부
	public bool IsBlock(int boardNum)
	{
		if (Boards[boardStature[boardNum]].getEnemy() || Boards[boardStature[boardNum]].getFriend())
		{
			return true;
		}

		return false;
	}

	// 터치한 보드 위의 기물을 선택
	public void SetPiece(GameObject piece)
	{
		_Piece = piece;
		_PieceData = piece.GetComponent<ChessPiece>();

		_DataUI.PickPiece(_PieceData);
	}

	// 현재 선택하고 있는 피스를 반환하고 초기화
	public GameObject GetPiece()
	{
		GameObject piece = _Piece;
		_Piece = null;
		return piece;
	}

	// 현재 선택하고 있는 피스 정보
	public void DeletePieceData()
	{
		_PieceData = null;

		_DataUI.hideUI();
	}

	private void Update()
	{
		if (GM_instance.ShopUI.activeSelf)
			return;

		// 마우스 좌클릭 감지
		if (Input.GetMouseButtonDown(0))
		{
			// 좌클릭 시 현재 마우스 포인트의 월드 좌표 위치로 레이캐스트 발사 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// 레이에 맞은 오브젝트 정보체
			RaycastHit hit;

			// 현재 레이감지용 Plane에 레이캐스트 충돌 시
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// 마우스 포인터 오브젝트의 위치를 레이캐스트 충돌 위치로 변경
				TouchDownVec.transform.position = hit.point;
				// 업 이벤트 충돌 제어 오브젝트를 충돌 판정에서 벗어나도록 제어
				TouchUpVec.transform.position =
					new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			// 좌클릭 시 현재 마우스 포인트의 월드 좌표 위치로 레이캐스트 발사 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// 레이에 맞은 오브젝트 정보체
			RaycastHit hit;

			// 현재 레이감지용 Plane에 레이캐스트 충돌 시
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// 마우스 포인터 오브젝트의 위치를 레이캐스트 충돌 위치로 변경
				TouchUpVec.transform.position = hit.point;
				// 다운 이벤트 충돌 제어 오브젝트를 충돌 판정에서 벗어나도록 제어
				TouchDownVec.transform.position =
					new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}

		if (Input.GetMouseButton(0) && _Piece != null)
		{
			// 좌클릭 시 현재 마우스 포인트의 월드 좌표 위치로 레이캐스트 발사 
			mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

			// 레이에 맞은 오브젝트 정보체
			RaycastHit hit;

			// 현재 레이감지용 Plane에 레이캐스트 충돌 시
			if (mesh.Raycast(mouseRay, out hit, 1000))
			{
				// 마우스 포인터 오브젝트의 위치를 레이캐스트 충돌 위치로 변경
				_Piece.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
			}
		}
	}
}
