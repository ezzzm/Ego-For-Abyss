using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform player; // 主角对象的Transform
    public Vector3 offset;   // 相机与主角的偏移值
    public float smoothSpeed = 0.125f; // 平滑移动速度 (可调节)

    void Start()
    {
        // 初始化相机与主角的偏移
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // 计算相机的新位置，但只跟随位置，不跟随旋转
        Vector3 newPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

        // 使用 Lerp 插值平滑移动
        transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed);

        // 保持相机的旋转不变（即保持相机的原始旋转）
        transform.rotation = Quaternion.identity;
    }
}
