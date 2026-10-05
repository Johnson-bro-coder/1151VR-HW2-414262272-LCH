# 1151VR-HW2 - 2D 角色路徑移動與跳躍作業

本專案為輔仁大學虛擬實境（VR）課程作業 2：使用 Unity 實作 2D 人物載入，並透過 **C# 陣列 (Array)** 與 **Vector** 達成角色「向前走、向上跳、向下跳到達終點」的自動巡航與演示系統。

---

## 📌 繳交資訊表

| 項目 | 內容 |
| :--- | :--- |
| **作業名稱** | 1151VR-HW2-2D角色移動與跳躍 |
| **學生學號** | `[請在此填入學號，例如：410XXXXXX]` |
| **學生姓名** | `[請在此填入姓名，例如：王小明]` |
| **GitHub 專案連結** | [GitHub 專案庫網址](https://github.com/Johnson-bro-coder/1151VR-HW2-學號-姓名) |
| **YouTube 演示影片** | [YouTube 影片連結](https://youtube.com/watch?v=YOUR_VIDEO_ID) |

---

## 📸 專案執行截圖

### 1. 2D 角色與遊戲場景配置
![遊戲畫面截圖](Assets/Sprites/Player.png)

> **說明**：
> - 畫面中包含綠色起點平台、高台跳躍區與終點平台。
> - 角色會在啟動後沿著預設的路徑點依序前進與跳躍。

---

## 🎯 作業目標與核心技術

1. **2D 專案與角色載入**：在 Unity 中配置 2D 正交攝影機（Orthographic Camera），載入 2D Sprite 人物圖片並配置 SpriteRenderer。
2. **Vector 與陣列功能實作**：
   - 宣告 `Vector3[] pathPoints` 儲存多個關鍵位移座標（起點 $\to$ 平地前進 $\to$ 向上起跳高點 $\to$ 向下跳落至終點）。
   - 利用 `Vector3.MoveTowards()` 與 `Time.deltaTime` 進行幀率無關的平滑位移。
   - 使用 `Vector3.Distance()` 判定是否抵達當前節點並自動推進索引 `currentIndex++`。
3. **輔助與演示功能**：
   - 支援按下鍵盤 <kbd>R</kbd> 或 <kbd>Space</kbd> 隨時重新回到起點播放演示（方便錄影與檢查）。
   - 內建 `OnDrawGizmos()` 在 Scene 視窗即時繪製路徑線段與節點球體。

---

## 🛠️ 製作流程與新手操作指引

### 第一步：載入 2D 人物圖片
1. 將角色圖片放置於專案資料夾 `Assets/Sprites/Player.png`。
2. 點選圖片，在 Inspector 檢視面板確認：
   - **Texture Type**：設定為 `Sprite (2D and UI)`
   - **Sprite Mode**：設定為 `Single`
   - 點擊下方 **Apply** 套用。

### 第二步：編寫核心移動腳本 (`PlayerMovement.cs`)
於 `Assets/Scripts/PlayerMovement.cs` 撰寫如下程式碼：

```csharp
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 3.5f;

    [Header("路徑點陣列 (使用 Vector3 與 Array)")]
    public Vector3[] pathPoints = new Vector3[]
    {
        new Vector3(-5f, -1.8f, 0f), // 索引 0: 起點
        new Vector3(-1.5f, -1.8f, 0f), // 索引 1: 往前走
        new Vector3(1.5f, 1.8f, 0f),   // 索引 2: 往上跳
        new Vector3(5f, -1.8f, 0f)    // 索引 3: 往下跳到達終點
    };

    private int currentIndex = 0;
    private bool isFinished = false;

    void Start()
    {
        if (pathPoints != null && pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
            currentIndex = 1;
        }
    }

    void Update()
    {
        if (!isFinished && pathPoints != null && currentIndex < pathPoints.Length)
        {
            MoveToTarget();
        }

        // 按鍵重置播放
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            ResetMovement();
        }
    }

    void MoveToTarget()
    {
        Vector3 targetPos = pathPoints[currentIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= pathPoints.Length)
            {
                isFinished = true;
                Debug.Log("【完成】角色已抵達終點！");
            }
        }
    }

    public void ResetMovement()
    {
        if (pathPoints != null && pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
            currentIndex = 1;
            isFinished = false;
        }
    }
}
```

### 第三步：場景與角色物件綁定
1. 在 Hierarchy 面板右鍵建立空的 GameObject 並命名為 `Player`。
2. 為 `Player` 掛上 `Sprite Renderer` 元件，將 `Player.png` 拖入 Sprite 欄位。
3. 為 `Player` 掛上 `PlayerMovement` 腳本元件。
4. 在 Inspector 面板可自由調整 `Path Points` 各節點的 X、Y、Z 數值及移動速度 `Move Speed`。

### 第四步：執行與測試
1. 點擊 Unity 頂部的 **Play (播放按鈕 ▶)**。
2. 角色將自動由左側平移前進、往右上跳躍、接著向下落至終點。
3. 測試期間可隨時按下鍵盤 <kbd>R</kbd> 或 <kbd>Space</kbd> 重新播放路徑動畫。

---

## 📦 GitHub 繳交與推播步驟

1. **確認遠端儲存庫名稱**（依作業規範）：
   ```bash
   git remote set-url origin https://github.com/Johnson-bro-coder/1151VR-HW2-學號-姓名.git
   ```
2. **加入變更並提交**：
   ```bash
   git add .
   git commit -m "feat: 完成 HW2 2D 角色 Vector 陣列路徑移動與跳躍功能"
   git push -u origin main
   ```
3. **設定 GitHub Repository**：
   - 進入 GitHub 專案頁面的 **Settings** $\to$ **General** $\to$ **Danger Zone** $\to$ 確認設為 **Public**。
   - 進入 **Settings** $\to$ **Collaborators** $\to$ 點擊 **Add people** $\to$ 輸入助教信箱 `cchu.fju@gmail.com` 發送協作者邀請。
