using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class RubyController : MonoBehaviour
{
    // 发射子弹的声音
    public AudioClip launchAudioClip;

    // 角色受伤的声音
    public AudioClip hurtAudioClip;

    // 角色移动的声音
    public AudioClip moveAudioClip;

    // 移动速度
    public float speed = 100f;

    // 最大生命
    public int maxHealth = 5;

    // 设置无敌的时间间隔
    public float timeOfInvincible = 1.0f;

    // 用于获取子弹预制件
    public GameObject projectilePrefab;

    // 是否无敌
    private bool _isInvinvible;

    // 无敌时间计时器
    private float _invincibleTimer;

    // 用于获取输入的方向
    private float _horizontal, _vertical;

    // 声明音频源对象，用于进行音频播放控制
    private AudioSource _audioSource;

    // 当前生命
    private int _currentHealth;

    public int CurrentHealth
    {
        get { return _currentHealth; }
        // set { _currentHealth = value; }
    }

    // 获取角色刚体组件
    private Rigidbody2D _rigidbody2D;

    // 动画控制器,用于动画的操作
    private Animator _animator;

    // 获取角色面向的方向
    private Vector2 _lookDirection = new Vector2(1, 0);

    // 用于判断角色是否移动
    private Vector2 _move;


    void Start()
    {
        /*
        // 只有将垂直同步设置为0，才能锁帧，否则锁帧的代码无效
        // 垂直同步的作用是显著减少游戏撕裂、跳帧，因为画面的渲染不是整个画面一同渲染的，而是逐行或逐列进行渲染的，能够让FPS保持与显示屏的刷新率相同
        QualitySettings.vSyncCount = 0;
        
        // 设定应用程序帧数为0
        Application.targetFrameRate = 60;
        */

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        // 判断无敌状态的进入与取消
        if (_isInvinvible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0)
            {
                _isInvinvible = false;
            }
        }

        _move = new Vector2(_horizontal, _vertical);

        if (!Mathf.Approximately(_move.x, 0.0f) || !Mathf.Approximately(_move.y, 0.0f))
        {
            _lookDirection.Set(_move.x, _move.y);
            _lookDirection.Normalize();
        }

        // ReSharper disable once Unity.PreferAddressByIdToGraphicsParams
        _animator.SetFloat("LookX", _lookDirection.x);
        _animator.SetFloat("LookY", _lookDirection.y);
        _animator.SetFloat("Speed", _move.magnitude);

        // 发射子弹
        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire1"))
        {
            Launch();
        }

        // 创建射线投射碰撞对象,来接收射线投射碰撞信息
        // 参数一:射线发出的位置
        // 参数二:射线投射方向
        // 参数三:投射距离
        // 参数四:射线生效的层
        if (Input.GetKeyDown(KeyCode.F))
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * 0.2f, _lookDirection, 1.5f,
                LayerMask.GetMask("Npc"));
            if (null != hit.collider)
            {
                // Debug.Log($"射线投射到的对象是{hit.collider.gameObject}");
                NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
                if (npc != null)
                {
                    npc.DialogDisplay();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Time.deltaTime : 每帧的时间间隔
        Vector2 position = transform.position;
        position.x += speed * _horizontal * Time.deltaTime;
        position.y += speed * _vertical * Time.deltaTime;
        _rigidbody2D.position = position;
    }

    // 控制角色的血量更改
    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (_isInvinvible)
            {
                return;
            }

            _isInvinvible = true;
            _invincibleTimer = timeOfInvincible;

            PlaySound(hurtAudioClip, 0.9f);
            _animator.SetTrigger("Hit");
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);

        UIHealthBar.Instance.SetValue(_currentHealth / (float)maxHealth);
    }

    // 发射子弹
    private void Launch()
    {
        GameObject projectileObject =
            Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
        PlaySound(launchAudioClip, 0.8f);
        _animator.SetTrigger("Launch");
    }

    // 播放指定的音频
    public void PlaySound(AudioClip audioClip, float volume)
    {
        _audioSource.PlayOneShot(audioClip, 0.9f);
    }
}