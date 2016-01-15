using UnityEngine;
using System.Collections;

public class Forest : MonoBehaviour {
    private Transform player;
    public GameObject[] Obs;
    public int StartLength;//多少米处开始生成
    private GameEnvCreater gameEnvCreater;
    public float rangeMin;
    public float rangeMax;
    private WayPoints waypoints;
    private int TargetPos;

	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        waypoints = transform.Find("WayPoints").GetComponent<WayPoints>();
        CreateObs();
        TargetPos = waypoints.points.Length - 2;//初始化下一个点
       

        gameEnvCreater = Camera.main.GetComponent<GameEnvCreater>();
    }
	
	// Update is called once per frame
	void Update () {
        //if (player.position.z>transform.position.z+200)
        //{
        //    Camera.main.GetComponent<GameEnvCreater>().CreaterForest();
        //    GameObject.Destroy(gameObject);
        //}
	}
    public void CreateObs()
    {
        float starZ = transform.position.z - 3000;
        float endZ = transform.position.z;

        float z = starZ + StartLength;
        while (true)
        {
            z += Random.Range(rangeMin,rangeMax);
            if (z>endZ)
            {
                break;
            }
            else
            {
                Vector3 posInLine = GetPositonByZ(z);
                //创建障碍物
                int obsIndex = Random.Range(0, Obs.Length);
                GameObject go= Instantiate(Obs[obsIndex], posInLine, Quaternion.identity)as GameObject;
                go.transform.parent = transform;
            }
        }
    }

    /// <summary>
    /// 取得路径上的点
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    Vector3 GetPositonByZ(float z)
    {
        Transform[] points = waypoints.points;
        int index = 0;//当前点的索引
        for (int i = 0; i < waypoints.points.Length-1; i++)
        {

            if (z<=waypoints.points[i].position.z&& z >= waypoints.points[i+1].position.z)
            {
                //判断在哪两个点之间
                index = i;
                break;
            }
        }
        //index+1 <z< index 
        //由于数组是反的
        //index+1是前一个点，index是后一个点
        return  Vector3.Lerp(points[index + 1].position, points[index].position,(z-points[index+1].position.z)/(points[index].position.z-points[index+1].position.z));
        //根据后面的比值计算前面两个向量之间上对应的向量
    }
    /// <summary>
    /// 得到下一个点的坐标
    /// </summary>
    /// <returns></returns>
    public Vector3 GetTargetPos()
    {
        while (true)
        {
            if ((waypoints.points[TargetPos].position.z-player.position.z)<=100)
            {
                //当目标点和角色距离小于10时，就更新目标点
                TargetPos--;
                if (TargetPos<0)
                {
                    //到达下一张地图
                    gameEnvCreater.CreaterForest();
                    Destroy(gameObject, 1.5f);
                    return gameEnvCreater.forest1.GetTargetPos();//经过转换，forset为下一个森林
                }
            }
            else
            {
                return waypoints.points[TargetPos].position;
            }
        }
    }
}
