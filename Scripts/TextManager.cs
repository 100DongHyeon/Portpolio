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
	/// 게임 상태 표시 텍스트
	/// </summary>
	/// <param name="state">현재 게임 상태</param>
	public void StateText(GameManager.GameState state)
	{
		switch(state)
		{
			case GameManager.GameState.Prepare:
				_stateText.text = "준비 단계";
				break;
			case GameManager.GameState.Start:
				_stateText.text = "전투 준비";
				break;
			case GameManager.GameState.Battle:
				_stateText.text = "전투";
				break;
			case GameManager.GameState.End:
				_stateText.text = "전투 종료";
				break;
		}
	}

	[SerializeField]
	Text _roundText;

	/// <summary>
	/// 게임 상태 표시 텍스트
	/// </summary>
	/// <param name="state">현재 게임 상태</param>
	public void RoundText(int round)
	{
		_roundText.text = "Round" + round.ToString();
	}

	[SerializeField]
	Text _timeText;

	/// <summary>
	/// 게임 진행 시간 표시 텍스트
	/// </summary>
	/// <param name="time">현재 진행 시간</param>
	public void TimeText(int time)
	{
		_timeText.text = time.ToString();
	}

	[SerializeField]
	Text _lvText;

	/// <summary>
	/// 플레이어 레벨 표시 텍스트
	/// </summary>
	/// <param name="level">현재 레벨</param>
	public void LevelText(int level)
	{
		_lvText.text = "Lv." + level.ToString();
	}

	[SerializeField]
	Text _expText;

	/// <summary>
	/// 플레이어 경험치 표시 텍스트
	/// </summary>
	/// <param name="level">현재 레벨</param>
	/// <param name="exp">현재 경험치</param>
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
	/// 플레이어 골드 표시 텍스트
	/// </summary>
	/// <param name="gold">현재 체력</param>
	public void GoldText(int gold)
	{
		_goldText.text = gold.ToString();
	}

	[SerializeField]
	Text _hpText;

	/// <summary>
	/// 플레이어 체력 표시 텍스트
	/// </summary>
	/// <param name="hp">현재 체력</param>
	public void HpText(int hp)
	{
		_goldText.text = hp.ToString();
	}
}
