using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderXPController : MonoBehaviour
{
    [SerializeField, Header("SliderXP")]
    private Slider _sliderXP;

    private PlayerController _playerController;


    public void Init(PlayerController playerController)
    {
        this._playerController = playerController;
        if(_playerController != null)
        {
            UpdateXP(_playerController.currentXP, _playerController.maxXP);
        }

    }


    private void UpdateXP(float currentXP , float maxXP)
    {
        _sliderXP.maxValue = maxXP;
        _sliderXP.value = currentXP;
    }

}
