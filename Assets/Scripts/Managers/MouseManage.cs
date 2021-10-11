using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MouseManage : MonoBehaviour
{
    public static MouseManage Instance;  //MouseManage 单例模式实例
    RaycastHit hitInfo;  //射线碰撞信息
    public event Action<Vector3> OnMouseClicked;  //鼠标点击事件
    public event Action<GameObject> OnEnemyClicked;  //点击敌人
    public Texture2D Arrow, Attack, Doorway, Point;  //鼠标贴图

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    private void Update()
    {
        SetCursorTextrue();
        MouseControl();
    }


    //设置鼠标图片
    void SetCursorTextrue()
    {
        //从摄像机发射一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            //切换鼠标贴图
            switch (hitInfo.collider.tag)
            {
                case "Ground":
                    Cursor.SetCursor(Arrow, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(Attack, new Vector2(0, 0), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                OnMouseClicked?.Invoke(hitInfo.point);
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
        }
    }
}