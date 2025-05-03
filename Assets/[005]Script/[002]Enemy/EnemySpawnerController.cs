using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Rnadom = UnityEngine.Random;

public enum SpawnTyep
{
    Normal,
    Group,
}

[System.Serializable]
public class EnemySpawnData
{
    //�C���X�y�N�^�[��ł̐����p
    public string Title;

    [Header("�o���o�ߎ���")]
    public float ElapsedMinutes;
    public int ElapsedSeconds;

    [Header("�o���p�^�[��")]
    public SpawnTyep SpawnType;

    [Header("��������")]
    public float SpawnDuration;
    [Header("�ő吶����")]
    public int _spawnCountMax;
    [Header("��������G��ID")]
    public List<int> EnemyIDs;
}




public class EnemySpawnerController : MonoBehaviour
{
    //�G�̃f�[�^
    [SerializeField,Header("�G�̃f�[�^")]
    private List<EnemySpawnData> _enemySpawnDataList;

    //�����������G�̃��X�g
    List<EnemyController> enemies;

    GameSceneManager _gameSceneManager;
    //�����蔻��̂���^�C���}�b�v
    Tilemap tilemapCollider;
    //���݂̎Q�ƃf�[�^
    EnemySpawnData _enemySpawnData;

    //�o�ߎ���
    float _oldSeconds;
    float _spawnTimer;
    //���݂̃f�[�^�ʒu
    int _spawnDataIndex;
    //�G�̐����ʒu(�����p�^�[��Grop�ɂĎg�p�\��)
    const float SpawnRadius = 13;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�K�����f�[�^���X�V
       
        //�G�̃X�|�[������
    }

    public void Init(GameSceneManager sceneManager , Tilemap timemapCollider)
    {
        this._gameSceneManager = sceneManager;
        this.tilemapCollider = timemapCollider;


        //���X�g�̏�����
        enemies = new List<EnemyController>();
        _spawnDataIndex = -1;
    }

    private void SpawnEnemy()
    {
        if (null == _enemySpawnData) return;

        //�^�C�}�[�̏���
        _spawnTimer -= Time.deltaTime;
        if (0 < _spawnTimer) return;

        if (SpawnTyep.Group == _enemySpawnData.SpawnType)
        {
            SpawnGroup();
        }else if(SpawnTyep.Normal ==_enemySpawnData.SpawnType){

            SpawnNormal();
        }

        _spawnTimer= _enemySpawnData.SpawnDuration;

    }

    private void SpawnNormal()
    {
        //�v���C���[�̈ʒu���擾���A�X�|�[���͈͂̒��S�Ƃ��Đݒ�
        Vector3 center = _gameSceneManager._playerController.transform.position;

    

        //�G�𐶐�����
        for (int i = 0; i < _enemySpawnData._spawnCountMax; ++i)
        {
                  
            //�v���C���[�̎��͂���G���o��������
            float angle = Random.Range(0, 360);
            // Cos�֐��Ƀ��W�A���p���w�肷��ƁAx�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * SpawnRadius;
            // Sin�֐��Ƀ��W�A���p���w�肷��ƁAy�̍��W��Ԃ��Ă����Aradius�������ă��[���h���W�ɕϊ�����
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * SpawnRadius;

            //�����ʒu�̌���
            Vector2 pos = center + new Vector3(x,y,0);

            //�����蔻��̂���^�C���}�b�v�̏�ɐ�������Ȃ��悤�ɂ���
            if (Utils.IsColliderTile(tilemapCollider, pos)) continue;

            //����
            CreateRandomEnemy(pos);
        }
    }

    private void SpawnGroup()
    {

    }

    private void CreateRandomEnemy(Vector3 pos)
    {

    }
}
