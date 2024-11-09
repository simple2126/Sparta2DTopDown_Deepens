using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPool;

public class GameManaer : MonoBehaviour
{
    public static GameManaer Instance;
    [SerializeField]private string playerTag;

    public ObjectPool ObjectPool {  get; private set; }
    public Transform Player { get; private set; }

    public ParticleSystem EffectParticle;

    private void Awake()
    {
        if ( Instance != null ) Destroy(Instance);
        Instance = this;

        Player = GameObject.FindGameObjectWithTag(playerTag).transform;
        ObjectPool = GetComponent<ObjectPool>();
        EffectParticle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
    }
}