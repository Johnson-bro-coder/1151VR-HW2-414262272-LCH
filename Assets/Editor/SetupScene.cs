using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetupScene
{
    [MenuItem("Tools/Setup Assignment Scene")]
    public static void Setup()
    {
        string scenePath = "Assets/Scenes/SampleScene.unity";
        var scene = EditorSceneManager.OpenScene(scenePath);

        // 1. 設定 Main Camera 為 2D 正交攝影機 (Orthographic)
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.orthographic = true;
            mainCam.orthographicSize = 4.5f;
            mainCam.transform.position = new Vector3(0, 0, -10f);
            mainCam.backgroundColor = new Color(0.18f, 0.22f, 0.28f); // 沉穩質感深藍灰背景
        }

        // 2. 載入角色 Sprite
        Sprite playerSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Player.png");

        // 3. 建立或更新 Player 物件
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            player = new GameObject("Player");
        }

        player.transform.position = new Vector3(-5f, -1.8f, 0f);
        player.transform.localScale = new Vector3(0.35f, 0.35f, 1f); // 適當縮放

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = player.AddComponent<SpriteRenderer>();
        }
        if (playerSprite != null)
        {
            sr.sprite = playerSprite;
            sr.sortingOrder = 10;
        }

        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm == null)
        {
            pm = player.AddComponent<PlayerMovement>();
        }

        // 設定路徑點 (起點 -> 往前走 -> 往上跳 -> 往下跳至終點)
        pm.pathPoints = new Vector3[]
        {
            new Vector3(-5f, -1.8f, 0f), // 起點
            new Vector3(-1.5f, -1.8f, 0f), // 往前走
            new Vector3(1.5f, 1.8f, 0f),   // 往上跳
            new Vector3(5f, -1.8f, 0f)    // 往下跳到終點
        };
        pm.moveSpeed = 3.5f;

        // 4. 建立地面與階梯平台 (使用預設 Sprite 或簡單長條，增加畫面真實感)
        CreateOrUpdateGround("Ground_Start", new Vector3(-3.5f, -3.2f, 0), new Vector3(6f, 1.2f, 1f), new Color(0.3f, 0.6f, 0.4f));
        CreateOrUpdateGround("Ground_HighPlatform", new Vector3(1.5f, 0.2f, 0), new Vector3(2.5f, 1f, 1f), new Color(0.4f, 0.7f, 0.5f));
        CreateOrUpdateGround("Ground_End", new Vector3(5f, -3.2f, 0), new Vector3(4f, 1.2f, 1f), new Color(0.3f, 0.6f, 0.4f));

        // 標記場景已變更並儲存
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("【成功】SampleScene 場景已自動配置完成！");
    }

    private static void CreateOrUpdateGround(string name, Vector3 pos, Vector3 scale, Color color)
    {
        GameObject obj = GameObject.Find(name);
        if (obj == null)
        {
            obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.name = name;
        }
        obj.transform.position = pos;
        obj.transform.localScale = scale;

        // 去掉 3D 陰影干擾，簡單設色
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null && renderer.sharedMaterial != null)
        {
            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = color;
            renderer.material = mat;
        }
    }
}
