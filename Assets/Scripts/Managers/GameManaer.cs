using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ObjectPool;
using Random = UnityEngine.Random;

public class GameManaer : MonoBehaviour
{
    public static GameManaer Instance;
    [SerializeField]private string playerTag;

    public ObjectPool ObjectPool {  get; private set; }
    public Transform Player { get; private set; }
    public ParticleSystem EffectParticle;

    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0; // wave ���� ����
    private int waveSpawnPosCount = 0; // wave ���� ��ġ

    public float spawnInterval = 0.5f; // ���� �ֱ�
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    [SerializeField] private Transform spawnPositionRoot;
    private List<Transform> spawnPositions = new List<Transform>();

    [SerializeField] private List<GameObject> rewards = new List<GameObject>();

    private void Awake()
    {
        if ( Instance != null ) Destroy(Instance);
        Instance = this;

        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();

        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        for (int i = 0;i < spawnPositionRoot.childCount; i++)
        {
            spawnPositions.Add(spawnPositionRoot.GetChild(i));
        }
    }

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        while (true)
        {
            if(currentSpawnCount == 0) // ���� ���� ��
            {
                UpdateWaveUI();

                yield return new WaitForSeconds(2f);

                ProcessWaveConditions();

                yield return StartCoroutine(SpawnEnemiesInWave());

                currentWaveIndex++;
            }

            yield return null;
        }
    }

    private void ProcessWaveConditions()
    {
        if(currentWaveIndex % 20 == 0)
        {
            RandomUpgrade();
        }

        if(currentWaveIndex % 10 == 0)
        {
            IncreaseSpawnPositions();
        }

        if(currentWaveIndex % 5 == 0)
        {
            CreateReward();
        }

        if(currentWaveIndex % 3 == 0)
        {
            IncreaseWaveSpawnCount();
        }
    }

    // ���� ����
    private void RandomUpgrade()
    {
        Debug.Log("RandomUpgrade ȣ��");
    }

    // ���� ���� �� ����
    private void IncreaseSpawnPositions()
    {
        waveSpawnPosCount = waveSpawnCount * 1 > spawnPositions.Count ? waveSpawnPosCount : waveSpawnCount + 1;
        waveSpawnCount = 0;
    }

    // ������ ����
    private void CreateReward()
    {
        int selectedRewardIndex = Random.Range(0, rewards.Count);
        int randomPositionIndex = Random.Range(0, spawnPositions.Count);

        GameObject obj = rewards[selectedRewardIndex];
        Instantiate(obj, spawnPositions[randomPositionIndex].position, Quaternion.identity);
    }

    // ��ġ �þ
    private void IncreaseWaveSpawnCount()
    {
        waveSpawnCount++;
    }

    private IEnumerator SpawnEnemiesInWave()
    {
        for (int i = 0;i < waveSpawnPosCount; i++)
        {
            int posIdx = Random.Range(0, spawnPositions.Count);
            for (int j = 0; j < waveSpawnCount; j++)
            {
                SpawnEnemyAtPosition(posIdx);
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnEnemyAtPosition(int posIdx)
    {
        int prefabIdx = Random.Range(0, enemyPrefabs.Count);
        GameObject enemy = Instantiate(enemyPrefabs[prefabIdx], spawnPositions[posIdx].position, Quaternion.identity);
        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath;
        currentSpawnCount++;
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }

    private void GameOver()
    {
        // UI ���ֱ�
        gameOverUI.SetActive(true);
    }

    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    private void UpdateWaveUI()
    {
        waveText.text = $"{currentWaveIndex + 1}";
    }

    public void RestartGame()
    {
        // �� �ε����� ���� �ε�� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}