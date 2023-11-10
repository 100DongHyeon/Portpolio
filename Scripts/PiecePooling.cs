using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;

public class PiecePooling : MonoBehaviour
{
	public static PiecePooling Pool_Instance = null;

	// T1 Ȯ��
	public List<GameObject> T1 = new List<GameObject>();
	// ǲ�� ������Ʈ Ǯ
	List<GameObject> T1_PootManPool = new List<GameObject>();
	public GameObject pootman;
	// �ذ����� ������Ʈ Ǯ
	List<GameObject> T1_SkelWarriorPool = new List<GameObject>();
	public GameObject skelwarrior;
	// ���ܱ�� ������Ʈ Ǯ
	List<GameObject> T1_BeeKnghitPool = new List<GameObject>();
	public GameObject beeknghit;
	// �Ǹ���� ������Ʈ Ǯ
	List<GameObject> T1_DemonGuardPool = new List<GameObject>();
	public GameObject demonguard;
	// �Ǹ���� ������Ʈ Ǯ
	List<GameObject> T1_TraineeWizardPool = new List<GameObject>();
	public GameObject traineewizard;

	// T2 Ȯ��
	public List<GameObject> T2 = new List<GameObject>();
	// ��ó ������Ʈ Ǯ
	List<GameObject> T2_ArhcerPool = new List<GameObject>();
	public GameObject archer;
	// ���� ���� ������Ʈ Ǯ
	List<GameObject> T2_AxeWarriorPool = new List<GameObject>();
	public GameObject axewarrior;
	// �ذ� ������ ������Ʈ Ǯ
	List<GameObject> T2_UndeadMagePool = new List<GameObject>();
	public GameObject undeadmage;
	// �Ǹ��� �� ������Ʈ Ǯ
	List<GameObject> T2_EvilEyePool = new List<GameObject>();
	public GameObject evileye;
	// ���� ��� ������Ʈ Ǯ
	List<GameObject> T2_PhantomKnightPool = new List<GameObject>();
	public GameObject phantomknight;

	// T3 Ȯ��
	public List<GameObject> T3 = new List<GameObject>();
	// �߼� ���� ������Ʈ Ǯ
	List<GameObject> T3_BeastManPool = new List<GameObject>();
	public GameObject beastman;
	// ������ ������Ʈ Ǯ
	List<GameObject> T3_BerserkerPool = new List<GameObject>();
	public GameObject berserker;
	// �ҾǸ� ������Ʈ Ǯ
	List<GameObject> T3_ImpPool = new List<GameObject>();
	public GameObject imp;
	// ��ö ��� ������Ʈ Ǯ
	List<GameObject> T3_MetalKnightPool = new List<GameObject>();
	public GameObject metalknight;
	// �߸� ���� ������Ʈ Ǯ
	List<GameObject> T3_OrcWarriorPool = new List<GameObject>();
	public GameObject orcwarrior;

	// T4 Ȯ��
	public List<GameObject> T4 = new List<GameObject>();
	// ��ü ������ ������Ʈ Ǯ
	List<GameObject> T4_CorpseCollecterPool = new List<GameObject>();
	public GameObject corpsecollecter;
	// ������ ��� ������Ʈ Ǯ
	List<GameObject> T4_DeathKnightPool = new List<GameObject>();
	public GameObject deathkight;
	// ���� ���� ������Ʈ Ǯ
	List<GameObject> T4_DruidPool = new List<GameObject>();
	public GameObject druid;
	// ���� ��� ������Ʈ Ǯ
	List<GameObject> T4_ElvenKnightPool = new List<GameObject>();
	public GameObject elvenknight;

	// T5 Ȯ��
	public List<GameObject> T5 = new List<GameObject>();
	// ���� ���� ������Ʈ Ǯ
	List<GameObject> T5_BloodLordPool = new List<GameObject>();
	public GameObject bloodlord;
	// �Žź� ������Ʈ Ǯ
	List<GameObject> T5_GolemPool = new List<GameObject>();
	public GameObject golem;
	// ��ź�� ��� ������Ʈ Ǯ
	List<GameObject> T5_MagicShooterPool = new List<GameObject>();
	public GameObject magicshooter;


	// �̱���
	private void Awake()
	{
		if (Pool_Instance == null)
			Pool_Instance = this;
	}

	// ��� �ǽ� �ϰ� ����
	private void Start()
	{
		PiecePoolInit(27, pootman, "PootManPool", T1_PootManPool);
		PiecePoolInit(27, skelwarrior, "SkelWarriorPool", T1_SkelWarriorPool);
		PiecePoolInit(27, beeknghit, "BeeKnghitPool", T1_BeeKnghitPool);
		PiecePoolInit(27, demonguard, "DemonGuardPool", T1_DemonGuardPool);
		PiecePoolInit(27, traineewizard, "TraineeWizardPool", T1_TraineeWizardPool);
		PiecePoolInit(24, archer, "ArcherPool", T2_ArhcerPool);
		PiecePoolInit(24, axewarrior, "AxeWarriorPool", T2_AxeWarriorPool);
		PiecePoolInit(24, undeadmage, "UndeadMagePool", T2_UndeadMagePool);
		PiecePoolInit(24, evileye, "EvilEyePool", T2_EvilEyePool);
		PiecePoolInit(24, phantomknight, "PhantomKnightPool", T2_PhantomKnightPool);
		PiecePoolInit(18, beastman, "BeastManPool", T3_BeastManPool);	
		PiecePoolInit(18, berserker, "BerserkerPool", T3_BerserkerPool);
		PiecePoolInit(18, imp, "ImpPool", T3_ImpPool);
		PiecePoolInit(18, metalknight, "MetalKnightPool", T3_MetalKnightPool);
		PiecePoolInit(18, orcwarrior, "OrcWarriorPool", T3_OrcWarriorPool);
		PiecePoolInit(15, corpsecollecter, "CorpseCollecterPool", T4_CorpseCollecterPool);
		PiecePoolInit(15, deathkight, "DeathKnightPool", T4_DeathKnightPool);
		PiecePoolInit(15, druid, "DruidPool", T4_DruidPool);
		PiecePoolInit(15, elvenknight, "ElvenKnightPool", T4_ElvenKnightPool);
		PiecePoolInit(12, bloodlord, "BloodLordPool", T5_BloodLordPool);
		PiecePoolInit(12, golem, "GolemPool", T5_GolemPool);
		PiecePoolInit(12, magicshooter, "MagicShooterPool", T5_MagicShooterPool);
	}

	/// <summary>
	/// Ǯ �ʱ�ȭ �Լ�
	/// </summary>
	/// <param name="num">�� �ǽ��� ����</param>
	/// <param name="obj">�ǽ��� ����</param>
	/// <param name="name">�ǽ� Ǯ�� ���� ������Ʈ</param>
	/// <param name="pool">�ǽ� Ǯ ����Ʈ</param>
	void PiecePoolInit(int num, GameObject obj, string name, List<GameObject> pool)
	{
		for (int i = 0; i < num; i++)
		{
			GameObject piece = Instantiate(obj);

			piece.SetActive(false);
			piece.transform.parent = GameObject.Find(name).transform;

			pool.Add(piece);
		}
	}

	/// <summary>
	/// �ǽ� Ǯ�� �Լ�
	/// </summary>
	/// <param name="name">Ǯ���� �ǽ�</param>
	/// <returns></returns>
	public GameObject poolPiece(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		// �ǽ��� ������ ���� ��ȯ
		switch (name)
		{
			case ChessPiece.PieceName.PootMan:
				piece = poolPootMan(name, crip, type);
				break;
			case ChessPiece.PieceName.SkelWarrior:
				piece = poolSkelWarrior(name, crip, type);
				break;
			case ChessPiece.PieceName.BeeKnghit:
				piece = poolBeeKnghit(name, crip, type);
				break;
			case ChessPiece.PieceName.DemonGuard:
				piece = poolDemonGuard(name, crip, type);
				break;
			case ChessPiece.PieceName.TraineeWizard:
				piece = poolTraineeWizard(name, crip, type);
				break;
			case ChessPiece.PieceName.Archer:
				piece = poolArcher(name, crip, type);
				break;
			case ChessPiece.PieceName.AxeWarrior:
				piece = poolAxeWarrior(name, crip, type);
				break;
			case ChessPiece.PieceName.UndeadMage:
				piece = poolUndeadMage(name, crip, type);
				break;
			case ChessPiece.PieceName.EvilEye:
				piece = poolEvilEye(name, crip, type);
				break;
			case ChessPiece.PieceName.PhantomKnight:
				piece = poolPhatomKnight(name, crip, type);
				break;
			case ChessPiece.PieceName.BesatMan:
				piece = poolBeastMan(name, crip, type);
				break;
			case ChessPiece.PieceName.Berserker:
				piece = poolBerserker(name, crip, type);
				break;
			case ChessPiece.PieceName.Imp:
				piece = poolImp(name, crip, type);
				break;
			case ChessPiece.PieceName.MetalKnight:
				piece = poolMetalKnight(name, crip, type);
				break;
			case ChessPiece.PieceName.OrcWarrior:
				piece = poolOrcWarrior(name, crip, type);
				break;
			case ChessPiece.PieceName.DeathKnight:
				piece = poolDeathKnight(name, crip, type);
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				piece = poolCorpseCollecter(name, crip, type);
				break;
			case ChessPiece.PieceName.ElvenKnight:
				piece = poolElvenKnight(name, crip, type);
				break;
			case ChessPiece.PieceName.Druid:
				piece = poolDruid(name, crip, type);
				break;
			case ChessPiece.PieceName.BloodLord:
				piece = poolBloodLord(name, crip, type);
				break;
			case ChessPiece.PieceName.Golem:
				piece = poolGolem(name, crip, type);
				break;
			case ChessPiece.PieceName.MagicShooter:
				piece = poolMagicShooter(name, crip, type);
				break;
		}

		return piece;
	}

	/// <summary>
	/// �ǽ� Ǯ ��ȯ �Լ�
	/// </summary>
	/// <param name="piece">��ȯ�� �ǽ�</param>
	public void resetPieceList(ChessPiece piece)
	{
		// �ǽ��� ������ �ʱ�ȭ
		piece.PieceInit(piece.GetComponent<ChessPiece>()._Name,
						piece.GetComponent<ChessPiece>()._CripName,
						PieceType.Friend);
		piece.StarSet(Pieces<ChessPiece>.PieceStar.oneStar);

		// �ǽ��� ��Ȱ��ȭ
		piece.gameObject.SetActive(false);

		// �ǽ��� ������ ���� �ش� �ǽ� Ǯ�� ��ȯ
		switch (piece._Name)
		{
			case ChessPiece.PieceName.PootMan:
				T1_PootManPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.SkelWarrior:
				T1_SkelWarriorPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.BeeKnghit:
				T1_BeeKnghitPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.DemonGuard:
				T1_DemonGuardPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.TraineeWizard:
				T1_TraineeWizardPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.Archer:
				T2_ArhcerPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.AxeWarrior:
				T2_AxeWarriorPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.UndeadMage:
				T2_UndeadMagePool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.EvilEye:
				T2_EvilEyePool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.PhantomKnight:
				T2_PhantomKnightPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.BesatMan:
				T3_BeastManPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.Berserker:
				T3_BerserkerPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.Imp:
				T3_ImpPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.MetalKnight:
				T3_MetalKnightPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.OrcWarrior:
				T3_OrcWarriorPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.DeathKnight:
				T4_DeathKnightPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				T4_CorpseCollecterPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.ElvenKnight:
				T4_ElvenKnightPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.Druid:
				T4_DruidPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.BloodLord:
				T5_BloodLordPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.Golem:
				T5_GolemPool.Add(piece.gameObject);
				break;
			case ChessPiece.PieceName.MagicShooter:
				T5_MagicShooterPool.Add(piece.gameObject);
				break;
		}
	}

	/// <summary>
	/// 1Ƽ�� �⹰ Ȯ��
	/// </summary>
	/// <returns></returns>
	public ChessPiece.PieceName setPieceT1()
	{
		// ���� ��Ȱ��ȭ ���� ��� 1Ƽ�� �⹰���� �����´�
		{
			for (int i = 0; i < T1_PootManPool.Count; i++)
			{
				T1.Add(T1_PootManPool[i]);
			}
			for (int i = 0; i < T1_SkelWarriorPool.Count; i++)
			{
				T1.Add(T1_SkelWarriorPool[i]);
			}
			for (int i = 0; i < T1_BeeKnghitPool.Count; i++)
			{
				T1.Add(T1_BeeKnghitPool[i]);
			}
			for (int i = 0; i < T1_DemonGuardPool.Count; i++)
			{
				T1.Add(T1_DemonGuardPool[i]);
			}
			for (int i = 0; i < T1_TraineeWizardPool.Count; i++)
			{
				T1.Add(T1_TraineeWizardPool[i]);
			}
		}

		// ���� ��Ȱ��ȭ ���� 1Ƽ�� �⹰�� �� �ϳ��� ���� ������ ÷�� ����
		int r = Random.RandomRange(0, T1.Count);

		// ������ �ø� �⹰ ������ ��ȯ
		ChessPiece.PieceName name = T1[r].GetComponent<ChessPiece>()._Name;
		T1.Clear();
		return name;
	}

	/// <summary>
	/// Ƽ�� �⹰ Ȯ��
	/// </summary>
	/// <returns></returns>
	public ChessPiece.PieceName setPieceT2()
	{
		{
			// ���� ��Ȱ��ȭ ���� ��� 2Ƽ�� �⹰���� �����´�
			for (int i = 0; i < T2_ArhcerPool.Count; i++)
			{
				T2.Add(T2_ArhcerPool[i]);
			}
			for (int i = 0; i < T2_AxeWarriorPool.Count; i++)
			{
				T2.Add(T2_AxeWarriorPool[i]);
			}
			for (int i = 0; i < T2_UndeadMagePool.Count; i++)
			{
				T2.Add(T2_UndeadMagePool[i]);
			}
			for (int i = 0; i < T2_EvilEyePool.Count; i++)
			{
				T2.Add(T2_EvilEyePool[i]);
			}
			for (int i = 0; i < T2_PhantomKnightPool.Count; i++)
			{
				T2.Add(T2_PhantomKnightPool[i]);
			}
		}
		

		// ���� ��Ȱ��ȭ ���� 1Ƽ�� �⹰�� �� �ϳ��� ���� ������ ÷�� ����
		int r = Random.RandomRange(0, T2.Count);

		// ������ �ø� �⹰ ������ ��ȯ
		ChessPiece.PieceName name = T2[r].GetComponent<ChessPiece>()._Name;
		T2.Clear();

		return name;
	}

	/// <summary>
	/// 3Ƽ�� �⹰ Ȯ��
	/// </summary>
	/// <returns></returns>
	public ChessPiece.PieceName setPieceT3()
	{
		{
			// ���� ��Ȱ��ȭ ���� ��� 3Ƽ�� �⹰���� �����´�
			for (int i = 0; i < T3_BeastManPool.Count; i++)
			{
				T3.Add(T3_BeastManPool[i]);
			}
			for (int i = 0; i < T3_BerserkerPool.Count; i++)
			{
				T3.Add(T3_BerserkerPool[i]);
			}
			for (int i = 0; i < T3_ImpPool.Count; i++)
			{
				T3.Add(T3_ImpPool[i]);
			}
			for (int i = 0; i < T3_MetalKnightPool.Count; i++)
			{
				T3.Add(T3_MetalKnightPool[i]);
			}
			for (int i = 0; i < T3_OrcWarriorPool.Count; i++)
			{
				T3.Add(T3_OrcWarriorPool[i]);
			}
		}


		// ���� ��Ȱ��ȭ ���� 1Ƽ�� �⹰�� �� �ϳ��� ���� ������ ÷�� ����
		int r = Random.RandomRange(0, T3.Count);

		// ������ �ø� �⹰ ������ ��ȯ
		ChessPiece.PieceName name = T3[r].GetComponent<ChessPiece>()._Name;
		T3.Clear();

		return name;
	}

	/// <summary>
	/// 4Ƽ�� �⹰ Ȯ��
	/// </summary>
	/// <returns></returns>
	public ChessPiece.PieceName setPieceT4()
	{
		{
			// ���� ��Ȱ��ȭ ���� ��� 3Ƽ�� �⹰���� �����´�
			for (int i = 0; i < T4_CorpseCollecterPool.Count; i++)
			{
				T4.Add(T4_CorpseCollecterPool[i]);
			}
			for (int i = 0; i < T4_DeathKnightPool.Count; i++)
			{
				T4.Add(T4_DeathKnightPool[i]);
			}
			for (int i = 0; i < T4_DruidPool.Count; i++)
			{
				T4.Add(T4_DruidPool[i]);
			}
			for (int i = 0; i < T4_ElvenKnightPool.Count; i++)
			{
				T4.Add(T4_ElvenKnightPool[i]);
			}
		}


		// ���� ��Ȱ��ȭ ���� 1Ƽ�� �⹰�� �� �ϳ��� ���� ������ ÷�� ����
		int r = Random.RandomRange(0, T4.Count);

		// ������ �ø� �⹰ ������ ��ȯ
		ChessPiece.PieceName name = T4[r].GetComponent<ChessPiece>()._Name;
		T4.Clear();

		return name;
	}

	/// <summary>
	/// 5Ƽ�� �⹰ Ȯ��
	/// </summary>
	/// <returns></returns>
	public ChessPiece.PieceName setPieceT5()
	{
		{
			// ���� ��Ȱ��ȭ ���� ��� 3Ƽ�� �⹰���� �����´�
			for (int i = 0; i < T5_BloodLordPool.Count; i++)
			{
				T5.Add(T5_BloodLordPool[i]);
			}
			for (int i = 0; i < T5_GolemPool.Count; i++)
			{
				T5.Add(T5_GolemPool[i]);
			}
			for (int i = 0; i < T5_MagicShooterPool.Count; i++)
			{
				T5.Add(T5_MagicShooterPool[i]);
			}
		}


		// ���� ��Ȱ��ȭ ���� 1Ƽ�� �⹰�� �� �ϳ��� ���� ������ ÷�� ����
		int r = Random.RandomRange(0, T5.Count);

		// ������ �ø� �⹰ ������ ��ȯ
		ChessPiece.PieceName name = T5[r].GetComponent<ChessPiece>()._Name;
		T5.Clear();

		return name;
	}

	GameObject poolPootMan(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T1_PootManPool.Count > 0)
		{
			piece = T1_PootManPool[0];
			T1_PootManPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolSkelWarrior(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T1_SkelWarriorPool.Count > 0)
		{
			piece = T1_SkelWarriorPool[0];
			T1_SkelWarriorPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolBeeKnghit(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T1_BeeKnghitPool.Count > 0)
		{
			piece = T1_BeeKnghitPool[0];
			T1_BeeKnghitPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolDemonGuard(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T1_DemonGuardPool.Count > 0)
		{
			piece = T1_DemonGuardPool[0];
			T1_DemonGuardPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolTraineeWizard(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T1_TraineeWizardPool.Count > 0)
		{
			piece = T1_TraineeWizardPool[0];
			T1_TraineeWizardPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolArcher(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T2_ArhcerPool.Count > 0)
		{
			piece = T2_ArhcerPool[0];
			T2_ArhcerPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip ,type);

		return piece;
	}

	GameObject poolAxeWarrior(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T2_AxeWarriorPool.Count > 0)
		{
			piece = T2_AxeWarriorPool[0];
			T2_AxeWarriorPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolUndeadMage(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T2_UndeadMagePool.Count > 0)
		{
			piece = T2_UndeadMagePool[0];
			T2_UndeadMagePool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolEvilEye(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T2_EvilEyePool.Count > 0)
		{
			piece = T2_EvilEyePool[0];
			T2_EvilEyePool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolPhatomKnight(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T2_PhantomKnightPool.Count > 0)
		{
			piece = T2_PhantomKnightPool[0];
			T2_PhantomKnightPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolBeastMan(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T3_BeastManPool.Count > 0)
		{
			piece = T3_BeastManPool[0];
			T3_BeastManPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolBerserker(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T3_BerserkerPool.Count > 0)
		{
			piece = T3_BerserkerPool[0];
			T3_BerserkerPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolImp(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T3_ImpPool.Count > 0)
		{
			piece = T3_ImpPool[0];
			T3_ImpPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolMetalKnight(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T3_MetalKnightPool.Count > 0)
		{
			piece = T3_MetalKnightPool[0];
			T3_MetalKnightPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolOrcWarrior(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T3_OrcWarriorPool.Count > 0)
		{
			piece = T3_OrcWarriorPool[0];
			T3_OrcWarriorPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolDeathKnight(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T4_DeathKnightPool.Count > 0)
		{
			piece = T4_DeathKnightPool[0];
			T4_DeathKnightPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolCorpseCollecter(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T4_CorpseCollecterPool.Count > 0)
		{
			piece = T4_CorpseCollecterPool[0];
			T4_CorpseCollecterPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolDruid(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T4_DruidPool.Count > 0)
		{
			piece = T4_DruidPool[0];
			T4_DruidPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolElvenKnight(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T4_ElvenKnightPool.Count > 0)
		{
			piece = T4_ElvenKnightPool[0];
			T4_ElvenKnightPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolBloodLord(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T5_BloodLordPool.Count > 0)
		{
			piece = T5_BloodLordPool[0];
			T5_BloodLordPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolGolem(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T5_GolemPool.Count > 0)
		{
			piece = T5_GolemPool[0];
			T5_GolemPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

	GameObject poolMagicShooter(ChessPiece.PieceName name, ChessPiece.CripName crip, PieceType type)
	{
		GameObject piece = null;

		if (T5_MagicShooterPool.Count > 0)
		{
			piece = T5_MagicShooterPool[0];
			T5_MagicShooterPool.Remove(piece);
		}
		piece.GetComponent<ChessPiece>().PieceInit(name, crip, type);

		return piece;
	}

}
