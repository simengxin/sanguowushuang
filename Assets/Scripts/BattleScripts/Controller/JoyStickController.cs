using UnityEngine;
using System.Collections;

public class JoyStickController : MonoBehaviour 
{

    private bool isPress = false;
    private Transform button;

    //从虚拟摇杆的得到的x，y偏移值-1到1之间
    public static float h = 0;
    public static float v = 0;
    void Awake()
    {
        button = this.transform;
    }
    void OnPress(bool isPress)
    {
        this.isPress = isPress;
        if (!isPress)
        {
            button.localPosition = Vector2.zero;
            h = 0;
            v = 0;
        }
    }

    void Update()
    {
        if (isPress)
        {
            Vector2 touchPos = UICamera.lastTouchPosition - new Vector2(91, 91);
            float distance = Vector2.Distance(Vector2.zero, touchPos);
            if (distance > 73)//虚拟摇杆按钮不能超过半径
            {
                touchPos = touchPos.normalized * 73;
            }            
            button.localPosition = touchPos;

            h = touchPos.x / 73;
            v = touchPos.y / 73;
        }
    }
}