using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ParabolicMovement : MonoBehaviour
{
    public Vector3 launchVelocity; // �����ٶ�
    public float gravity = -9.81f; // �������ٶ�
    public int resolution = 10; // ���߷ֱ���

    private LineRenderer lineRenderer; // ������Ⱦ��
    private Vector3 position; // ��ǰλ����Ϣ
    private float time; // ʱ�����

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution + 1; // ���������ĵ�����
        DrawParabola(); // ����������
        time = 0f;
        position = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime; // ����ʱ��

        // ʹ���������˶���ʽ�����������λ��
        float x = launchVelocity.x * time; // ˮƽ�˶�
        float y = launchVelocity.y * time + 0.5f * gravity * time * time; // ��ֱ�˶�

        position = new Vector3(x, y, transform.position.z); // ����λ����Ϣ
        transform.position = position; // ������Ϸ�����λ��

        // ���������أ�����ֹͣ���»�����
        if (position.y < 0)
        {
            position.y = 0; // ȷ�������ڵ���
            launchVelocity = Vector3.zero; // ֹͣ�˶�
            this.enabled = false; // �������
        }
    }

    void DrawParabola()
    {
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution; // ��һ��ʱ��
            float x = launchVelocity.x * t; // ˮƽ�˶�
            float y = launchVelocity.y * t + 0.5f * gravity * t * t; // ��ֱ�˶�
            lineRenderer.SetPosition(i, new Vector3(x, y, 0)); // ����������ÿ����
        }
    }
}