using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    // 对话框显示时长
    public float displayTime = 4.0f;

    // 获取对话框游戏对象
    public GameObject dialogBox;

    public GameObject dialogTMP;


    // 对话框计时器
    private float _diaplayTimer;

    private TextMeshProUGUI _tmTxtBox;
    private int _currentPage = 1;
    private int _totalPage;


    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        _diaplayTimer = -1.0f;
        _tmTxtBox = dialogTMP.GetComponent<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        _totalPage = _tmTxtBox.textInfo.pageCount; 

        if (_diaplayTimer >= 0.0f)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (_currentPage < _totalPage)
                {
                    _currentPage++;
                }
                else
                {
                    _currentPage = 1;
                }

                _diaplayTimer = displayTime;
            }

            _tmTxtBox.pageToDisplay = _currentPage;

            _diaplayTimer -= Time.deltaTime;
            if (_diaplayTimer < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DialogDisplay()
    {
        _diaplayTimer = displayTime;
        dialogBox.SetActive(true);
    }
}