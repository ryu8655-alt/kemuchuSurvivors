using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPContoroller : MonoBehaviour
{
    GameSceneManager _gameSceneManager;
    Rigidbody2D _rifgidbody2d;
    SpriteRenderer _spriteRenderer;

    //�o���l��
    private float _xp;

    //�o���l�A�C�e�����}�b�v��ɑ��݂��鎞��
    float _aliveTimer = 50.0f;
    //�o���l�A�C�e�������łɂ����鎞��
    float fadeTime = 10.0f;

    //�ϐ�����������
    public void Init(GameSceneManager sceneManager , float xp)
    {
        this._gameSceneManager = sceneManager;
        this._xp = xp;

        _rifgidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {

        //�Q�[������~���Ă���ꍇ�͏����𔲂���
        if (!_gameSceneManager.enabled) return;

        //�^�C�}�[��������
        _aliveTimer -= Time.deltaTime;

        if( 0 > _aliveTimer) 
        {
            //�A���t�@�l��ݒ�
            Color color = _spriteRenderer.color;
            color.a -= 1.0f / fadeTime * Time.deltaTime;
            _spriteRenderer.color = color;

            //�A���t�@�l��0�ȉ��ɂȂ�����폜
            if(0 >= color.a)
            {
                Destroy(gameObject);
                return;
            }

        }

        //�v���C���[�Ƃ̋������Q�Ƃ���
        float distance = Vector2.Distance(transform.position,_gameSceneManager._playerController.transform.position);
        //�v���C���[�̎擾�͈͓��Ȃ�v���C���[�ɋz�����܂��
        if(distance < _gameSceneManager._playerController._characterStatus.PickUpRange)
        {
            //������������
            float speed = _gameSceneManager._playerController._characterStatus.MoveSpeed * 1.1f;
            Vector2 foward = _gameSceneManager._playerController.transform.position - transform.position;
            _rifgidbody2d.position += foward.normalized * speed * Time.deltaTime;
        }

    }



    //�Փˎ�����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ȊO�Փ˂������͏����𔲂���
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;

        //�o���l���擾����
        //�v���C���[�Ɍo���l�擾������ǉ�����
        player.GetXP(_xp);
        Destroy(gameObject);
    }
}
