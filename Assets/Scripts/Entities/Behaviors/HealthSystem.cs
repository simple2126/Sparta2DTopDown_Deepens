using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // 이 시간이 지나면 공격 허용
    [SerializeField] private float invincibleDuration;
    [SerializeField] private AudioClip damageClip;

    private CharacterStatsHandler statsHandler;
    // 마지막 공격을 받고 얼마나 시간이 지났는지
    private float currentInvincibilityTime = 0f;

    // 체력이 변했을 때 할 행동들을 정의하고 적용 가능
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }

    // get만 구현된 것처럼 프로퍼티를 사용하는 것
    // 이렇게 하면 데이터의 복제본이 여기저기 돌아다니다가 싱크가 깨지는 문제를 막을 수 있어요!
    public float MaxHealth => statsHandler.CurrentStat.maxHealth;

    public bool isAttacked = false;

    private void Awake()
    {
        statsHandler = GetComponent<CharacterStatsHandler>();
    }

    private void Start()
    {
        CurrentHealth = statsHandler.CurrentStat.maxHealth;
    }

    private void Update()
    {
        // 공격 하지않고 끝남
        if (isAttacked && currentInvincibilityTime < invincibleDuration)
        {
            currentInvincibilityTime += Time.deltaTime;
            if (currentInvincibilityTime >= invincibleDuration)
            {
                OnInvincibilityEnd?.Invoke();
                currentInvincibilityTime = 0f;
                isAttacked = false;
            }
        }
    }

    public void ChangeHealth(float change)
    {
        if (CurrentHealth == 0f)
        {
            return;
        }

        CurrentHealth += change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, MaxHealth);

        if(CurrentHealth == 0f)
        {
            OnDeath?.Invoke();
            return;
        }

        if (change >= 0f)
        {
            OnHeal?.Invoke();
        }
        else
        {
            OnDamage?.Invoke();
            isAttacked = true;

            if (damageClip) SoundManager.PlayClip(damageClip);
        }
    }

    private void CallDeath()
    {
        OnDeath?.Invoke();
    }
}