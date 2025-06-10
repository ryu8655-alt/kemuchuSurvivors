using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Q�[���ɓo�ꂷ��S�L�����N�^�[�̃X�e�[�^�X�����Ǘ�����scriptableObject
/// ID���w�肷�邱�ƂŁA�L�����N�^�[�̃X�e�[�^�X���擾����
/// </summary>
[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableProject/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    [Tooltip("�L�����N�^�[�̃X�e�[�^�X�f�[�^���X�g")]
    public List<CharacterStatus> _characterStatusList;

    private static CharacterSettings _instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
                if(_instance == null)
                {
                    Debug.LogError("[CharacterSettings] Resources�t�H���_����CharacterSettings.asset��������܂���");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// �w�肵�� ID �Ɉ�v����L�����N�^�[�X�e�[�^�X���R�s�[���ĕԂ�
    /// </summary>
    /// <param name="id">�L�����N�^�[ID</param>
    /// <returns>�Y������ CharacterStatus �̃R�s�[�B�Ȃ���� null�B</returns>aram>
    /// <returns></returns>
    public CharacterStatus Get(int id)
    {
        var foundStatus = _characterStatusList.Find(item => item.Id == id);
        if(foundStatus == null)
        {
            Debug.LogError($"[CharacterSettings] ID {id} �̃L�����N�^�[��������܂���");
            return null;
        }

        return (CharacterStatus)foundStatus.GetCopy();


        // return (CharacterStatus)_characterStatusList.Find(item => item.Id == id).GetCopy();
    }


    public EnemyController CretaeEnemy(int id , GameSceneManager gameSceneManager, Vector3 position)
    {
        //�w�肵��ID�̃L�����N�^�[�X�e�[�^�X���擾����
        CharacterStatus characterStatus = Instance.Get(id);
        //�Ώۂ̃L�����N�^�[�v���n�u���擾����
        GameObject obj = Instantiate(characterStatus._characterPrefab, position, Quaternion.identity);

        //�f�[�^�Z�b�g
        EnemyController�@enemyController = obj.GetComponent<EnemyController>();
        enemyController.Init(gameSceneManager, characterStatus);

        return enemyController;
    }

    public PlayerController CreatePlayer(int id ,GameSceneManager gameSceneManager,EnemySpawnerController enemySpawner,
        TextMeshProUGUI textLV , Slider sliderXP)
    {
        //ID�w�肳�ꂽ�L�����N�^�[�X�e�[�^�X�̎擾���s��
        CharacterStatus status =  Instance.Get(id);
        //�I�u�W�F�N�g����
        GameObject obj = Instantiate(status._characterPrefab, Vector3.zero, Quaternion.identity);


        //�f�[�^�Z�b�g
        PlayerController ctrl = obj.GetComponent<PlayerController>();
        ctrl.Init(gameSceneManager,enemySpawner,status,textLV,sliderXP);

        return ctrl;
    }       




}









/// <summary>
/// �G�̈ړ��^�C�v���`����񋓌^
/// </summary>
public enum MoveType
{
    //�v���C���[�p
    Player,
    //�v���C���[�Ɍ������Đi��ł���
    TargetPlayer,
    //������ɐi��
    TargetDirection,
    //�����Ȃ��ň�苗���߂Â���Player�ɒe���΂�(��قǎ����\��)
    ShootOnSight

}

/// <summary>
/// �v���C���[�E�G���ʂŎg���L�����N�^�[�X�e�[�^�X�f�[�^�\���B
/// ScriptableObject ���Ŏg�p�����B
/// </summary>
[Serializable]
public class CharacterStatus : BaseCharacterStatus
{
    [Header("�L�����N�^�[�v���n�u")]
    public GameObject _characterPrefab;

    [Header("�v���C���[���p�����[�^�[")]
    //���������ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _defaultWeaponIds;
    //�����\�ȕ����ID = Player�ɂĎg�p����p�����[�^�[
    public List<int> _usableWeaponIds;
    //�����\�� = Player�ɂĎg�p����p�����[�^�[
    public int _usableWeaponMax;

    [Header("�G���p�����[�^�[")]
    //�ړ��̃^�C�v�@= Enemy�ɂĎg�p����
    public MoveType _moveType;

    //�A�C�e����ǉ����鏈������Œǉ�����
}

