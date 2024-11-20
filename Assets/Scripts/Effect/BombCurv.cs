using System.Collections;
using UnityEngine;

public class BombCurv : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve animCurv;

    [Header("CurvStat")]
    public GameObject target;
    public float hoverHeight;

    public Vector2 Flight(float speed, float time)
    {
        float duration = speed;
        Vector2 velocity = Vector2.zero;
        Vector3 start = transform.position;
        Vector3 end = target.transform.position;

        float linearT = time / duration;
        float heightT = animCurv.Evaluate(linearT);
        float height = Mathf.Lerp(0.0f, hoverHeight, heightT);
        velocity = (Vector2.Lerp(start, end, linearT) + new Vector2(0.0f, height)) - (Vector2)transform.position;
        return velocity;
    }
}