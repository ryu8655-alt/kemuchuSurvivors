using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����p�̊�{�X�e�[�^�X�N���X
/// </summary>
/// 

//����X�e�[�^�X����
public enum WeaponStatusType
{
    Attack,
    PenetrationCount,
    Speed,
    AliveTime,
    SpawnCount,
    SpawnIntervalMax,
    SpawnIntervalMin,
}

[System.Serializable]
public class BaseWeaponStatus
{
    //Inspector����ݒ肷��X�e�[�^�X
    [Header("��{���")]
    //Inspector�ŕ\�������^�C�g��
    public string Title;

    //����f�[�^ID
    public int Id;
    //�ݒ背�x��
    public int Level;
    //���O
    public string Name;
    //������
    [TextArea] public string Description;

    [Header("�퓬�X�e�[�^�X")]
    //�U����
    public float Attack;
    //�ђʐ�
    public float PenetrationCount;
    //�e�̈ړ����x
    public float Speed;
    //�t�B�[���h��ɂ��鎞��
    public float AliveTime;

    [Header("�O��")]
    //����X�|�i�[�̃v���n�u
    public GameObject PrefabSpawner;
    //����A�C�R��
    public Sprite Icon;

    [Header("�X�|�[���ݒ�")]
    //��x�ɃX�|�[������鐔
    public int SpawnCount;
    //�X�|�[���Ԋu�̍ő�l
    public float SpawnIntervalMax;
    //�X�|�[���Ԋu�̍ŏ��l
    public float SpawnIntervalMin;

    [Header("���x���A�b�v�ɂ�鐬����`")]
    //���x���A�b�v�ɂ�鐬����`
    public List<WeaponGrowthStep> GrowSteps;

    //WeaponStatusTypet�Ƃ̕R�Â����s��
    //�R�Â��ɂ̓C���f�N�T���g�p����
    public float this[WeaponStatusType type]
    {
        get => type switch
        {
            WeaponStatusType.Attack => Attack,
            WeaponStatusType.PenetrationCount => PenetrationCount,
            WeaponStatusType.Speed => Speed,
            WeaponStatusType.AliveTime => AliveTime,
            WeaponStatusType.SpawnCount => SpawnCount,
            WeaponStatusType.SpawnIntervalMax => SpawnIntervalMax,
            WeaponStatusType.SpawnIntervalMin => SpawnIntervalMin,
            _ => 0,
        };

        set
        {
            switch (type)
            {
                case WeaponStatusType.Attack:
                    Attack = value;
                    break;
                case WeaponStatusType.PenetrationCount:
                    PenetrationCount = value;
                    break;
                case WeaponStatusType.Speed:
                    Speed = value;
                    break;
                case WeaponStatusType.AliveTime:
                    AliveTime = value;
                    break;
                case WeaponStatusType.SpawnCount:
                    SpawnCount = (int)value;
                    break;
                case WeaponStatusType.SpawnIntervalMax:
                    SpawnIntervalMax = value;
                    break;
                case WeaponStatusType.SpawnIntervalMin:
                    SpawnIntervalMin = value;
                    break;
            }
        }
    }

    public BaseWeaponStatus GetCopy()
    {
        return (BaseWeaponStatus)MemberwiseClone();
    }


    public void ApplyGrowthStep(int level)
    {
        foreach (var step in GrowSteps)
        {
            if (step.Level != level) continue;

            foreach (var bonus in step.Bonuses)
            {
                float original = this[bonus.Type];

                if (bonus.ApplyType == BonusApplyType.Flat)
                {
                    this[bonus.Type] = original + bonus.Value;
                }
                else if (bonus.ApplyType == BonusApplyType.Rate)
                {
                    this[bonus.Type] = original * (1 + bonus.Value);
                }
            }
        }
    }

}




