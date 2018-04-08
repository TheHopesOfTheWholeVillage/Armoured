using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class NotTrack : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES



    public GameObject Robot;   //机器人

    public GameObject AimObject; // 脱卡之后的模型位置的空物体

    public bool canMoveTo = false;  //是否脱卡

    public bool canChange = false;  //是否可以散开

    public bool hasSetPos = false;  //是否设置了部件该去的位置



    #endregion



    #region PRIVATE_MEMBER_VARIABLES



    private TrackableBehaviour mTrackableBehaviour;

    private bool hasFirstLoad = false; //是否是第一次识别到物体

    private Vector3 AimPos;  //脱卡之后的模型位置      

    private Transform robotTransform; //机器人Transform的引用

    private float timer = 0f;   //timer递减看机器人子物体的移动次数

    private int childCount;     //机器人的子物体个数

    private Vector3 centerPos;  //要移动到的位置

    private Transform[] childTransform; //机器人子物体的Transform

    private Vector3[] moveTo;   //移动的方向（没有单位化）

    private Vector3[] localPos; //机器人子物体的本地坐标，为了在分散开后归位时正常位置



    #endregion // PRIVATE_MEMBER_VARIABLES



    void Start()

    {

        //原始代码保留

        //添加的代码

        //初始化一堆变量

        robotTransform = Robot.transform;

        childCount = robotTransform.childCount;

        childTransform = new Transform[childCount];

        moveTo = new Vector3[childCount];

        localPos = new Vector3[childCount];

        for (int i = 0; i < childCount; i++)

        {

            childTransform[i] = robotTransform.GetChild(i);

            localPos[i] = childTransform[i].localPosition;

            //记录本地坐标位置，以便还原

        }

    }




  void Update()

    {

        if (canMoveTo)   //如果可以移动了 也就是脱卡后

        {

            robotTransform.position = Vector3.MoveTowards(robotTransform.position, AimPos, 1.0f);   //移动到相应位置             



            if (Vector3.Distance(robotTransform.position, AimPos) < 0.01f) //基本以已经到位置了 就可以单击散开

            {

                if (!hasSetPos)

                {

                    //获得机器人的子物体引用 并设置方向

                    centerPos = robotTransform.position;

                    for (int i = 0; i < childCount; i++)

                    {

                        moveTo[i] = centerPos - childTransform[i].position; //获得机器人的每个部件该去的位置的反方向（由于后面有个单击一次就反向一次的操作）

                    }

                    hasSetPos = true;

                }



                if (timer > 0f)    //如果能移动

                {

                    if (!canChange) //移动

                    {

                        for (int i = 0; i < childCount; i++)

                        {

                            childTransform[i].position += moveTo[i] * timer;

                            timer -= 0.08f;//循环次数

                        }

                    }

                }

                else

                {

                    canChange = true;  //移动到位置了 也即是timer<=0 就可以再移动

                }



                //单击散开或者合拢

                if (Input.GetMouseButtonDown(0))//按下左键 如果手机就改成单机

                {

                    if (timer <= 0) //如果能移动 就重置Timer=1

                    {

                        RaycastHit _hit;

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out _hit, 10f))

                        {

                            if (_hit.collider.gameObject.name == Robot.name)

                            {

                                timer = 1f;

                                canChange = false;



                                for (int i = 0; i < childCount; i++)

                                {

                                    moveTo[i] *= -1;  //向相反方向移动

                                }

                            }

                        }

                    }

                }



            }

        }



    }


 

private void OnTrackingFound()

    {

        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

        Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

        //将Robot的父物体设为ImgTarget 并调整

        robotTransform.SetParent(transform);

        robotTransform.localEulerAngles = new Vector3(0, 180, 0);

        robotTransform.transform.position = this.transform.position;



        for (int i = 0; i < childCount; i++)

        {

            childTransform[i].localPosition = localPos[i];

        }



        // Enable rendering:

        foreach (Renderer component in rendererComponents)

        {

            component.enabled = true;

        }



        // Enable colliders:

        foreach (Collider component in colliderComponents)

        {

            component.enabled = true;

        }

        canMoveTo = false;

        hasSetPos = false;

        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");

        hasFirstLoad = true;   //第一次找到了 渲染出来



    }





    private void OnTrackingLost()

    {

        if (!hasFirstLoad)

        {

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);

            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);



            // Disable rendering:

            foreach (Renderer component in rendererComponents)

            {

                component.enabled = false;

            }



            // Disable colliders:

            foreach (Collider component in colliderComponents)

            {

                component.enabled = false;

            }



            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

        }

        else

        {

            //在第一次渲染出图形以后的操作

            canMoveTo = true;

            AimPos = AimObject.transform.position;  //脱卡后的位置  

            robotTransform.SetParent(AimObject.transform);  //设置父物体为AimObject

            robotTransform.localEulerAngles = new Vector3(0, 180, 0);

        }

    }
}
