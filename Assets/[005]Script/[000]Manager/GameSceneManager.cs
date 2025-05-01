using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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


    // Start is called before the first frame update
    void Start()
    {

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
        
    }
}
