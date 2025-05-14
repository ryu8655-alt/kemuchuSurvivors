using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHPController : MonoBehaviour
{
    [SerializeField, Header("SliderHP")]
    private Slider _sliderHP;
    [SerializeField, Header("OffsetPosition")]
    private Vector3 _offset = new Vector3(0.0f,-75.0f, 0.0f);//スライダーの位置を調整するためのオフセット
 
    
    private PlayerController _playerController;
    private RectTransform _rectTransform;
    private Camera _mainCamera;

    public void Init(PlayerController playerController)
    {
        this._playerController = playerController;
        if(_playerController != null)
        {
            UpdateHP(_playerController.currentHP, _playerController.maxHP);
            _playerController.OnHPChanged += UpdateHP;


        }

    }


    private void Awake()
    {
        this._rectTransform = GetComponent<RectTransform>();
        this._mainCamera = Camera.main;
    }

    private void OnDisable()
    {
        if(_playerController == null) return;
    
        _playerController.OnHPChanged -= UpdateHP;
    }



    private void FixedUpdate()
    {
      MoveSliderHP();
    }


    ///// <summary>
    ///// Canvas上のHPスライダーがプレイヤーを追従する処理
    ///// 
    private void MoveSliderHP()
    {
        if (_playerController == null) return;

        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(_mainCamera, _playerController.transform.position + _offset);
        _rectTransform.position = screenPosition;
    }



    private void UpdateHP(float currentHP , float maxHP)
    {
        _sliderHP.maxValue = maxHP;
        _sliderHP.value = currentHP;
    }
}
