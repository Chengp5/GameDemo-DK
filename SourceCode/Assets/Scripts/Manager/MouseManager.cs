using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
    
public class MouseManager : SingleTon<MouseManager>
{
    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked;
    RaycastHit mouseHitInfo;


    public Texture2D point, portal, target, arrow, attack;
    private cursorType currentCursor;

    private enum cursorType
    {
        point,
        portal,
        target,
        arrow,
        attack
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        currentCursor =cursorType.target;

    }
    
   

    // Update is called once per frame
    void Update()
    {
        setCursorTexture();
        mouseControl();
    }
    void setCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray,out mouseHitInfo))
        {
            //switch mouse cursor
            switch(mouseHitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    {
                        if (currentCursor != cursorType.target)
                        {
                            Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                            currentCursor = cursorType.target;
                        } 
                        break;
                    }
                case "Enemy":
                    {
                        if (currentCursor != cursorType.attack)
                        {
                            Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                            currentCursor = cursorType.attack;
                        }
                        break;
                    }
                case "Portal":
                    {
                        if (currentCursor != cursorType.portal)
                        {
                            Cursor.SetCursor(portal, new Vector2(16, 16), CursorMode.Auto);
                            currentCursor = cursorType.portal;
                        }
                        break;
                    }
                default:
                    if (currentCursor != cursorType.arrow)
                    {
                        Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                        currentCursor = cursorType.arrow;
                    }
                    break;
            }
        }
    }
    void mouseControl()
    {
        
        if (Input.GetMouseButtonUp(0)&&mouseHitInfo.collider!=null)
        {
           
            if (mouseHitInfo.collider.CompareTag("Ground"))
            {
              
                OnMouseClicked?.Invoke(mouseHitInfo.point);
            }
            if (mouseHitInfo.collider.CompareTag("Enemy"))
            {
                //Debug.Log("click enemy");
                OnEnemyClicked?.Invoke(mouseHitInfo.collider.gameObject);
            }
            if (mouseHitInfo.collider.CompareTag("Attackable"))
            {
                //Debug.Log("click enemy");
                OnEnemyClicked?.Invoke(mouseHitInfo.collider.gameObject);
            }
            if (mouseHitInfo.collider.CompareTag("Portal"))
            {
                //Debug.Log("click enemy");
                OnMouseClicked?.Invoke(mouseHitInfo.point);
            }

        }

    }
}
