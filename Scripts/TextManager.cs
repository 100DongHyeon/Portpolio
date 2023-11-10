using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public static TextManager TM_instance = null;

	private void Awake()
	{
		if (TM_instance == null)	
			TM_instance = this;
	}

	[SerializeField]
	Text _stateText;

	/// <summary>
	/// ���� ���� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="state">���� ���� ����</param>
	public void StateText(GameManager.GameState state)
	{
		switch(state)
		{
			case GameManager.GameState.Prepare:
				_stateText.text = "�غ� �ܰ�";
				break;
			case GameManager.GameState.Start:
				_stateText.text = "���� �غ�";
				break;
			case GameManager.GameState.Battle:
				_stateText.text = "����";
				break;
			case GameManager.GameState.End:
				_stateText.text = "���� ����";
				break;
		}
	}

	[SerializeField]
	Text _roundText;

	/// <summary>
	/// ���� ���� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="state">���� ���� ����</param>
	public void RoundText(int round)
	{
		_roundText.text = "Round" + round.ToString();
	}

	[SerializeField]
	Text _timeText;

	/// <summary>
	/// ���� ���� �ð� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="time">���� ���� �ð�</param>
	public void TimeText(int time)
	{
		_timeText.text = time.ToString();
	}

	[SerializeField]
	Text _lvText;

	/// <summary>
	/// �÷��̾� ���� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="level">���� ����</param>
	public void LevelText(int level)
	{
		_lvText.text = "Lv." + level.ToString();
	}

	[SerializeField]
	Text _expText;

	/// <summary>
	/// �÷��̾� ����ġ ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="level">���� ����</param>
	/// <param name="exp">���� ����ġ</param>
	public void ExpText(int level, int exp)
	{
		switch(level)
		{
			case 1:
				_expText.text =  exp.ToString() + "/1";
				break;
			case 2:
				_expText.text = exp.ToString() + "/1";
				break;
			case 3:
				_expText.text = exp.ToString() + "/2";
				break;
			case 4:
				_expText.text = exp.ToString() + "/4";
				break;
			case 5:
				_expText.text = exp.ToString() + "/8";
				break;
			case 6:
				_expText.text = exp.ToString() + "/16";
				break;
			case 7:
				_expText.text = exp.ToString() + "/24";
				break;
			case 8:
				_expText.text = exp.ToString() + "/36";
				break;
			case 9:
				_expText.text = exp.ToString() + "/48";
				break;
			case 10:
				_expText.text = "Max";
				break;
		}
		
	}

	[SerializeField]
	Text _goldText;

	/// <summary>
	/// �÷��̾� ��� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="gold">���� ü��</param>
	public void GoldText(int gold)
	{
		_goldText.text = gold.ToString();
	}

	[SerializeField]
	Text _hpText;

	/// <summary>
	/// �÷��̾� ü�� ǥ�� �ؽ�Ʈ
	/// </summary>
	/// <param name="hp">���� ü��</param>
	public void HpText(int hp)
	{
		_goldText.text = hp.ToString();
	}
}
