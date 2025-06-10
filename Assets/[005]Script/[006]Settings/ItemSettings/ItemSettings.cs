using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�E�N���b�N���j���[�ɂĕ\������,filename
[CreateAssetMenu(fileName = "ItemSettings", menuName = "ScriptableObjects/ItemSettings")]
public class ItemSettings : ScriptableObject
{

    //�A�C�e���f�[�^
    [SerializeField,Header("�A�C�e���f�[�^")]
    public List<ItemData> _datas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

////�A�C�e���f�[�^
[System.Serializable]
public class ItemData
{
    public string Title;//���X�g���ڂ̃^�C�g�� =�@�C���X�y�N�^�[��ŕ\������l���̂悤�ȕ�

    //�A�C�e��ID
    public int Id;
    //�A�C�e����
    public string Name;
    //�A�C�e������
    [TextArea] public string Description;
    //�A�C�e���A�C�R��
    public Sprite Icon;
    //�{�[�i�X
    public List<BonusStatus> BonusStatusesType;
}

public enum BonusType
{
    //�萔�I�Ȑ��l�㏸
    Bonus,
    //�����I�Ȑ��l�㏸
    Boost,
}



[Serializable]
public class BonusStatus
{
    // �ǉ��^�C�v
    public BonusType Type;
    // �ǉ�����v���p�e�B
    public CharacterStatusType Key;
    // �ǉ�����l
    public float Value;
}
