using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    // �� �ð��� ������ ���� ���
    [SerializeField] private float invincibleDuration;
    [SerializeField] private AudioClip damageClip;

    private CharacterStatsHandler statsHandler;
    // ������ ������ �ް� �󸶳� �ð��� ��������
    private float currentInvincibilityTime = 0f;

    // ü���� ������ �� �� �ൿ���� �����ϰ� ���� ����
    public event Action OnDamage;
    public event Action OnHeal;
    public event Action OnDeath;
    public event Action OnInvincibilityEnd;

    public float CurrentHealth { get; private set; }

    // get�� ������ ��ó�� ������Ƽ�� ����ϴ� ��
    // �̷��� �ϸ� �������� �������� �������� ���ƴٴϴٰ� ��ũ�� ������ ������ ���� �� �־��!
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
        // ���� �����ʰ� ����
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