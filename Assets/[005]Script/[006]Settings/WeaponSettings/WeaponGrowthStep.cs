using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����{�[�i�X�̉��Z���@
/// </summary>
public enum BonusApplyType
{
    Flat,//�Œ�l���Z
    Rate//�����ω�
}

[Serializable]
public class WeaponGrowthBonus
{
    [Header("�����{�[�i�X�ΏۃX�e�[�^�X")]
    //�����{�[�i�X��K�p����X�e�[�^�X
    public WeaponStatusType Type;

    [Header("�����{�[�i�X�̉��Z���@")]
    //�����{�[�i�X�̉��Z���@
    public BonusApplyType ApplyType;

    [Header("�����{�[�i�X�̒l")]
    //�����{�[�i�X�̒l
    public float Value;
}

[Serializable]
public class WeaponGrowthStep
{
    [Header("�K������郌�x��")]
    public int Level;

    [Header("���̃��x���ŉ��Z�����{�[�i�X�ꗗ")]
    //���̃��x���ŉ��Z�����{�[�i�X�ꗗ
    public List<WeaponGrowthBonus> Bonuses;
}
