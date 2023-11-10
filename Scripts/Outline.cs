using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
	// 아웃라인을 적용시킬 스킨
    [SerializeField]
    SkinnedMeshRenderer unitMat;
	[SerializeField]
	MeshRenderer weaponMat;

	// 아웃라인 색 배열
    [SerializeField]
    Material[] outline;

	/// <summary>
	///  하얀색 아웃라인 (1성)
	/// </summary>
	public void outlineWhite()
	{
		// 무기 렌더러
		if (unitMat == null)
		{
			// 아웃라인 마테리얼을 하얀 아웃라인으로 변경한다
			weaponMat.materials[1].color = outline[0].color;
		}
		// 옷 렌더러
		else
		{
			// 아웃라인 마테리얼을 하얀 아웃라인으로 변경한다
			unitMat.materials[1].color = outline[0].color;
		}
	}

	/// <summary>
		///  파란색 아웃라인 (2성)
		/// </summary>
	public void outlineBlue()
	{
		// 무기 렌더러
		if (unitMat == null)
		{
			// 아웃라인 마테리얼을 파란 아웃라인으로 변경한다
			weaponMat.materials[1].color = outline[1].color;
		}
		// 옷 렌더러
		else
		{
			// 아웃라인 마테리얼을 파란 아웃라인으로 변경한다
			unitMat.materials[1].color = outline[1].color;
		}
	}

	/// <summary>
	///  금색 아웃라인 (3성)
	/// </summary>
	public void outlineOrange()
	{
		// 무기 렌더러
		if (unitMat == null)
		{
			// 아웃라인 마테리얼을 하얀 아웃라인으로 변경한다
			weaponMat.materials[1].color = outline[2].color;
		}
		// 옷 렌더러
		else
		{
			// 아웃라인 마테리얼을 하얀 아웃라인으로 변경한다
			unitMat.materials[1].color = outline[2].color;
		}
	}
}
