using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDamageController : MonoBehaviour
{
    //�_���[�W�e�L�X�g�̕\������
    private float _dispTextTime = 1.0f;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAnimation();
    }

    // Update is called once per frame
    void Update()
    {
       
        UpdateTextPosition();

    }

    /// <summary>
    /// �_���[�W�e�L�X�g�̃X�|�[�����̃A�j���[�V�����ݒ�
    /// </summary>
    private void SpawnAnimation()
    {
        //�e�L�X�g���c���ł��珬�����Ȃ��Ă��������鉉�o���s��
        //�e�L�X�g���c��މ��o����n�܂�A������ɏ������Ȃ鉉�o���s��
        transform.DOScale(new Vector2(1, 1), _dispTextTime / 2).SetRelative()
            .OnComplete(() =>
            {
                //�e�L�X�g���������Ȃ��ď����鉉�o
                transform.DOScale(new Vector2(0, 0), _dispTextTime / 2).SetRelative()
                    .OnComplete(() => Destroy(gameObject));
            });
    }


    /// <summary>
    ///�e�L�X�g�̈ʒu�X�V���s������
    ///�ΏۂƂȂ�I�u�W�F�N�g�̈ʒu���擾���A�e�L�X�g�̈ʒu���X�V����
    /// </summary>
    private void UpdateTextPosition()
    {
         if (!_target) return;
         //�e�L�X�g�̈ʒu���X�V����
         Vector3 targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);
        transform.position = targetPos;
    }


    public void Init(GameObject target , float damage)
    {
        this._target = target;
        //�R���|�[�l���g�擾
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = ""+ (int)damage;

    }
}
