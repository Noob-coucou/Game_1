using UnityEngine;

/// <summary>
/// 简单的2.5D角色控制器
/// 用于等距视角/2.5D游戏的角色移动
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("移动设置")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    
    [Header("组件")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    private float horizontalInput;
    private float verticalInput;
    private bool isGrounded;
    private bool isJumping;
    
    // 地面检测半径
    private float groundCheckRadius = 0.2f;
    
    // 输入轴名称（可在Edit > Project Settings > Input中修改）
    private string horizontalAxis = "Horizontal";
    private string verticalAxis = "Vertical";
    private string jumpButton = "Jump";
    
    private void Awake()
    {
        // 如果没有手动绑定Rigidbody，自动获取
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        
        // 如果没有绑定地面检测点，创建一个
        if (groundCheck == null)
        {
            GameObject checkPoint = new GameObject("GroundCheck");
            checkPoint.transform.SetParent(transform);
            checkPoint.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = checkPoint.transform;
        }
    }
    
    private void Update()
    {
        // 获取输入
        horizontalInput = Input.GetAxisRaw(horizontalAxis);
        verticalInput = Input.GetAxisRaw(verticalAxis);
        
        // 跳跃输入
        if (Input.GetButtonDown(jumpButton) && isGrounded)
        {
            isJumping = true;
        }
        
        // 地面检测
        CheckGround();
    }
    
    private void FixedUpdate()
    {
        Move();
        
        if (isJumping)
        {
            Jump();
            isJumping = false;
        }
    }
    
    /// <summary>
    /// 角色移动 - 2.5D限制在X轴和Z轴
    /// </summary>
    private void Move()
    {
        // 计算移动方向（仅在水平面移动）
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        // 应用移动
        Vector3 targetVelocity = moveDirection * moveSpeed;
        
        // 保持Y轴速度（重力）
        targetVelocity.y = rb.linearVelocity.y;
        
        // 使用Rigidbody移动
        rb.linearVelocity = targetVelocity;
        
        // 如果有移动输入，让角色朝向移动方向
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                toRotation, 
                720f * Time.fixedDeltaTime
            );
        }
    }
    
    /// <summary>
    /// 跳跃
    /// </summary>
    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        }
    }
    
    /// <summary>
    /// 检测是否在地面
    /// </summary>
    private void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
    }
    
    /// <summary>
    /// 在编辑器中绘制调试 Gizmos
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 绘制地面检测点
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
