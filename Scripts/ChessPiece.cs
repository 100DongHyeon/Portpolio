using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BoardManager;
using static Pieces<ChessPiece>;
using static GameManager;
using static SkillManager;
using Unity.VisualScripting;

public class ChessPiece : Pieces<ChessPiece>
{
	public enum PieceName
	{
		None,
		PootMan,
		SkelWarrior,
		BeeKnghit,
		DemonGuard,
		TraineeWizard,
		Archer,
		AxeWarrior,
		UndeadMage,
		EvilEye,
		PhantomKnight,
		Berserker,
		Imp,
		OrcWarrior,
		MetalKnight,
		BesatMan,
		DeathKnight,
		ElvenKnight,
		CorpseCollecter,
		Druid,
		MagicShooter,
		Golem,
		BloodLord
	}

	public enum CripName
	{
		None,
		Slime,
		ArmorSlime
	}

	public PieceName _Name;
	public CripName _CripName;

	// Ÿ�� ���ʹ�
	public ChessPiece targetPiece;

	// Ÿ�� ���� �ѹ�
	int target;

	// ��ų ��� ����
	[SerializeField]
	bool noSkill;
	// ���Ÿ� ����
	[SerializeField]
	bool longRange;

	// ���� ���� ������
	float curDelay = 0;
	// ���� ���� ����
	[SerializeField]
	bool isAttack;
	// ���� �ݶ��̴�
	[SerializeField]
	Collider collider;
	// ���Ÿ� ����
	[SerializeField]
	Shoot shotBullet;

	// ������ ������Ʈ
	[SerializeField]
	GameObject[] _SlashEffecter;

	// ��Ʈ ������Ʈ
	[SerializeField]
	HitEff _HitEffecter;

	// ��ų ������Ʈ
	public GameObject[] skillBullet;

	int DG_Counter = 0;

	public void BattleStart()
	{
		_State = NowState.IDLE;
	}

	public void BattleEnd()
	{
		_anim.SetTrigger("EndBattle");
		// ���� ����
		_State = NowState.END;
	}

	public void Preparing()
	{
		_anim.SetTrigger("GoIdle");
		_State = NowState.START;
		isAttack = false;
	}

	private void Update()
	{
		// Ư�� ��Ȳ���� ���� �� X
		if (_State == NowState.DEAD || _State == NowState.END || _State == NowState.START)
			return;

		// Ÿ���� ������ ��
		if (targetPiece != null)
		{
			// Ÿ���� ������� �� ���ʹ� ��Ž�� ���� (���ʹ� ����)
			if (targetPiece._State == NowState.DEAD)
			{
				targetPiece = null;
				_State = NowState.IDLE;
			}
		}

		// ���� �ִϸ��̼� ���� ���� ���� Ȯ��
		if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
			0.95f <= _anim.GetCurrentAnimatorStateInfo(0).normalizedTime)
		{
			isAttack = false;

			if (_Name == PieceName.DemonGuard && getIsSkill())
				setIsSkill(false);
		}


		switch (_State)
		{
			case NowState.IDLE:
				// ����� �� Ž��
				target = BM_instance.SearchPieces(nowBoard.getBoardNum(), Type);
				// Ÿ�� ����
				targetPiece = BM_instance.EnemyTargetting(target, Type);

				if (!isAttack && !getIsSkill())
					autoRotate();

				// �̵� ����
				_State = NowState.MOVE; ;

				break;
			case NowState.MOVE:
				// �̵� ���� �ƴҶ�, ���� ���� �ƴ� ��
				if (!isMove && !isAttack)
				{
					if (!getIsSkill())
						autoRotate();

					// �̵� ����
					StartCoroutine(Move(targetPiece.nowBoard));

					curDelay = 0.5f;
				}
				break;
			case NowState.BATTLE:
				// ��ų �ߵ� ���� �ƴϰ� ���� �����̸� ������ ��
				if (!getIsSkill() && curDelay > Fun_nowDelay)
				{
					_anim.SetTrigger("IsAttack");

					// ���� ���� ȸ��
					Fun_curMP = Fun_curMP + 3;

					// ������ �ʱ�ȭ
					curDelay = 0;

					if (!longRange)
					{
						autoRotate();

						if (_Name == PieceName.DemonGuard && DG_Counter >= 5)
							// ��ų ��� �� Battle ���� ����
							setIsSkill(true);

						// Ÿ�� ��Ʈ �ݶ��̴� On
						collider.gameObject.GetComponent<AttackScript>().targetSetting(targetPiece);
						collider.enabled = true;

						SlashEff();
					}
					else
					{
						// ���Ÿ� ���� ����ü �߻� ������
						StartCoroutine(shootBullet());
					}

					isAttack = true;
				}

				if (!isAttack && Fun_maxMP < Fun_curMP && !noSkill)
					_State = NowState.SKILL;

				// ������ Ÿ��
				if (!isAttack)
					curDelay += Time.deltaTime;

				break;
			case NowState.SKILL:
				if (!getIsSkill())
				{
					autoRotate();

					// ��ų �ߵ� �Լ�
					Skill();
				}
				else
				{
					curDelay += Time.deltaTime;

					if (Fun_skillTime < curDelay)
						SkillEnd();
				}

				break;
		}
	}

	IEnumerator shootBullet()
	{
		switch (_Name)
		{
			case PieceName.TraineeWizard:
				yield return new WaitForSeconds(0.4f);
				// ���Ÿ� ���� �⹰ ���� ����
				longRotate();
				// ���Ÿ� ����
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.Archer:
				yield return new WaitForSeconds(0.8f);
				// ���Ÿ� ���� �⹰ ���� ����
				longRotate();
				// ���Ÿ� ����
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.UndeadMage:
				yield return new WaitForSeconds(0.4f);
				// ���Ÿ� ���� �⹰ ���� ����
				longRotate();
				// ���Ÿ� ����
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.Druid:
				yield return new WaitForSeconds(0.4f);
				// ���Ÿ� ���� �⹰ ���� ����
				longRotate();
				// ���Ÿ� ����
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.MagicShooter:
				yield return new WaitForSeconds(0.8f);
				// ���Ÿ� ���� �⹰ ���� ����
				longRotate();
				// ���Ÿ� ����
				shotBullet.Shootting(this, targetPiece);
				break;
		}
	}

	/// <summary>
	/// ��Ʈ �Լ�
	/// </summary>
	/// <param name="damage">��Ʈ ������</param>
	public void Hit(float damage, HitEff.HitType type)
	{
		if (_Name == PieceName.DemonGuard && DG_Counter < 5)
			DG_Counter++;
		else if (_Name == PieceName.DemonGuard && DG_Counter >= 5)
			Skill_Instance.DemonGuardSkill(this, true);

		_HitEffecter.Hit(type);

		// ���� ü�¿��� ������ ����
		if (Fun_nowArmor - damage >= 0)
		{
			Fun_curHP = -1;
		}
		else
			Fun_curHP = Fun_nowArmor - damage;	

		// ���� ���� 2 ����
		Fun_curMP = Fun_curMP + 2;

		// ���� ü���� 0���� ���� ��
		if (Fun_curHP <= 0)
		{
			_anim.SetTrigger("IsDead");
			_State = NowState.DEAD;
			// ��� �Լ� & ��� ����
			Dead();
		}
	}

	void SlashEff()
	{
		if (_CripName != CripName.None)
			return;

		if (_Name == PieceName.BeeKnghit ||
			_Name == PieceName.SkelWarrior ||
			_Name == PieceName.TraineeWizard)
			return;

		if (_Name != PieceName.AxeWarrior &&
			_Name != PieceName.ElvenKnight &&
			_Name != PieceName.BloodLord)
		{
			switch(_Name)
			{
				case PieceName.PootMan:
					StartCoroutine(slashRate(0.2f, 0));
					break;
				case PieceName.DemonGuard:
					StartCoroutine(slashRate(0.1f, 0));
					break;
			}
			
		}
	}

	/// <summary>
	/// ��ų �Լ�
	/// </summary>
	void Skill()
	{
		// ��ų ��� �ߵ�
		_anim.SetTrigger("IsSkill");

		// ���� ������ �ʱ�ȭ
		curDelay = 0;
		// ���� ���� �ʱ�ȭ
		Fun_curMP = 0;

		// ��ų ��� �� Battle ���� ����
		setIsSkill(true);

		switch (_Name)
		{
			case PieceName.PootMan:
				StartCoroutine(slashRate(0.1f, 1));
				Skill_Instance.PootmanSkill(this);
				// Ÿ�� ��Ʈ �ݶ��̴� On
				collider.gameObject.GetComponent<AttackScript>().targetSetting(targetPiece);
				collider.enabled = true;
				break;
			case PieceName.SkelWarrior:
				StartCoroutine(Skill_Instance.SkelWarriorSkill(this));
				break;
			case PieceName.BeeKnghit:
				Skill_Instance.BeeKnightSkill(this);
				// Ÿ�� ��Ʈ �ݶ��̴� On
				collider.gameObject.GetComponent<AttackScript>().targetSetting(targetPiece);
				collider.enabled = true;
				break;
			case PieceName.TraineeWizard:
				Skill_Instance.TraineeWizardSkill(this);
				break;
		}
	}

	IEnumerator slashRate(float delay, int index)
	{
		yield return new WaitForSeconds(delay);

		_SlashEffecter[index].GetComponent<ParticleSystem>().Play();
	}

	void SkillEnd()
	{
		curDelay = 1.0f;

		_anim.SetTrigger("UseSkill");

		setIsSkill(false);

		// ���¸� ��Ʋ��
		_State = NowState.BATTLE;
	}

	/// <summary>
	/// ��� ó�� �Լ�
	/// </summary>
	public void Dead()
	{
		// ����� �ǽ� �����ǿ��� ����
		DeadPiece(nowBoard.getBoardNum(), Type);

		Invoke("hideObj", 3f);
	}

	void hideObj()
	{
		gameObject.SetActive(false);
	}

	// �ǽ� ������ Init
	public void PieceInit(PieceName name, CripName crip, PieceType type)
	{
		switch (name)
		{
			case PieceName.None:
				break;
			case PieceName.PootMan:
				PieceInit(60, 25, 10, 3, 1.5f, 1.5f, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.SkelWarrior:
				PieceInit(50, 25, 12, 2, 1.4f, 1.5f, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Undead;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.BeeKnghit:
				PieceInit(60, 30, 5, 8, 1.6f, 1.7f, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.DemonGuard:
				PieceInit(55, 0, 6, 5, 1.5f, 1.7f, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.TraineeWizard:
				PieceInit(40, 40, 12, 1, 1.6f, 1.5f, 4, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Mage;
				break;
			case PieceName.Archer:
				PieceInit(45, 35, 15, 3, 1.7f, 4.0f, 4, type, PieceTier.TWO, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Ranger;
				break;
			case PieceName.AxeWarrior:
				PieceInit(55, 30, 12, 3, 1.5f, 1.5f, 1, type, PieceTier.TWO, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.UndeadMage:
				PieceInit(40, 35, 15, 1, 1.5f, 1.5f, 4, type, PieceTier.TWO, PieceStar.oneStar);
				Attribute = PieceSynergy.Undead;
				Class = PieceSynergy.Mage;
				break;
			case PieceName.PhantomKnight:
				PieceInit(55, 35, 13, 2, 1.7f, 0f, 1, type, PieceTier.TWO, PieceStar.oneStar);
				Attribute = PieceSynergy.Undead;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.EvilEye:
				PieceInit(50, 25, 10, 3, 1.6f, 4.0f, 1, type, PieceTier.TWO, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Mage;
				break;
			case PieceName.BesatMan:
				PieceInit(50, 20, 9, 3, 1.4f, 1.5f, 1, type, PieceTier.THREE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Ranger;
				break;
			case PieceName.Berserker:
				PieceInit(45, 0, 15, 1, 1.5f, 1.5f, 1, type, PieceTier.THREE, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.Imp:
				PieceInit(45, 25, 12, 3, 1.5f, 1.5f, 1, type, PieceTier.THREE, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Ranger;
				break;
			case PieceName.OrcWarrior:
				PieceInit(60, 30, 10, 1, 1.6f, 1.5f, 1, type, PieceTier.THREE, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.MetalKnight:
				PieceInit(55, 35, 8, 5, 1.6f, 1.5f, 1, type, PieceTier.THREE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.DeathKnight:
				PieceInit(55, 25, 12, 3, 1.5f, 1.5f, 1, type, PieceTier.FOUR, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.Druid:
				PieceInit(40, 30, 13, 3, 1.5f, 1.5f, 4, type, PieceTier.FOUR, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Mage;
				break;
			case PieceName.CorpseCollecter:
				PieceInit(50, 30, 14, 1, 1.4f, 1.5f, 1, type, PieceTier.FOUR, PieceStar.oneStar);
				Attribute = PieceSynergy.Undead;
				Class = PieceSynergy.Ranger;
				break;
			case PieceName.ElvenKnight:
				PieceInit(55, 20, 10, 4, 1.6f, 1.5f, 1, type, PieceTier.FOUR, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Knight;
				break;
			case PieceName.BloodLord:
				PieceInit(55, 35, 12, 3, 1.4f, 2.5f, 1, type, PieceTier.FIVE, PieceStar.oneStar);
				Attribute = PieceSynergy.Demon;
				Class = PieceSynergy.Ranger;
				break;
			case PieceName.Golem:
				PieceInit(60, 35, 15, 5, 2.0f, 1.5f, 1, type, PieceTier.FIVE, PieceStar.oneStar);
				Attribute = PieceSynergy.Undead;
				Class = PieceSynergy.Warrior;
				break;
			case PieceName.MagicShooter:
				PieceInit(50, 30, 16, 1, 1.8f, 1.5f, 4, type, PieceTier.FIVE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Ranger;
				break;
		}

		switch (crip)
		{
			case CripName.None:
				break;
			case CripName.Slime:
				PieceInit(40, 0, 8, 0, 1.9f, 0, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Nature;
				Class = PieceSynergy.Warrior;
				break;
			case CripName.ArmorSlime:
				PieceInit(45, 0, 8, 3, 2.0f, 0, 1, type, PieceTier.ONE, PieceStar.oneStar);
				Attribute = PieceSynergy.Steel;
				Class = PieceSynergy.Warrior;
				break;
		}
	}

	// �ǽ� ������ȯ
	public void PieceRotate(float z)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0, z, 0));
	}

	void autoRotate()
	{
		// ���� ��ȯ
		// ��ǥ�� ���� ���� ���� ��
		if (targetPiece.nowBoard.getBoardNum() / 10 > nowBoard.getBoardNum() / 10)
		{
			// �⹰�� ���� ������ ��������(����)
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		// ��ǥ�� ���� �Ʒ��� ���� ��
		else if (targetPiece.nowBoard.getBoardNum() / 10 < nowBoard.getBoardNum() / 10)
		{
			// �⹰�� ���� ������ �ĸ�����(�ڷ�)
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		// ��ǥ�� ���� �࿡ ���� ��
		else if (targetPiece.nowBoard.getBoardNum() / 10 == nowBoard.getBoardNum() / 10)
		{
			// ��ǥ�� ���� ������ ���� ��
			if (targetPiece.nowBoard.getBoardNum() % 10 > nowBoard.getBoardNum() % 10)
			{
				// �⹰�� ���� ������ ��������
				transform.rotation = Quaternion.Euler(0, 90, 0);
			}
			// ��ǥ�� ���� ������ ���� ��
			else
			{
				// �⹰�� ���� ������ ��������
				transform.rotation = Quaternion.Euler(0, -90, 0);
			}
		}
	}

	void longRotate()
	{
		Vector3 Vec = targetPiece.transform.position - transform.position;
		Vec.y = 0;

		float z = Mathf.Atan2(Vec.z, Vec.x) * Mathf.Rad2Deg;

		if (0 < z)
			PieceRotate(90 - z);
		else if (z <= 0)
			PieceRotate(90 + z);
	}
}
