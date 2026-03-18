using UnityEngine;

/// <summary>
/// 2.5D摄像机跟随脚本
/// 保持固定角度跟随目标，适用于等距视角游戏
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("跟随目标")]
    [SerializeField] private Transform target;
    
    [Header("跟随设置")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -12f);
    [SerializeField] private float smoothSpeed = 0.125f;
    
    [Header("旋转设置")]
    [SerializeField] private float xRotation = 40f;
    
    private Vector3 currentOffset;
    
    private void Start()
    {
        // 应用旋转角度
        transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        // 计算基于旋转的偏移
        currentOffset = CalculateOffset();
    }
    
    private void FixedUpdate()
    {
        if (target == null) return;
        
        FollowTarget();
    }
    
    /// <summary>
    /// 跟随目标
    /// </summary>
    private void FollowTarget()
    {
        // 目标位置 + 偏移
        Vector3 desiredPosition = target.position + currentOffset;
        
        // 平滑移动
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // 应用位置
        transform.position = smoothedPosition;
    }
    
    /// <summary>
    /// 计算基于摄像机角度的偏移
    /// </summary>
    private Vector3 CalculateOffset()
    {
        // 根据X轴旋转角度计算偏移
        Quaternion rotation = Quaternion.Euler(xRotation, 0f, 0f);
        return rotation * offset;
    }
    
    /// <summary>
    /// 在编辑器中绘制调试 Gizmos
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 desiredPos = target.position + CalculateOffset();
            Gizmos.DrawLine(transform.position, desiredPos);
            Gizmos.DrawWireSphere(desiredPos, 0.5f);
        }
    }
}
