using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static PiecePooling;
using UnityEditor.Experimental.GraphView;

public class ShopPiece : MonoBehaviour
{
	// 모델 검색을 위한 딕셔너리
	Dictionary<ChessPiece.PieceName, int> keyName;

	// 상점 피스
	public GameObject[] pieces;

	// 상점 피스 애니메이션
	public Animator _anim;

	// 상점 피스 모델
	GameObject _piece;
	// 상점 피스 정보
	ChessPiece.PieceName _name;
	// 상점 구매 여부
	bool _Sold;

	// 시스템 메세지
	[SerializeField]
	GameObject[] lackMessage;

	// 메세지 코루틴
	IEnumerator GoldMessage;
	IEnumerator BoardMessage;

	/// <summary>
	/// 상점 피스 활성화
	/// </summary>
	/// <param name="name">활성화할 피스의 종류</param>
	public void SetVis(ChessPiece.PieceName name)
	{
		if (keyName == null)
		{
			// 딕셔너리 초기화
			keyName = new Dictionary<ChessPiece.PieceName, int>();

			// 피스 종류에 맞춰 키 밸류 설정
			for (int i = 1; i < pieces.Length; i++)
			{
				keyName.Add((ChessPiece.PieceName)i, i);
			}
		}

		// 모든 상점 피스 비활성화
		for (int i = 1; i < pieces.Length; i++)
		{
			pieces[i].SetActive(false);
		}

		_name = name;
		// 피스를 특정
		_piece = pieces[keyName[name]];

		// 종류에 맞는 상점 피스 활성화
		_piece.SetActive(true);
		// 상품 재등록
		_Sold = false;

		// 최초 애니메이션 재생
		_anim = _piece.GetComponent<Animator>();
		_anim.SetTrigger("IsShop");
	}

	/// <summary>
	/// 구매 함수
	/// </summary>
	public void Buy()
	{
		// 상점에 매물이 있을 때
		if (_piece.activeSelf && !_Sold)
		{
			// 해당 기물을 사기에 골드가 충분할 때
			if (GM_instance.GoldCheck(_name))
			{
				// 대기석에 자리가 충분할 때
				if (GM_instance.BoardCheck())
				{
					// 기물 구매
					_piece.SetActive(false);
					GM_instance.BuyPiece(_name);
					// 판매 완료
					_Sold = true;
				}
				else
				{
					if (BoardMessage == null)
						BoardMessage = lackBoard();

					StopCoroutine(BoardMessage);
					StartCoroutine(BoardMessage);
				}
			}
			else
			{
				if (GoldMessage == null)
					GoldMessage = lackGold();

				StopCoroutine(GoldMessage);
				StartCoroutine(GoldMessage);
			}
		}
	}

	/// <summary>
	/// 골드 부족 메세지
	/// </summary>
	/// <returns></returns>
	IEnumerator lackGold()
	{
		float time = 0;

		lackMessage[0].SetActive(true);

		while (true)
		{
			if (time > 3)
				break;

			time += Time.deltaTime;

			yield return null;
		}

		lackMessage[0].SetActive(false);
	}

	/// <summary>
	/// 대기석 부족 메세지
	/// </summary>
	/// <returns></returns>
	IEnumerator lackBoard()
	{
		float time = 0;

		lackMessage[1].SetActive(true);

		while (true)
		{
			if (time > 3)
				break;

			time += Time.deltaTime;

			yield return null;
		}

		lackMessage[1].SetActive(false);
	}
}
