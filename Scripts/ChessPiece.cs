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

	// 타깃 에너미
	public ChessPiece targetPiece;

	// 타깃 보드 넘버
	int target;

	// 스킬 사용 여부
	[SerializeField]
	bool noSkill;
	// 원거리 유닛
	[SerializeField]
	bool longRange;

	// 현재 공격 딜레이
	float curDelay = 0;
	// 현재 공격 여부
	[SerializeField]
	bool isAttack;
	// 공격 콜라이더
	[SerializeField]
	Collider collider;
	// 원거리 공격
	[SerializeField]
	Shoot shotBullet;

	// 슬래쉬 오브젝트
	[SerializeField]
	GameObject[] _SlashEffecter;

	// 히트 오브젝트
	[SerializeField]
	HitEff _HitEffecter;

	// 스킬 오브젝트
	public GameObject[] skillBullet;

	int DG_Counter = 0;

	public void BattleStart()
	{
		_State = NowState.IDLE;
	}

	public void BattleEnd()
	{
		_anim.SetTrigger("EndBattle");
		// 상태 종료
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
		// 특정 상황에서 전투 논리 X
		if (_State == NowState.DEAD || _State == NowState.END || _State == NowState.START)
			return;

		// 타겟이 존재할 때
		if (targetPiece != null)
		{
			// 타겟이 사망했을 시 에너미 재탐색 시작 (에너미 전투)
			if (targetPiece._State == NowState.DEAD)
			{
				targetPiece = null;
				_State = NowState.IDLE;
			}
		}

		// 공격 애니메이션 종료 판정 여부 확인
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
				// 가까운 적 탐색
				target = BM_instance.SearchPieces(nowBoard.getBoardNum(), Type);
				// 타겟 고정
				targetPiece = BM_instance.EnemyTargetting(target, Type);

				if (!isAttack && !getIsSkill())
					autoRotate();

				// 이동 상태
				_State = NowState.MOVE; ;

				break;
			case NowState.MOVE:
				// 이동 중이 아닐때, 공격 중이 아닐 때
				if (!isMove && !isAttack)
				{
					if (!getIsSkill())
						autoRotate();

					// 이동 시작
					StartCoroutine(Move(targetPiece.nowBoard));

					curDelay = 0.5f;
				}
				break;
			case NowState.BATTLE:
				// 스킬 발동 중이 아니고 공격 딜레이를 만족할 때
				if (!getIsSkill() && curDelay > Fun_nowDelay)
				{
					_anim.SetTrigger("IsAttack");

					// 현재 마나 회복
					Fun_curMP = Fun_curMP + 3;

					// 딜레이 초기화
					curDelay = 0;

					if (!longRange)
					{
						autoRotate();

						if (_Name == PieceName.DemonGuard && DG_Counter >= 5)
							// 스킬 사용 중 Battle 상태 차단
							setIsSkill(true);

						// 타겟 히트 콜라이더 On
						collider.gameObject.GetComponent<AttackScript>().targetSetting(targetPiece);
						collider.enabled = true;

						SlashEff();
					}
					else
					{
						// 원거리 공격 투사체 발사 딜레이
						StartCoroutine(shootBullet());
					}

					isAttack = true;
				}

				if (!isAttack && Fun_maxMP < Fun_curMP && !noSkill)
					_State = NowState.SKILL;

				// 딜레이 타임
				if (!isAttack)
					curDelay += Time.deltaTime;

				break;
			case NowState.SKILL:
				if (!getIsSkill())
				{
					autoRotate();

					// 스킬 발동 함수
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
				// 원거리 공격 기물 방향 조정
				longRotate();
				// 원거리 공격
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.Archer:
				yield return new WaitForSeconds(0.8f);
				// 원거리 공격 기물 방향 조정
				longRotate();
				// 원거리 공격
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.UndeadMage:
				yield return new WaitForSeconds(0.4f);
				// 원거리 공격 기물 방향 조정
				longRotate();
				// 원거리 공격
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.Druid:
				yield return new WaitForSeconds(0.4f);
				// 원거리 공격 기물 방향 조정
				longRotate();
				// 원거리 공격
				shotBullet.Shootting(this, targetPiece);
				break;
			case PieceName.MagicShooter:
				yield return new WaitForSeconds(0.8f);
				// 원거리 공격 기물 방향 조정
				longRotate();
				// 원거리 공격
				shotBullet.Shootting(this, targetPiece);
				break;
		}
	}

	/// <summary>
	/// 히트 함수
	/// </summary>
	/// <param name="damage">히트 데미지</param>
	public void Hit(float damage, HitEff.HitType type)
	{
		if (_Name == PieceName.DemonGuard && DG_Counter < 5)
			DG_Counter++;
		else if (_Name == PieceName.DemonGuard && DG_Counter >= 5)
			Skill_Instance.DemonGuardSkill(this, true);

		_HitEffecter.Hit(type);

		// 현재 체력에서 데미지 감소
		if (Fun_nowArmor - damage >= 0)
		{
			Fun_curHP = -1;
		}
		else
			Fun_curHP = Fun_nowArmor - damage;	

		// 현재 마나 2 증가
		Fun_curMP = Fun_curMP + 2;

		// 현재 체력이 0보다 작을 때
		if (Fun_curHP <= 0)
		{
			_anim.SetTrigger("IsDead");
			_State = NowState.DEAD;
			// 사망 함수 & 사망 상태
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
	/// 스킬 함수
	/// </summary>
	void Skill()
	{
		// 스킬 모션 발동
		_anim.SetTrigger("IsSkill");

		// 어택 딜레이 초기화
		curDelay = 0;
		// 현재 마나 초기화
		Fun_curMP = 0;

		// 스킬 사용 중 Battle 상태 차단
		setIsSkill(true);

		switch (_Name)
		{
			case PieceName.PootMan:
				StartCoroutine(slashRate(0.1f, 1));
				Skill_Instance.PootmanSkill(this);
				// 타겟 히트 콜라이더 On
				collider.gameObject.GetComponent<AttackScript>().targetSetting(targetPiece);
				collider.enabled = true;
				break;
			case PieceName.SkelWarrior:
				StartCoroutine(Skill_Instance.SkelWarriorSkill(this));
				break;
			case PieceName.BeeKnghit:
				Skill_Instance.BeeKnightSkill(this);
				// 타겟 히트 콜라이더 On
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

		// 상태를 배틀로
		_State = NowState.BATTLE;
	}

	/// <summary>
	/// 사망 처리 함수
	/// </summary>
	public void Dead()
	{
		// 사망한 피스 보드판에서 제거
		DeadPiece(nowBoard.getBoardNum(), Type);

		Invoke("hideObj", 3f);
	}

	void hideObj()
	{
		gameObject.SetActive(false);
	}

	// 피스 데이터 Init
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

	// 피스 방향전환
	public void PieceRotate(float z)
	{
		transform.rotation = Quaternion.Euler(new Vector3(0, z, 0));
	}

	void autoRotate()
	{
		// 방향 전환
		// 목표의 행이 위에 있을 때
		if (targetPiece.nowBoard.getBoardNum() / 10 > nowBoard.getBoardNum() / 10)
		{
			// 기물의 보는 방향을 전면으로(위로)
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		// 목표의 행이 아래에 있을 때
		else if (targetPiece.nowBoard.getBoardNum() / 10 < nowBoard.getBoardNum() / 10)
		{
			// 기물의 보는 방향을 후면으로(뒤로)
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
		// 목표와 같은 행에 있을 때
		else if (targetPiece.nowBoard.getBoardNum() / 10 == nowBoard.getBoardNum() / 10)
		{
			// 목표의 열이 우측에 있을 때
			if (targetPiece.nowBoard.getBoardNum() % 10 > nowBoard.getBoardNum() % 10)
			{
				// 기물의 보는 방향을 우측으로
				transform.rotation = Quaternion.Euler(0, 90, 0);
			}
			// 목표의 열이 좌측에 있을 때
			else
			{
				// 기물의 보는 방향을 좌측으로
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
