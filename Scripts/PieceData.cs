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
				Texts[0].text = "��";
				break;
			case Pieces<ChessPiece>.PieceStar.twoStar:
				Texts[0].text = "�ڡ�";
				break;
			case Pieces<ChessPiece>.PieceStar.threeStar:
				Texts[0].text = "�ڡڡ�";
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
				Texts[1].text = "����";
				break;
			case ChessPiece.PieceName.SkelWarrior:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "�ذ� ����";
				camVec.x += 6;
				break;
			case ChessPiece.PieceName.BeeKnghit:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "���� ���";
				camVec.x += 12;
				break;
			case ChessPiece.PieceName.DemonGuard:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "�Ǹ� ���";
				camVec.x += 18;
				break;
			case ChessPiece.PieceName.TraineeWizard:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "�߽� ������";
				camVec.x += 24;
				break;
			case ChessPiece.PieceName.Archer:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "������";
				camVec.x += 30;
				break;
			case ChessPiece.PieceName.AxeWarrior:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "���� ����";
				camVec.x += 36;
				break;
			case ChessPiece.PieceName.UndeadMage:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "�ذ� ������";
				camVec.x += 42;
				break;
			case ChessPiece.PieceName.PhantomKnight:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "���� ���";
				camVec.x += 54;
				break;
			case ChessPiece.PieceName.EvilEye:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "�Ǹ��� ��";
				camVec.x += 48;
				break;
			case ChessPiece.PieceName.Berserker:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "������";
				camVec.x += 60;
				break;
			case ChessPiece.PieceName.Imp:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "�ҾǸ�";
				camVec.x += 66;
				break;
			case ChessPiece.PieceName.OrcWarrior:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "�߸��� ����";
				camVec.x += 72;
				break;
			case ChessPiece.PieceName.MetalKnight:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "ö���";
				camVec.x += 78;
				break;
			case ChessPiece.PieceName.BesatMan:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "�߼� ��ɲ�";
				camVec.x += 84;
				break;
			case ChessPiece.PieceName.DeathKnight:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "������ ���";
				camVec.x += 90;
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "��ü ������";
				camVec.x += 102;
				break;
			case ChessPiece.PieceName.Druid:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[3];
				Texts[1].text = "���� ����";
				camVec.x += 108;
				break;
			case ChessPiece.PieceName.ElvenKnight:
				_Attribute.sprite = _AttributeSprite[1];
				_Class.sprite = _ClassSprite[2];
				Texts[1].text = "���� ���";
				camVec.x += 96;
				break;
			case ChessPiece.PieceName.MagicShooter:
				_Attribute.sprite = _AttributeSprite[0];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "��ź�� ���";
				camVec.x += 114;
				break;
			case ChessPiece.PieceName.Golem:
				_Attribute.sprite = _AttributeSprite[2];
				_Class.sprite = _ClassSprite[0];
				Texts[1].text = "�Žź�";
				camVec.x += 120;
				break;
			case ChessPiece.PieceName.BloodLord:
				_Attribute.sprite = _AttributeSprite[3];
				_Class.sprite = _ClassSprite[1];
				Texts[1].text = "���� ����";
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
				Texts[3].text = "��ų : ��Ÿ\n" +
					"�� ���� ������\n" +
					"������ �ϰ�(" + nowPiece.Fun_nowAttack * 1.35f + ")��\n" +
					"���մϴ�.\n";
				_skillImage.sprite = _SkillSprite[0];
				break;
			case ChessPiece.PieceName.SkelWarrior:
				Texts[3].text = "��ų : ���и���\n" +
					"3�ʰ� �ڽ��� ������\n��ȭ�մϴ�\n" +
					"���� �ð����� ������(" + nowPiece.Fun_nowArmor * (0.2f * ((int)nowPiece.Star) + 1) + ")\n" +
					"����մϴ�.\n";
				_skillImage.sprite = _SkillSprite[1];
				break;
			case ChessPiece.PieceName.BeeKnghit:
				Texts[3].text = "��ų : �ʻ� ���\n" +
					"�� ���� ������\n" +
					"������ �ϰ�(" + nowPiece.Fun_nowAttack * 1.45f + ")��\n" +
					"���մϴ�.\n";
				_skillImage.sprite = _SkillSprite[2];
				break;
			case ChessPiece.PieceName.DemonGuard:
				Texts[3].text = "��ų : �Ǹ��� �ݰ�(�нú�)\n" +
					"���ظ� ������ŵ�ϴ�\n" +
					"5���� ���ظ� ������\n���� ������\n" +
					"��ȭ(" + nowPiece.Fun_nowAttack * 1.15f + ")�˴ϴ�.";
				_skillImage.sprite = _SkillSprite[3];
				break;
			case ChessPiece.PieceName.TraineeWizard:
				Texts[3].text = "��ų : ����� ź\n" +
					"������ ����� ź�� �����մϴ�\n" +
					"���� ���� ������� ���� ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.6f + ")�� �����ϴ�.";
				_skillImage.sprite = _SkillSprite[4];
				break;
			case ChessPiece.PieceName.Archer:
				Texts[3].text = "��ų : ���� ���\n" +
					"���� ������ ȭ���� �߻��մϴ�.\n" +
					"�� ȭ���� ���� ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.2f + ")�� �����ϴ�.";
				_skillImage.sprite = _SkillSprite[5];
				break;
			case ChessPiece.PieceName.AxeWarrior:
				Texts[3].text = "��ų : õ�� ���\n" +
					"�� ���� ������\n" +
					"�� ������ �ϰ�(" + nowPiece.Fun_nowAttack * 1.2f + ")��\n" +
					"���մϴ�.\n";
				_skillImage.sprite = _SkillSprite[6];
				break;
			case ChessPiece.PieceName.UndeadMage:
				Texts[3].text = "��ų : ���� ��帧\n" +
					"�ڽŰ� ���� �� �� �� ����\n" +
					"���� ����(" + nowPiece.Fun_nowAttack * 1.8f + ")��\n" +
					"����Ʈ���ϴ�.\n";
				_skillImage.sprite = _SkillSprite[7];
				break;
			case ChessPiece.PieceName.PhantomKnight:
				Texts[3].text = "��ų : ���� ����\n" +
					"4ĭ �̳��� ���� ü���� ����\n" +
					"������ ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.25f + ")�� �����ϴ�";
				_skillImage.sprite = _SkillSprite[8];
				break;
			case ChessPiece.PieceName.EvilEye:
				Texts[3].text = "��ų : �˼� �ϰ�\n" +
					"�� ���� ������\n" +
					"�� ������ ������" + " ���մϴ�.\n";
				_skillImage.sprite = _SkillSprite[9];
				break;
			case ChessPiece.PieceName.Berserker:
				Texts[3].text = "��ų : �ݳ��� �ұ�(�нú�)\n" +
					"�� ���ݿ� �ұ��� ���ϴ�.\n" +
					"�ұ��� ������ ������ ��������\n" + 
					"3�� ���� �����Ǹ�\n" +
					"�ʴ� ���� ����(" + nowPiece.Fun_nowAttack * 0.1f + ")��\n�����ϴ�";
				_skillImage.sprite = _SkillSprite[10];
				break;
			case ChessPiece.PieceName.Imp:
				Texts[3].text = "��ų : �ҵ���\n" +
					"������ �ҵ��̸� �߻��մϴ�\n" +
					"4ĭ �̳��� ���� ü���� ����\n" +
					"������ �߻�Ǹ�\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.3f + ")�� �����ϴ�.";
				_skillImage.sprite = _SkillSprite[11];
				break;
			case ChessPiece.PieceName.OrcWarrior:
				Texts[3].text = "��ų : �߸��� �Լ�\n" +
					"�������� �Լ��� �������ϴ�.\n" +
					"�߸��� ����� ���� ���� ����\n" +
					"���еǸ� 3�ʰ�\n" +
					"���ݷ���(" + nowPiece.Fun_nowAttack * 0.2f + ")���ϵ˴ϴ�.";
				_skillImage.sprite = _SkillSprite[12];
				break;
			case ChessPiece.PieceName.MetalKnight:
				Texts[3].text = "��ų : ���� ������\n" +
					"�Ʒ����� ���� ���� �÷����ϴ�.\n" +
					"�� ���� ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.3f + ")�� �����ϴ�.";
				_skillImage.sprite = _SkillSprite[13];
				break;
			case ChessPiece.PieceName.BesatMan:
				Texts[3].text = "��ų : ��� ǥ��\n" +
					"��ɰ����� ǥ���� �����մϴ�.\n" +
					"ǥ���� ������ 3��\n���� �� ���� �����Ǹ�\n" +
					"�ǰ� �� �߰� ����(" + nowPiece.Fun_nowAttack * 0.2f + ")�� ���մϴ�.";
				_skillImage.sprite = _SkillSprite[14];
				break;
			case ChessPiece.PieceName.DeathKnight:
				Texts[3].text = "��ų : �ܵδ� �ϰ�\n" +
					"�� ���� ������\n" +
					"ġ������ �ϰ�(" + nowPiece.Fun_nowAttack * 1.4f + ")�� " +
					"���մϴ�.\n" +
					"����� ü���� 20%������ ��\n" + 
					"���� Ȯ���� ���� ����ŵ�ϴ�.";
				_skillImage.sprite = _SkillSprite[15];
				break;
			case ChessPiece.PieceName.CorpseCollecter:
				Texts[3].text = "��ų : ���� �ٶ�\n" +
					"�ڽſ��� ���� �� ���� ����\n" +
					"������ �ҿ뵹�̸� �����ϴ�.\n" +
					"�ҿ뵹�̴� ���� ���� ���ư���\n" +
					"���� ���� ������\n����(" + nowPiece.Fun_nowAttack * 1.3f + ")�� " +
					"�����ϴ�.";
				_skillImage.sprite = _SkillSprite[16];
				break;
			case ChessPiece.PieceName.Druid:
				Texts[3].text = "��ų : �ŷ��� ġ��\n" +
					"������ ȸ�� ������ ��ġ�մϴ�\n" +
					"�ڱ� �ڽ��� �߽����� ��ġ�Ǹ�\n" +
					"���� ���� �Ʊ� ���� ü����\n" +
					"�ʴ�(" + (int)nowPiece.Tier * 2f + ")�� ȸ����ŵ�ϴ�";
				_skillImage.sprite = _SkillSprite[17];
				break;
			case ChessPiece.PieceName.ElvenKnight:
				Texts[3].text = "��ų : �ż��� �˰�\n" +
					"�������� �� ��\n������ �����մϴ�\n" +
					"�� ���� ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.6f + ")�� �����ϴ�";
				_skillImage.sprite = _SkillSprite[18];
				break;
			case ChessPiece.PieceName.MagicShooter:
				Texts[3].text = "��ų : ������ ��\n" +
					"�ϴ÷� ���� ȭ���� �߻��մϴ�.\n" +
					"���� �ð� ���� ���� ����\n" +
					"ȭ��� ������ " +
					"���� ���� ������\n" +
					"�ʴ� ����(" + nowPiece.Fun_nowAttack * 0.4f + ")��\n�Խ��ϴ�";
				_skillImage.sprite = _SkillSprite[19];
				break;
			case ChessPiece.PieceName.Golem:
				Texts[3].text = "��ų : ������ �ϰ�\n" +
					"�Žź��� �� ���� ����\n�ϰ��� ���մϴ�.\n" +
					"���� ���� ��� ������\n" +
					"����(" + nowPiece.Fun_nowAttack * 1.2f + ")�� �����ϴ�";
				_skillImage.sprite = _SkillSprite[20];
				break;
			case ChessPiece.PieceName.BloodLord:
				Texts[3].text = "��ų : ��ī�ο� ����\n" +
					"��õ��� ���� ���ְ�\n���� �����մϴ�.\n" +
					"�����ð� ���� ���ְ�\n���� ���� �ع��ϸ�\n" +
					"�ڽ� �ֺ��� ���鿡��\n" +
					"�ʴ� ����(" + nowPiece.Fun_nowAttack * 0.2f + ")�� �����ϴ�";
				_skillImage.sprite = _SkillSprite[21];
				break;
		}
	}
}
