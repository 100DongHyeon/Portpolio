using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static BoardManager;
using static PiecePooling;

public class AIplayer : MonoBehaviour
{
	public static AIplayer AI_instance = null;

	private void Awake()
	{
		if (AI_instance == null)
			AI_instance = this;
	}

	public enum ChessMater
    {
		Conqueror
	}

	ChessMater _AI;

	// 적 웨이팅 피스 리스트
	public List<ChessPiece> _waitEnemy;
	// 적 보드 리스트
	public List<GameObject> _enemyList;
	// 적 크립 리스트
	public List<GameObject> _cripList;

	[SerializeField]
	GameObject[] Crips;

	private void Start()
	{
		_AI = (ChessMater)Random.RandomRange(0, 1);
		_waitEnemy = new List<ChessPiece>();
		_enemyList = new List<GameObject>();
		_cripList = new List<GameObject>();
	}

	public void ActToRound(int round)
	{
		for (int i = 0; i < BM_instance._Enemies.Count; i++)
		{
			BM_instance._Enemies[i].gameObject.SetActive(true);

			if (BM_instance._Enemies[i].nowBoard != null)
				BM_instance._Enemies[i].OutBoard(BM_instance._Enemies[i].Type);
		}

		BM_instance._Enemies.Clear();

		for (int i = 0; i < _cripList.Count; i++)
		{
			Destroy(_cripList[i]);
		}
		_cripList.Clear();

		for (int i = 0; i < _enemyList.Count; i++)
		{
			int boardNum = _enemyList[i].GetComponent<Pieces<ChessPiece>>().Fun_oriPos;
			_enemyList[i].GetComponent<Pieces<ChessPiece>>().InEnemyBoard(boardNum);
		}

		CripRound(round);

		switch(_AI)
		{
			case ChessMater.Conqueror:
				ConquerorSetting(round);
				break;
		}
	}

	public void ActToBattle()
	{
		if (_cripList.Count <= 0)
		{
			List<Pieces<ChessPiece>> list = new List<Pieces<ChessPiece>>();

			for (int i = 0; i < _enemyList.Count; i++)
			{
				Pieces<ChessPiece> script = _enemyList[i].GetComponent<Pieces<ChessPiece>>();

				script.Fun_oriPos = list[i].nowBoard.getBoardNum();
			}
		}
	}

	void CripRound(int round)
	{
		// 크립 오브젝트
		GameObject[] crips = new GameObject[6];

		// 크립 라운드에 따라
		switch (round)
		{
			// 1라운드
			case 1:
				for (int i = 0; i < 2; i++)
				{
					crips[i] = Instantiate(Crips[0]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[i]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				crips[0].GetComponent<ChessPiece>().InEnemyBoard(84);
				crips[1].GetComponent<ChessPiece>().InEnemyBoard(85);
				break;
			// 2라운드
			case 2:
				for (int i = 0; i < 2; i++)
				{
					crips[i] = Instantiate(Crips[0]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[i]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				for (int i = 2; i < 4; i++)
				{
					crips[i] = Instantiate(Crips[1]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[i]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				crips[0].GetComponent<ChessPiece>().InEnemyBoard(83);
				crips[1].GetComponent<ChessPiece>().InEnemyBoard(86);
				crips[2].GetComponent<ChessPiece>().InEnemyBoard(64);
				crips[3].GetComponent<ChessPiece>().InEnemyBoard(65);
				break;
			// 3라운드
			case 3:
				for (int i = 0; i < 4; i++)
				{
					crips[i] = Instantiate(Crips[0]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[i]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				for (int i = 4; i < 6; i++)
				{
					crips[i] = Instantiate(Crips[1]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[i]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				crips[0].GetComponent<ChessPiece>().InEnemyBoard(73);
				crips[1].GetComponent<ChessPiece>().InEnemyBoard(76);
				crips[2].GetComponent<ChessPiece>().InEnemyBoard(63);
				crips[3].GetComponent<ChessPiece>().InEnemyBoard(67);
				crips[4].GetComponent<ChessPiece>().InEnemyBoard(54);
				crips[5].GetComponent<ChessPiece>().InEnemyBoard(55);
				break;
			// 10라운드
			case 10:
				for (int i = 0; i < 2; i++)
				{
					crips[0] = Instantiate(Crips[2]);
					ChessPiece script = crips[i].GetComponent<ChessPiece>();
					script.PieceInit(script._Name, script._CripName, script.Type);
					_cripList.Add(crips[0]);
					_cripList[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				crips[2] = Instantiate(Crips[3]);
				ChessPiece script2 = crips[2].GetComponent<ChessPiece>();
				script2.PieceInit(script2._Name, script2._CripName, script2.Type);
				_cripList.Add(crips[2]);
				_cripList[2].GetComponent<ChessPiece>().PieceRotate(180);

				crips[0].GetComponent<ChessPiece>().InEnemyBoard(62);
				crips[1].GetComponent<ChessPiece>().InEnemyBoard(67);
				crips[2].GetComponent<ChessPiece>().InEnemyBoard(55);
				break;
			case 15:

				break;
			case 20:

				break;
			case 25:

				break;
			case 30:

				break;
			case 35:

				break;
			case 40:

				break;
			default:
				break;
		}
	}

	/// <summary>
	/// 정복자 AI 세팅
	/// </summary>
	/// <param name="round">현재 라운드</param>
	void ConquerorSetting(int round)
	{
		GameObject[] pieces = new GameObject[10];
		ChessPiece[] scripts = new ChessPiece[10];

		switch (round)
		{
			case 4:
				for (int i = 0; i < 2; i++)
				{
					pieces[i] = Pool_Instance.poolPiece(ChessPiece.PieceName.PootMan, 
														ChessPiece.CripName.None,
														PieceType.Enemy);
					pieces[i].SetActive(true);
					_enemyList.Add(pieces[i]);
					pieces[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				pieces[0].GetComponent<ChessPiece>().InEnemyBoard(54);
				pieces[1].GetComponent<ChessPiece>().InEnemyBoard(55);
				break;
			case 6:
				for (int i = 0; i < 2; i++)
				{
					pieces[i] = Pool_Instance.poolPiece(ChessPiece.PieceName.PootMan, 
														ChessPiece.CripName.None,		
														PieceType.Enemy);
					pieces[i].SetActive(true);
					_enemyList.Add(pieces[i]);
					pieces[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				pieces[0].GetComponent<ChessPiece>().InEnemyBoard(53);
				pieces[0].GetComponent<ChessPiece>().InEnemyBoard(56);
				break;
			case 11:
				for (int i = 0; i < 4; i++)
				{
					pieces[i] = Pool_Instance.poolPiece(ChessPiece.PieceName.PootMan, 
														ChessPiece.CripName.None, 
														PieceType.Enemy);
					pieces[i].SetActive(true);
					pieces[i].GetComponent<ChessPiece>().PieceRotate(180);
				}
				scripts[0] = _enemyList[0].GetComponent<ChessPiece>();
				
				scripts[0].StarSet(Pieces<ChessPiece>.PieceStar.twoStar);
				scripts[0].inStorage(pieces[0].GetComponent<ChessPiece>());
				scripts[0].inStorage(pieces[1].GetComponent<ChessPiece>());
				
				scripts[1] = _enemyList[1].GetComponent<ChessPiece>();

				scripts[1].StarSet(Pieces<ChessPiece>.PieceStar.threeStar);
				scripts[1].inStorage(pieces[2].GetComponent<ChessPiece>());
				scripts[1].inStorage(pieces[3].GetComponent<ChessPiece>());
				break;
		}

		if (3 < round && round < 10)
		{

		}
		else if (10 < round && round < 15)
		{

		}
		else if (15 < round && round < 20)
		{

		}
		else if (20 < round && round < 25)
		{

		}
		else if (25 < round && round < 30)
		{

		}
		else if (30 < round && round < 35)
		{

		}
		else if (35 < round && round < 40)
		{

		}
	}
}
