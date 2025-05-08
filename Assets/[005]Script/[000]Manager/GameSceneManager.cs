using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    //�^�C���}�b�v
    [SerializeField, Header("�}�b�v")]
    private GameObject _gridMap;
    [SerializeField, Header("�}�b�v��̏�Q��")]
    private Tilemap _timemapCollider;//�̂��ق�Scene���ɔz�u���s���\��

    //�}�b�v�S�̍��W
    public Vector2 _tileMapStart;
    public Vector2 _tileMapEnd;
    public Vector2 _worldStart;
    public Vector2 _worldEnd;

    [SerializeField, Header("�v���C���[")]
    public PlayerController _playerController;

    [SerializeField, Header("Transform - TextDamage")]
    private Transform _parentTextDamage;

    [SerializeField, Header("Prefab-TextDamage ")]
    private GameObject _prefabTextDamage;

    //�^�C�}�[
    [SerializeField, Header("TextTimer")]
    private Text _textTimer;
    public float _gameTimer;
    public float _oldSeconds;

    //�G�l�~�[�X�|�i�[
    [SerializeField, Header("EnemySpawner")]
    private EnemySpawnerController _enemySpawnerController;


    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�
        _oldSeconds = -1;
        _enemySpawnerController.Init(this, _timemapCollider);

        //�J�����̈ړ����ł���͈͂�ݒ肷��
        foreach (Transform item in _gridMap.GetComponentInChildren<Transform>())
        {
            //�J�n�ʒu
            if (_tileMapStart.x > item.position.x)
            {
                _tileMapStart.x = item.position.x;
            }
            if (_tileMapStart.y > item.position.y)
            {
                _tileMapStart.y = item.position.y;
            }

            //�I���ʒu
            if (_tileMapEnd.x < item.position.x)
            {
                _tileMapEnd.x = item.position.x;
            }
            if (_tileMapEnd.y < item.position.y)
            {
                _tileMapEnd.y = item.position.y;
            }
        }

            //��ʏc�����̕`��͈�(�f�t�H���g��Ԃ�5�^�C������z��)
            float cameraSize = Camera.main.orthographicSize;
            //��ʂ̏c���䗦
            float aspect = (float)Screen.width / (float)Screen.height;
            //�v���C���[�̈ړ��ł���͈�
            _worldStart =new Vector2 (_tileMapStart.x - cameraSize * aspect, _tileMapStart.y - cameraSize);
            _worldEnd = new Vector2(_tileMapEnd.x + cameraSize * aspect, _tileMapEnd.y + cameraSize);
    }

        // Update is called once per frame
        void Update()
    {
        //�Q�[�����̃^�C�}�[���X�V����
       UpdateGameTimer();
    }

    public void DispDamage(GameObject target , float damage)
    {
        GameObject obj = Instantiate(_prefabTextDamage, _parentTextDamage);
        obj.GetComponent<TextDamageController>().Init(target, damage);
    }



    private void UpdateGameTimer()
    {
        _gameTimer += Time.deltaTime;

        //�b���̕ω������邩���m�F
        int seconds = (int)_gameTimer % 60;
        if (seconds == _oldSeconds) return;
        
        _textTimer.text = Utils.GetTextTimer(_gameTimer);
        _oldSeconds = seconds;
    }
}
