using UnityEngine;

public class DestroyOnDeath : MonoBehaviour
{
    private HealthSystem healthSystem;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        rigidbody = GetComponent<Rigidbody2D>();
        // ���� ���� ��ü�� healthSystem��
        healthSystem.OnDeath += OnDeath;
    }

    void OnDeath()
    {
        // ���ߵ��� ����
        rigidbody.velocity = Vector3.zero;

        // �ణ �������� �������� ����
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = renderer.color;
            // renderer.color.a = 0.3f �ȵ�
            color.a = 0.3f;
            renderer.color = color;
        }

        // ��ũ��Ʈ ���̻� �۵� ���ϵ��� ��
        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>())
        {
            component.enabled = false;
        }

        // 2�ʵڿ� �ı�
        Destroy(gameObject, 2f);
    }
}