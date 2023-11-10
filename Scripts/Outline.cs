using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outline : MonoBehaviour
{
	// �ƿ������� �����ų ��Ų
    [SerializeField]
    SkinnedMeshRenderer unitMat;
	[SerializeField]
	MeshRenderer weaponMat;

	// �ƿ����� �� �迭
    [SerializeField]
    Material[] outline;

	/// <summary>
	///  �Ͼ�� �ƿ����� (1��)
	/// </summary>
	public void outlineWhite()
	{
		// ���� ������
		if (unitMat == null)
		{
			// �ƿ����� ���׸����� �Ͼ� �ƿ��������� �����Ѵ�
			weaponMat.materials[1].color = outline[0].color;
		}
		// �� ������
		else
		{
			// �ƿ����� ���׸����� �Ͼ� �ƿ��������� �����Ѵ�
			unitMat.materials[1].color = outline[0].color;
		}
	}

	/// <summary>
		///  �Ķ��� �ƿ����� (2��)
		/// </summary>
	public void outlineBlue()
	{
		// ���� ������
		if (unitMat == null)
		{
			// �ƿ����� ���׸����� �Ķ� �ƿ��������� �����Ѵ�
			weaponMat.materials[1].color = outline[1].color;
		}
		// �� ������
		else
		{
			// �ƿ����� ���׸����� �Ķ� �ƿ��������� �����Ѵ�
			unitMat.materials[1].color = outline[1].color;
		}
	}

	/// <summary>
	///  �ݻ� �ƿ����� (3��)
	/// </summary>
	public void outlineOrange()
	{
		// ���� ������
		if (unitMat == null)
		{
			// �ƿ����� ���׸����� �Ͼ� �ƿ��������� �����Ѵ�
			weaponMat.materials[1].color = outline[2].color;
		}
		// �� ������
		else
		{
			// �ƿ����� ���׸����� �Ͼ� �ƿ��������� �����Ѵ�
			unitMat.materials[1].color = outline[2].color;
		}
	}
}
