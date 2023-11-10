using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceData : MonoBehaviour
{
    ChessPiece nowPiece;

	[SerializeField]
	Camera faceCam;

    [SerializeField]
    Image _Attribute;
    [SerializeField]
    Image _Class;

    [SerializeField]
    Sprite[] _AttributeSprite;
	[SerializeField]
	Sprite[] _ClassSprite;

    [SerializeField]
    Text[] Texts;

	[SerializeField]
	Image _skillImage;
	[SerializeField]
	Sprite[] _SkillSprite;

	public void PickPiece(ChessPiece piece)
    {
        nowPiece = piece;

		UpUI();

		SetData();
	}

	void UpUI()
	{
		gameObject.GetComponent<RectTransform>().anchoredPosition =
			new Vector2(700,
				gameObject.GetComponent<RectTransform>().anchoredPosition.y);
	}

	public void hideUI()
	{
		gameObject.GetComponent<RectTransform>().anchoredPosition = 
			new Vector2(1200,
				gameObject.GetComponent<RectTransform>().anchoredPosition.y);
	}

    void SetData()
    {
		Vector3 camVec = new Vector3(5000, 0, 0);

		switch(nowPiece.Star)
		{
			case Pieces<ChessPiece>.PieceStar.oneStar:
				Texts[0].text = "★";
				break;
			case Pieces<ChessPiece>.PieceStar.twoStar:
				Texts[0].text = "★★";
				break;
			case Pieces<ChessPiece>.PieceStar.threeStar:
				Texts[0].text = "★★★";
				break;
		}

		switch(nowPiece.Tier)
		{
			case Pieces<ChessPiece>.PieceTier.ONE:
				Texts[0].color = Color.white;
				Texts[1].color = Color.white;
				break;
			case Pieces<ChessPiece>.PieceTier.TWO:
				Texts[0].color = new Color(0.24f, 0.71f, 0.16f);
				Texts[1].color = new Color(0.24f, 0.71f, 0.16f);
				break;
			case Pieces<ChessPiece>.PieceTier.THREE:
				Texts[0].color = new Color(0, 0.39f, 1);
				Texts[1].color = new Color(0, 0.39f, 1);
				break;
			case Pieces<ChessPiece>.PieceTier.FOUR:
				Texts[0].color = new Color(0.4f, 0.13f, 0.75f);
				Texts[1].color = new Color(0.4f, 0.13f, 0.75f);
				break;
			case Pieces<ChessPiece>.PieceTier.FIVE:
				Texts[0].color = new Color(0.75f, 0.36f, 0.13f);
				Texts[1].color = new Color(0.75f, 0.36f, 0.13f);
				break;
		}

		switch (nowPiece._Name)
        {
            case ChessPiece.PieceName.PootMan:
                _Attribute.sprite = _AttributeSprite[0];
                _Class.sprite = _ClassSprite[0];
				Texts[1].text = "보병";
				break;
			case ChessPiece.PieceName.SkelWarrior:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "해골 전사";
				camVec.x += 6;
				break;
			case ChessPiece.PieceName.BeeKnghit:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "벌꿀 기사";
				camVec.x += 12;
				break;
			case ChessPiece.PieceName.DemonGuard:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "악마 경비병";
				camVec.x += 18;
				break;
			case ChessPiece.PieceName.TraineeWizard:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "견습 마법사";
				camVec.x += 24;
				break;
			case ChessPiece.PieceName.Archer:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "숲지기";
				camVec.x += 30;
				break;
			case ChessPiece.PieceName.AxeWarrior:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "도끼 전사";
				camVec.x += 36;
				break;
			case ChessPiece.PieceName.UndeadMage:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "해골 마법사";
				camVec.x += 42;
				break;
			case ChessPiece.PieceName.PhantomKnight:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "망령 기사";
				camVec.x += 54;
				break;
			case ChessPiece.PieceName.EvilEye:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "악마의 눈";
				camVec.x += 48;
				break;
			case ChessPiece.PieceName.Berserker:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "광전사";
				camVec.x += 60;
				break;
			case ChessPiece.PieceName.Imp:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "소악마";
				camVec.x += 66;
				break;
			case ChessPiece.PieceName.OrcWarrior:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "야만족 전사";
				camVec.x += 72;
				break;
			case ChessPiece.PieceName.MetalKnight:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "철기사";
				camVec.x += 78;
				break;
			case ChessPiece.PieceName.BesatMan:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "야수 사냥꾼";
				camVec.x += 84;
				break;
			case ChessPiece.PieceName.DeathKnight:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "죽음의 기사";
				camVec.x += 90;
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "시체 수집가";
				camVec.x += 102;
				break;
			case ChessPiece.PieceName.Druid:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "숲의 현자";
				camVec.x += 108;
				break;
			case ChessPiece.PieceName.ElvenKnight:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "요정 기사";
				camVec.x += 96;
				break;
			case ChessPiece.PieceName.MagicShooter:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "마탄의 사수";
				camVec.x += 114;
				break;
			case ChessPiece.PieceName.Golem:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "거신병";
				camVec.x += 120;
				break;
			case ChessPiece.PieceName.BloodLord:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "흡혈 군주";
				camVec.x += 126;
				break;
		}

		switch(nowPiece.Attribute)
		{
			case Pieces<ChessPiece>.PieceSynergy.Steel:
				_Attribute.color = Color.gray;
				break;
			case Pieces<ChessPiece>.PieceSynergy.Nature:
				_Attribute.color = Color.green;
				break;
			case Pieces<ChessPiece>.PieceSynergy.Undead:
				_Attribute.color = new Color(0.1f, 0.1f, 0.39f, 1);
				break;
			case Pieces<ChessPiece>.PieceSynergy.Demon:
				_Attribute.color = new Color(0.74f, 0.1f, 1, 1);
				break;
		}

		switch (nowPiece.Class)
		{
			case Pieces<ChessPiece>.PieceSynergy.Warrior:
				_Class.color = new Color(1, 0.66f, 0.39f, 1);
				break;
			case Pieces<ChessPiece>.PieceSynergy.Ranger:
				_Class.color = new Color(0.1f, 0.39f, 0.1f, 1);
				break;
			case Pieces<ChessPiece>.PieceSynergy.Knight:
				_Class.color = new Color(1, 0.86f, 0, 1);
				break;
			case Pieces<ChessPiece>.PieceSynergy.Mage:
				_Class.color = Color.blue;
				break;
		}

		faceCam.transform.position = camVec;

		Texts[2].text = "HP : " + nowPiece.Fun_curHP + "/" + nowPiece.Fun_maxHP +
						"\tMP : " + nowPiece.Fun_curMP + "/" + nowPiece.Fun_maxMP +"\r\n" +
			"ATK : " + nowPiece.Fun_nowAttack + "\t\tDEF : " + nowPiece.Fun_nowArmor + "\r\n" +
			"ATK Speed : " + nowPiece.Fun_nowDelay + "\tATK Range : " + nowPiece.Fun_attackRange;

		skillText();
	}

	void skillText()
	{
		switch(nowPiece._Name)
		{
			case ChessPiece.PieceName.PootMan:
				Texts[3].text = "스킬 : 강타\n" +
					"한 명의 적에게\n" +
					"강력한 일격(" + nowPiece.Fun_nowAttack * 1.35f + ")을\n" +
					"가합니다.\n";
				_skillImage.sprite = _SkillSprite[0];
				break;
			case ChessPiece.PieceName.SkelWarrior:
				Texts[3].text = "스킬 : 방패막기\n" +
					"3초간 자신의 방어력을\n강화합니다\n" +
					"유지 시간동안 방어력이(" + nowPiece.Fun_nowArmor * (0.2f * ((int)nowPiece.Star) + 1) + ")\n" +
					"상승합니다.\n";
				_skillImage.sprite = _SkillSprite[1];
				break;
			case ChessPiece.PieceName.BeeKnghit:
				Texts[3].text = "스킬 : 필살 찌르기\n" +
					"한 명의 적에게\n" +
					"강력한 일격(" + nowPiece.Fun_nowAttack * 1.45f + ")을\n" +
					"가합니다.\n";
				_skillImage.sprite = _SkillSprite[2];
				break;
			case ChessPiece.PieceName.DemonGuard:
				Texts[3].text = "스킬 : 악마의 반격(패시브)\n" +
					"피해를 누적시킵니다\n" +
					"5번의 피해를 입으면\n다음 공격이\n" +
					"강화(" + nowPiece.Fun_nowAttack * 1.15f + ")됩니다.";
				_skillImage.sprite = _SkillSprite[3];
				break;
			case ChessPiece.PieceName.TraineeWizard:
				Texts[3].text = "스킬 : 물방울 탄\n" +
					"마법의 물방울 탄을 사출합니다\n" +
					"가장 먼저 물방울을 맞은 적에게\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.6f + ")를 입힙니다.";
				_skillImage.sprite = _SkillSprite[4];
				break;
			case ChessPiece.PieceName.Archer:
				Texts[3].text = "스킬 : 다중 사격\n" +
					"여러 갈래의 화살을 발사합니다.\n" +
					"각 화살을 맞은 적에게\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.2f + ")를 입힙니다.";
				_skillImage.sprite = _SkillSprite[5];
				break;
			case ChessPiece.PieceName.AxeWarrior:
				Texts[3].text = "스킬 : 천둥 찍기\n" +
					"한 명의 적에게\n" +
					"방어를 무시한 일격(" + nowPiece.Fun_nowAttack * 1.2f + ")을\n" +
					"가합니다.\n";
				_skillImage.sprite = _SkillSprite[6];
				break;
			case ChessPiece.PieceName.UndeadMage:
				Texts[3].text = "스킬 : 서리 고드름\n" +
					"자신과 가장 먼 적 한 명에게\n" +
					"얼음 마법(" + nowPiece.Fun_nowAttack * 1.8f + ")을\n" +
					"떨어트립니다.\n";
				_skillImage.sprite = _SkillSprite[7];
				break;
			case ChessPiece.PieceName.PhantomKnight:
				Texts[3].text = "스킬 : 망령 습격\n" +
					"4칸 이내의 가장 체력이 적은\n" +
					"적에게 돌격해\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.25f + ")를 입힙니다";
				_skillImage.sprite = _SkillSprite[8];
				break;
			case ChessPiece.PieceName.EvilEye:
				Texts[3].text = "스킬 : 촉수 일격\n" +
					"한 명의 적에게\n" +
					"방어를 무시한 공격을" + " 가합니다.\n";
				_skillImage.sprite = _SkillSprite[9];
				break;
			case ChessPiece.PieceName.Berserker:
				Texts[3].text = "스킬 : 격노의 불길(패시브)\n" +
					"매 공격에 불길이 담깁니다.\n" +
					"불길은 마지막 공격을 기준으로\n" + 
					"3초 동안 유지되며\n" +
					"초당 지속 피해(" + nowPiece.Fun_nowAttack * 0.1f + ")를\n입힙니다";
				_skillImage.sprite = _SkillSprite[10];
				break;
			case ChessPiece.PieceName.Imp:
				Texts[3].text = "스킬 : 불덩이\n" +
					"지옥의 불덩이를 발사합니다\n" +
					"4칸 이내의 가장 체력이 적은\n" +
					"적에게 발사되며\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.3f + ")를 입힙니다.";
				_skillImage.sprite = _SkillSprite[11];
				break;
			case ChessPiece.PieceName.OrcWarrior:
				Texts[3].text = "스킬 : 야만의 함성\n" +
					"위협적인 함성을 내지릅니다.\n" +
					"야만족 전사와 전투 중인 적이\n" +
					"위압되며 3초간\n" +
					"공격력이(" + nowPiece.Fun_nowAttack * 0.2f + ")저하됩니다.";
				_skillImage.sprite = _SkillSprite[12];
				break;
			case ChessPiece.PieceName.MetalKnight:
				Texts[3].text = "스킬 : 번개 가르기\n" +
					"아래에서 위로 적을 올려벱니다.\n" +
					"한 명의 적에게\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.3f + ")를 입힙니다.";
				_skillImage.sprite = _SkillSprite[13];
				break;
			case ChessPiece.PieceName.BesatMan:
				Texts[3].text = "스킬 : 사냥 표식\n" +
					"사냥감에게 표식을 지정합니다.\n" +
					"표식은 공격을 3번\n맞을 때 까지 유지되며\n" +
					"피격 시 추가 피해(" + nowPiece.Fun_nowAttack * 0.2f + ")를 가합니다.";
				_skillImage.sprite = _SkillSprite[14];
				break;
			case ChessPiece.PieceName.DeathKnight:
				Texts[3].text = "스킬 : 단두대 일격\n" +
					"한 명의 적에게\n" +
					"치명적인 일격(" + nowPiece.Fun_nowAttack * 1.4f + ")을 " +
					"가합니다.\n" +
					"상대의 체력이 20%이하일 때\n" + 
					"일정 확률로 적을 즉사시킵니다.";
				_skillImage.sprite = _SkillSprite[15];
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				Texts[3].text = "스킬 : 부패 바람\n" +
					"자신에게 가장 먼 적을 향해\n" +
					"부패의 소용돌이를 날립니다.\n" +
					"소용돌이는 적을 향해 나아가며\n" +
					"범위 내의 적에게\n피해(" + nowPiece.Fun_nowAttack * 1.3f + ")를 " +
					"입힙니다.";
				_skillImage.sprite = _SkillSprite[16];
				break;
			case ChessPiece.PieceName.Druid:
				Texts[3].text = "스킬 : 신록의 치유\n" +
					"광범히 회복 영역을 설치합니다\n" +
					"자기 자신을 중심으로 설치되며\n" +
					"범위 내의 아군 현재 체력을\n" +
					"초당(" + (int)nowPiece.Tier * 2f + ")를 회복시킵니다";
				_skillImage.sprite = _SkillSprite[17];
				break;
			case ChessPiece.PieceName.ElvenKnight:
				Texts[3].text = "스킬 : 신속한 검격\n" +
					"연속으로 두 번\n공격을 시행합니다\n" +
					"한 명의 적에게\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.6f + ")를 입힙니다";
				_skillImage.sprite = _SkillSprite[18];
				break;
			case ChessPiece.PieceName.MagicShooter:
				Texts[3].text = "스킬 : 폭발의 비\n" +
					"하늘로 마법 화살을 발사합니다.\n" +
					"일정 시간 이후 적을 향해\n" +
					"화살비가 내리며 " +
					"범위 내의 적들은\n" +
					"초당 피해(" + nowPiece.Fun_nowAttack * 0.4f + ")를\n입습니다";
				_skillImage.sprite = _SkillSprite[19];
				break;
			case ChessPiece.PieceName.Golem:
				Texts[3].text = "스킬 : 거인의 일격\n" +
					"거신병이 온 힘을 다해\n일격을 가합니다.\n" +
					"범위 내의 모든 적에게\n" +
					"피해(" + nowPiece.Fun_nowAttack * 1.2f + ")를 입힙니다";
				_skillImage.sprite = _SkillSprite[20];
				break;
			case ChessPiece.PieceName.BloodLord:
				Texts[3].text = "스킬 : 날카로운 혈류\n" +
					"잠시동안 피의 군주가\n힘을 축적합니다.\n" +
					"일정시간 이후 군주가\n모은 힘을 해방하며\n" +
					"자신 주변의 적들에게\n" +
					"초당 피해(" + nowPiece.Fun_nowAttack * 0.2f + ")를 입힙니다";
				_skillImage.sprite = _SkillSprite[21];
				break;
		}
	}
}
