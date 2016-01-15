using UnityEngine;
using System.Collections;
using System;
public enum TouchDirection
{
    None,
    Left,
    Right,
    Top,
    Bottom,
}
public class PlayerMove : MonoBehaviour {
    private GameEnvCreater gameEvnCreater;
    private TouchDirection touchDir = TouchDirection.None;
    private Vector3 LastMouseDown = Vector3.zero;
    public int nowIndex=1;//当前位置
    public int targetIndex = 1;//目标位置
    private float moveDistance=0;//移动距离
    public float moveSpeed = 12;
    private float[] xOffSets = new float[3] {-13,0,13 };

    public bool isSlide = false;//当前有没有滑动
    public float SlideTime = 1.4f;
    private float SlideTimer = 0;//滑动计时器

    public Transform prisioner;
    public bool isJump = false;
    public bool isUp;
    public float JumpHeight = 20;
    private float JumpSpeed=60;
    private float HaveJumpHeight = 0;

    public AudioSource JumpLandMusic;
    // Use this for initialization
    void Start () {
        gameEvnCreater = Camera.main.GetComponent<GameEnvCreater>();
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.state==GameState.Playing)
        {
            Vector3 targetPos = gameEvnCreater.forest1.GetTargetPos();//得到目标位置
            targetPos = new Vector3(targetPos.x + xOffSets[nowIndex], targetPos.y, targetPos.z);//！！移动目标要加上左右偏移
            Vector3 moveDirection = targetPos - transform.position;
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;//设置目标点的位置

            #region Move
            if (targetIndex != nowIndex)
            {
                float moveLength = moveDistance * 0.2f;
                moveDistance -= moveLength;//更新剩余距离
                transform.position = new Vector3(transform.position.x + moveLength, transform.position.y, transform.position.z);
                Debug.Log("Distance" + moveDistance);
                if (Mathf.Abs(moveDistance) <= 0.5f)
                {
                    //完成左右移动
                    //switch (targetIndex)
                    //{
                    //    case 0:
                    //        transform.position = new Vector3(-35, transform.position.y, transform.position.z);
                    //        break;
                    //    case 1:
                    //        transform.position = new Vector3(-23, transform.position.y, transform.position.z);

                    //        break;
                    //    case 2:
                    //        transform.position = new Vector3(-10, transform.position.y, transform.position.z);

                    //        break;
                    //}
                    this.transform.position = new Vector3(transform.position.x + moveDistance, transform.position.y, transform.position.z);
                    nowIndex = targetIndex;
                    moveDistance = 0;
                }
            }
            if (isSlide)
            {
                SlideTimer += Time.deltaTime;
                if (SlideTimer > SlideTime)
                {
                    SlideTimer = 0;
                    isSlide = false;
                    //滑动完成
                }
            }
            if (isJump)
            {
                float yMove = JumpSpeed * Time.deltaTime;//每frame移动距离

                //8.4 是原来角色的y
                if (isUp)
                {
                    prisioner.position = new Vector3(prisioner.position.x, prisioner.position.y + yMove, prisioner.position.z);
                    HaveJumpHeight += yMove;
                    if (Mathf.Abs(HaveJumpHeight) >= JumpHeight)
                    {
                        prisioner.position = new Vector3(prisioner.position.x, 28.4f, prisioner.position.z);
                        HaveJumpHeight = JumpHeight;
                        isUp = false;
                    }
                }
                else
                {
                    prisioner.position = new Vector3(prisioner.position.x, prisioner.position.y - yMove, prisioner.position.z);
                    HaveJumpHeight -= yMove;
                    if (HaveJumpHeight < 0.05f)
                    {
                        prisioner.position = new Vector3(prisioner.position.x, 8.4f, prisioner.position.z);
                        HaveJumpHeight = 0;
                        isJump = false;
                        //角色落地
                        JumpLandMusic.Play();
                    }
                }
            }
            #endregion
            MoveControl();
            
        }
        
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 200, 300, 300), "逃跑距离："+(int)(transform.position.z-115));
    }
    private void MoveControl()
    {
        TouchDirection dir = GetTouchDir();
        switch (dir)
        {
            case TouchDirection.None:
                break;
            case TouchDirection.Right:
                if (targetIndex < 2)
                {
                    targetIndex++;
                    moveDistance = 13;
                }
                break;
            case TouchDirection.Left:
                if (targetIndex > 0)
                {
                    targetIndex--;
                    moveDistance = -13;
                }
                break;
            case TouchDirection.Top:
                if (isJump == false)
                {
                    isJump = true;
                    isUp = true;
                    HaveJumpHeight = 0;
                }
                break;
            case TouchDirection.Bottom:
                if (!isJump)
                {
                    isSlide = true;
                    SlideTimer = 0;//清空计时器
                }
                break;
        }
    }
    /// <summary>
    /// 获取移动方向
    /// </summary>
    /// <returns></returns>
    private TouchDirection GetTouchDir()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LastMouseDown = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 mouseUp = Input.mousePosition;
            Vector3 touchOffSet = mouseUp - LastMouseDown;

            //偏移大于50为有效输出
            if (Mathf.Abs(touchOffSet.x) > 50 ||  Mathf.Abs(touchOffSet.y) > 50||  Mathf.Abs(touchOffSet.z) > 50)
            {
                
                if (Mathf.Abs(touchOffSet.x)>Mathf.Abs(touchOffSet.y)&&touchOffSet.x>0)
                {
                    //if (targetIndex<2)
                    //{
                    //    //向右移动
                    //    targetIndex++;
                    //    moveDistance = 13;//设置移动距离
                    //}
                    return TouchDirection.Right;

                }
                if (Mathf.Abs(touchOffSet.x) > Mathf.Abs(touchOffSet.y) && touchOffSet.x < 0)
                {
                    //if (targetIndex > 0)
                    //{
                    //    //向右移动
                    //    targetIndex--;
                    //    moveDistance = -13;//设置移动距离
                    //}
                    return TouchDirection.Left;

                }
                if (Mathf.Abs(touchOffSet.x) < Mathf.Abs(touchOffSet.y) && touchOffSet.y < 0)
                {
                    //isSlide = true;
                    //SlideTimer = 0;//清空计时器
                    return TouchDirection.Bottom;
                }
                if (Mathf.Abs(touchOffSet.x) < Mathf.Abs(touchOffSet.y) && touchOffSet.y > 0)
                {
                    //if (isJump==false&&isSlide==false)
                    //{
                    //    isJump = true;
                    //    isUp = true;
                    //    HaveJumpHeight = 0;

                    //}
                    return TouchDirection.Top;

                }
            }
            
        }
        return TouchDirection.None;
    }
}
