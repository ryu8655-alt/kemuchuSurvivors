using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

//�G�l�~�[�X�|�[���p�^�[���̎��
public enum SpawnPattern
{
    Normmal,
    Group,
}


//�G�l�~�[�X�|�[���ݒ�f�[�^
[Serializable]
public class EnemySpawnData
{
    //�C���X�y�N�^�[��łł̃^�C�g��
    public string _title;

    //�o���o�ߎ���
    [Header("�o���o�ߎ���")]
    public int _elapsedMinutes;
    public int _elapsedSeconds;

    [Header("�X�|�[���p�^�[��")]
    public SpawnPattern _spawnPattern;

    [Header("�X�|�[������")]
    public float _spawnDuration;

    [Header("�ő�X�|�[����")]
    public int _spawncountMax;

    [Header("�X�|�[����������G�l�~�[�f�[�^ID")]
    public List<int> _enemyIds;

}

public class EnemySpawnerController : MonoBehaviour
{
    //�G�f�[�^
    [SerializeField]
    private List<EnemySpawnData> _enemySpawnDatas;
    //�X�|�[�����ꂽ�G
    List<EnemyController> enemies;

    //�Q�[���V�[���}�l�[�W���[
    GameSceneManager _gameSceneManager;
    //�����蔻��̂���^�C���}�b�v
    Tilemap _tilemapCollider;
    EnemySpawnData _enemySpawnData;
    //�o�ߎ���
    float _oldSeconds;
    float _spawnTimer;

    //���݂̃f�[�^�Q�ƈʒu
    private int _spawnDataIndex;
    //�G�̃X�|�[���ʒu
    const float _spawnRadius = 13;


    //����������
    public�@void Init(GameSceneManager gameSceneManager, Tilemap tilemapCollider)
    {
        this._gameSceneManager = gameSceneManager;
        this._tilemapCollider = tilemapCollider;

        //���������G�f�[�^���i�[���郊�X�g������������
        enemies = new List<EnemyController>();
        //�Q�ƃf�[�^�̈ʒu������������
        _spawnDataIndex = -1;
    }

    private void Update()
    {
        //�G�����f�[�^�̍X�V
        UpdateEnemySpawnData();

        //�X�|�[������
        EnemySpawn();
    }


    /// <summary>
    /// �G�̃X�|�[��������EnemySpaenData����SpawnPattern�ɏ]���čs��
    /// </summary>
    private void EnemySpawn()
    {
        if (_enemySpawnData == null) return;

        //�^�C�}�[�̏�������
        _spawnTimer -= Time.deltaTime;
        if(_spawnTimer > 0) return;

        //SpawnPattern�̎�ނɏ]���Ă��ꂼ��̃X�|�[�����������s����
        if (SpawnPattern.Normmal == _enemySpawnData._spawnPattern)
        {
            NormalSpawn();
        }
        else if (SpawnPattern.Group == _enemySpawnData._spawnPattern)
        {
            GroupSpawn();
        }

        _spawnTimer = _enemySpawnData._spawnDuration;

    }



    private void CreateRandomEnemy(Vector3 position)
    {
        //�f�[�^�����烉���_����ID���擾����
        int rnd = Random.Range(0, _enemySpawnData._enemyIds.Count);
        int id = _enemySpawnData._enemyIds[rnd];

        //�G����
        EnemyController enemy = CharacterSettings.Instance.CretaeEnemy(id , _gameSceneManager, position);
        enemies.Add(enemy);
    }

    /// <summary>
    /// SpawnPattern.Normmal�̃X�|�[������
    /// </summary>
    private void NormalSpawn()
    {
        //�v���C���[�̈ʒu�𒆐S�_�Ƃ��Ď擾����
        Vector3 center = _gameSceneManager._playerController.transform.position;

        //�G�̐�������
        for(int i = 0; i < _enemySpawnData._spawncountMax; ++i)
        {
            //�v���C���[�̎��肩��o������
            float angle = 360 / _enemySpawnData._spawncountMax * i;
            // Cos�֐��Ƀ��W�A���p���w�肷��ƁAx�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;
            // Sin�֐��Ƀ��W�A���p���w�肷��ƁAy�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

            //�X�|�[���ʒu���v�Z����
            Vector2 pos = center + new Vector3(x, y, 0);

            //�v�Z�����X�|�[���ʒu�ɓ����蔻��̂���^�C���}�b�v�����݂���ꍇ�͐������s��Ȃ�
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            //�G�̃X�|�[������
            CreateRandomEnemy(pos);

        }
    }

    private void GroupSpawn()
    {
        //�v���C���[�̈ʒu�𒆐S�_�Ƃ��Ď擾������
        Vector3 center = _gameSceneManager._playerController.transform.position;

        //�G���v���C���[�̎��肩��o��������
        float angle = Random.Range(0, 360);
        // Cos�֐��Ƀ��W�A���p���w�肷��ƁAx�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * _spawnRadius;
        // Sin�֐��Ƀ��W�A���p���w�肷��ƁAy�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * _spawnRadius;

        //�X�|�[���ʒu�̌v�Z
        center += new Vector3(x, y, 0);
        float radius = 0.5f;

        //�G�X�|�[������
        for (int i = 0; i < _enemySpawnData._spawncountMax; i++)
        {
            //�v���C���[�̎��肩��o������悤�ɂ���
            angle = 360 / _enemySpawnData._spawncountMax * i;
            // Cos�֐��Ƀ��W�A���p���w�肷��ƁAx�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            // Sin�֐��Ƀ��W�A���p���w�肷��ƁAy�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            //�����ʒu
            Vector2 pos = center + new Vector3(x, y, 0);
            //�����蔻��̂���^�C���}�b�v�����݂���ꍇ�͐������s��Ȃ�
            if (Utils.IsColliderTile(_tilemapCollider, pos)) continue;

            //�G�̃X�|�[������
            CreateRandomEnemy(pos);
        }
    }


    private void UpdateEnemySpawnData()
    {
        //�o�ߕb���ɈႢ���Ȃ��Ƃ��͏����𔲂���
        if (_oldSeconds == _gameSceneManager._gameTimer) return;

        //�P��̃f�[�^���Q�Ƃ���
        int idx = _spawnDataIndex + 1;

        //�f�[�O���Ō�ɗ����Ƃ��͏����𔲂���
        if (_enemySpawnDatas.Count - 1 < idx) return;

        //�ݒ肳�ꂽ�o�ߎ��Ԃ𒴂��Ă����ꍇ�̓f�[�^�̓���ւ����s��
        EnemySpawnData data = _enemySpawnDatas[idx];
        int elapsedSeconds = data._elapsedMinutes * 60 + data._elapsedSeconds;

        if(elapsedSeconds < _gameSceneManager._gameTimer)
        {
            _enemySpawnData = _enemySpawnDatas[idx];

            //����p�̐ݒ�
            _spawnDataIndex = idx;
            _spawnTimer = 0;
            _oldSeconds = _gameSceneManager._gameTimer;
        }
    }

    public List<EnemyController> GetEnemies()
    {
        enemies.RemoveAll(item => !item);
        return enemies;
    }

}