using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class EnemyControllerPlus : MonoBehaviour
{
    public float speed;
    public float moveTime = 3.0f;
    public AudioClip fixedAudioClip;


    // 获取烟雾粒子特效
    public ParticleSystem smokeEffect;

    // 是否垂直移动
    public bool vertical;

    private float _moveTimer;
    private bool _isBroked = true;

    //方向
    private int direction = 1;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int MoveX = Animator.StringToHash("MoveX");


    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _moveTimer = moveTime;
    }

    void Update()
    {
        if (!_isBroked)
        {
            return;
        }

        _moveTimer -= Time.deltaTime;
        if (_moveTimer < 0)
        {
            direction = -direction;
            _moveTimer = moveTime;
        }
    }

    private void FixedUpdate()
    {
        if (!_isBroked)
        {
            return;
        }

        Vector2 position = _rigidbody2D.position;

        if (vertical)
        {
            position.y += Time.deltaTime * speed * direction;
            _animator.SetFloat(MoveX, 0);
            _animator.SetFloat(MoveY, direction);
        }
        else
        {
            position.x += Time.deltaTime * speed * direction;
            _animator.SetFloat(MoveX, direction);
            _animator.SetFloat(MoveY, 0);
        }

        _rigidbody2D.MovePosition(position);
    }

    // 刚体碰撞事件
    private void OnCollisionStay2D(Collision2D other)
    {
        RubyController ruby = other.gameObject.GetComponent<RubyController>();

        if (ruby != null)
        {
            ruby.ChangeHealth(-1);
        }
    }

    //  机器人修复
    public void Fix()
    {   
        // 机器人被修复, 移动停止
        _isBroked = false;
        _rigidbody2D.simulated = false;

        // 播放机器人被修复后的动画
        _animator.SetTrigger("IsFixed");

        // 关闭机器人冒烟的粒子特效(已经产生但未达到生命周期的粒子也会立即消失)
        // Destroy(smokeEffect);

        // 粒子特效停止产生新的粒子(已经产生但未达到生命周期的粒子不会立刻消失, 达到其生命周期后才会消失)
        smokeEffect.Stop();

        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().PlayOneShot(fixedAudioClip);
    }
}