using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ParabolicMovement : MonoBehaviour
{
    public Vector3 launchVelocity; // 发射速度
    public float gravity = -9.81f; // 重力加速度
    public int resolution = 10; // 曲线分辨率

    private LineRenderer lineRenderer; // 线条渲染器
    private Vector3 position; // 当前位置信息
    private float time; // 时间变量

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution + 1; // 设置线条的点数量
        DrawParabola(); // 绘制抛物线
        time = 0f;
        position = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime; // 更新时间

        // 使用抛物线运动公式计算物体的新位置
        float x = launchVelocity.x * time; // 水平运动
        float y = launchVelocity.y * time + 0.5f * gravity * time * time; // 垂直运动

        position = new Vector3(x, y, transform.position.z); // 更新位置信息
        transform.position = position; // 更新游戏对象的位置

        // 如果物体落地，可以停止更新或重置
        if (position.y < 0)
        {
            position.y = 0; // 确保不低于地面
            launchVelocity = Vector3.zero; // 停止运动
            this.enabled = false; // 禁用组件
        }
    }

    void DrawParabola()
    {
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution; // 归一化时间
            float x = launchVelocity.x * t; // 水平运动
            float y = launchVelocity.y * t + 0.5f * gravity * t * t; // 垂直运动
            lineRenderer.SetPosition(i, new Vector3(x, y, 0)); // 设置线条的每个点
        }
    }
}