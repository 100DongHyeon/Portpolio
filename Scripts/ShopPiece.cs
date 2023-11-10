using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using static PiecePooling;
using UnityEditor.Experimental.GraphView;

public class ShopPiece : MonoBehaviour
{
	// �� �˻��� ���� ��ųʸ�
	Dictionary<ChessPiece.PieceName, int> keyName;

	// ���� �ǽ�
	public GameObject[] pieces;

	// ���� �ǽ� �ִϸ��̼�
	public Animator _anim;

	// ���� �ǽ� ��
	GameObject _piece;
	// ���� �ǽ� ����
	ChessPiece.PieceName _name;
	// ���� ���� ����
	bool _Sold;

	// �ý��� �޼���
	[SerializeField]
	GameObject[] lackMessage;

	// �޼��� �ڷ�ƾ
	IEnumerator GoldMessage;
	IEnumerator BoardMessage;

	/// <summary>
	/// ���� �ǽ� Ȱ��ȭ
	/// </summary>
	/// <param name="name">Ȱ��ȭ�� �ǽ��� ����</param>
	public void SetVis(ChessPiece.PieceName name)
	{
		if (keyName == null)
		{
			// ��ųʸ� �ʱ�ȭ
			keyName = new Dictionary<ChessPiece.PieceName, int>();

			// �ǽ� ������ ���� Ű ��� ����
			for (int i = 1; i < pieces.Length; i++)
			{
				keyName.Add((ChessPiece.PieceName)i, i);
			}
		}

		// ��� ���� �ǽ� ��Ȱ��ȭ
		for (int i = 1; i < pieces.Length; i++)
		{
			pieces[i].SetActive(false);
		}

		_name = name;
		// �ǽ��� Ư��
		_piece = pieces[keyName[name]];

		// ������ �´� ���� �ǽ� Ȱ��ȭ
		_piece.SetActive(true);
		// ��ǰ ����
		_Sold = false;

		// ���� �ִϸ��̼� ���
		_anim = _piece.GetComponent<Animator>();
		_anim.SetTrigger("IsShop");
	}

	/// <summary>
	/// ���� �Լ�
	/// </summary>
	public void Buy()
	{
		// ������ �Ź��� ���� ��
		if (_piece.activeSelf && !_Sold)
		{
			// �ش� �⹰�� ��⿡ ��尡 ����� ��
			if (GM_instance.GoldCheck(_name))
			{
				// ��⼮�� �ڸ��� ����� ��
				if (GM_instance.BoardCheck())
				{
					// �⹰ ����
					_piece.SetActive(false);
					GM_instance.BuyPiece(_name);
					// �Ǹ� �Ϸ�
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
	/// ��� ���� �޼���
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
	/// ��⼮ ���� �޼���
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
