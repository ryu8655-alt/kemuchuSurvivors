using System;
using UnityEngine;

/// <summary>
/// �v���C���[�E�G�L�����N�^�[�̃X�e�[�^�X���Ǘ�����N���X
/// </summary>
/// 

//�v���C���[�ƓG�L�����N�^�[�̃X�e�[�^�X����
public enum CharacterStatusType
{
    Attack,
    Defense,
    MoveSpeed,
    HP,
    MaxHP,
    XP,
    MaxXP,
    PickUpRange,
    AliveTime,
}


public class BaseCharacterStatus
{
    //Inspector����ݒ肷��X�e�[�^�X
    //Inspector�ŕ\�������^�C�g��
    public string Title;

    //�f�[�^ID
    public int Id;
    //�ݒ背�x��
    public int Level;
    //���O
    public string Name;
    //������
    [TextArea] public string Description;
    //�U����
    public float Attack;
    //�h���
    public float Defense;
    //HP
    public float HP;
    //�ő�HP
    public float MaxHP;
    //�o���l
    public float XP;
    //�ő�o���l
    public float MaxXP;
    //�ړ����x
    public float MoveSpeed;
    //�擾�͈�
    public float PickUpRange;
    //�t�B�[���h�ɂ��鎞��(���b�o�߂���Ǝ������ł���G�p)
    public float AliveTime;

    //CaharacterStatusType�Ƃ̕R�Â����s��
    //�R�Â��ɂ̓C���f�N�T���g�p����
    public float this[CharacterStatusType type]
    {
        get
        {
            if(type == CharacterStatusType.Attack) return Attack;
            else if (type == CharacterStatusType.Defense) return Defense;
            else if (type == CharacterStatusType.MoveSpeed) return MoveSpeed;
            else if (type == CharacterStatusType.HP) return HP;
            else if (type == CharacterStatusType.MaxHP) return MaxHP;
            else if (type == CharacterStatusType.XP) return XP;
            else if (type == CharacterStatusType.MaxXP) return MaxXP;
            else if (type == CharacterStatusType.PickUpRange) return PickUpRange;
            else if (type == CharacterStatusType.AliveTime) return AliveTime;
            else return 0;
        }

        set
        {
            if(type == CharacterStatusType.Attack) Attack = value;
            else if (type == CharacterStatusType.Defense) Defense = value;
            else if (type == CharacterStatusType.MoveSpeed) MoveSpeed = value;
            else if (type == CharacterStatusType.HP) HP = value;
            else if (type == CharacterStatusType.MaxHP) MaxHP = value;
            else if (type == CharacterStatusType.XP) XP = value;
            else if (type == CharacterStatusType.MaxXP) MaxXP = value;
            else if (type == CharacterStatusType.PickUpRange) PickUpRange = value;
            else if (type == CharacterStatusType.AliveTime) AliveTime = value;
        }
    }

    //�R�s�[�����f�[�^��Ԃ�
    public BaseCharacterStatus GetCopy()
    {
        return (BaseCharacterStatus)MemberwiseClone();
    }

}

