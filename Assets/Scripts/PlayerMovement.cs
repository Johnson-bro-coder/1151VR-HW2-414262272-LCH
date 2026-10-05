using UnityEngine;

/// <summary>
/// 實作作業要求：
/// 1. 往前走、上跳並下跳到終點
/// 2. 務必使用 Vector 和 陣列 (Array)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 4f;

    [Header("路徑點陣列 (使用 Vector3 與 Array)")]
    [Tooltip("依序記錄：起點 -> 往前走 -> 往上跳 -> 往下跳至終點")]
    public Vector3[] pathPoints = new Vector3[]
    {
        new Vector3(-6f, -2f, 0f), // 索引 0: 起點
        new Vector3(-2f, -2f, 0f), // 索引 1: 往前走到平地
        new Vector3(1f, 2f, 0f),   // 索引 2: 往上跳躍點
        new Vector3(5f, -2f, 0f)   // 索引 3: 往下跳到達終點
    };

    // 當前目標在陣列中的索引位置
    private int currentIndex = 0;
    private bool isFinished = false;

    void Start()
    {
        // 確保角色從陣列的第一個位置 (起點) 開始
        if (pathPoints != null && pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
            currentIndex = 1; // 下一個目標為索引 1 (往前走)
        }
    }

    void Update()
    {
        // 如果還沒走到終點，持續往當前目標點移動
        if (!isFinished && pathPoints != null && currentIndex < pathPoints.Length)
        {
            MoveToTarget();
        }

        // 貼心功能：按下 R 鍵或空白鍵可以重頭播放移動演示（方便螢幕錄影）
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            ResetMovement();
        }
    }

    /// <summary>
    /// 使用 Vector3.MoveTowards 平滑移向當前陣列所指定的目標點
    /// </summary>
    void MoveToTarget()
    {
        Vector3 targetPos = pathPoints[currentIndex];

        // 核心：使用 Vector3 計算每一幀移動的位置
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // 檢查是否已抵達當前目標點 (距離小於 0.05f 即視為到達)
        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            Debug.Log($"抵達節點 [{currentIndex}]：{targetPos}");
            currentIndex++;

            // 判斷是否已經完成所有路徑點
            if (currentIndex >= pathPoints.Length)
            {
                isFinished = true;
                Debug.Log("【完成】角色已成功上跳並下跳抵達終點！");
            }
        }
    }

    /// <summary>
    /// 重置回起點，重新開始演示
    /// </summary>
    public void ResetMovement()
    {
        if (pathPoints != null && pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
            currentIndex = 1;
            isFinished = false;
            Debug.Log("重置回起點重新播放！");
        }
    }

    /// <summary>
    /// 在 Unity 編輯器 Scene 視窗畫出路徑輔助線與球體，方便可視化除錯
    /// </summary>
    void OnDrawGizmos()
    {
        if (pathPoints == null || pathPoints.Length == 0) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < pathPoints.Length; i++)
        {
            Gizmos.DrawSphere(pathPoints[i], 0.25f);
            if (i < pathPoints.Length - 1)
            {
                Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
            }
        }
    }
}
