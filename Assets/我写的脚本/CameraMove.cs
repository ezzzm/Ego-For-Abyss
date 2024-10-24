using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform player; // ���Ƕ����Transform
    public Vector3 offset;   // ��������ǵ�ƫ��ֵ
    public float smoothSpeed = 0.125f; // ƽ���ƶ��ٶ� (�ɵ���)

    void Start()
    {
        // ��ʼ����������ǵ�ƫ��
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // �����������λ�ã���ֻ����λ�ã���������ת
        Vector3 newPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // ʹ�� Lerp ��ֵƽ���ƶ�
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);

        // �����������ת���䣨�����������ԭʼ��ת��
        transform.rotation = Quaternion.identity;
    }
}
