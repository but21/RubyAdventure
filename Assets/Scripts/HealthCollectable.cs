using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    // 收集物加的血量
    private int _healthAmount = 1;

    // 物品被收集时播放的音频剪辑变量
    public AudioClip collectedClip;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RubyController ruby = other.GetComponent<RubyController>();

        if (ruby != null && ruby.CurrentHealth < ruby.maxHealth)
        {
            ruby.ChangeHealth(_healthAmount);
            ruby.PlaySound(collectedClip, 0.9f);
            Destroy(gameObject);
        }
    }
}