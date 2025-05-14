using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHPController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField, Header("SliderHP")]
    private Slider _sliderHP;
    [SerializeField, Header("OffsetPosition")]
    private Vector3 _offset = new Vector3(0.0f,-75.0f, 0.0f);//スライダーの位置を調整するためのオフセット
    private RectTransform _rectTransform;

    public void SetPlayer(PlayerController playerController)
    {
        this._playerController = playerController;
        if(_playerController != null)
        {


        }

    }
}
