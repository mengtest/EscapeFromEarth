using UnityEngine;
using System.Collections;

public class GameEnvCreater : MonoBehaviour {
    public Forest forest1;//当前
    public Forest forest2;//下一个
    public GameObject[] forest;
    private int forestCount = 2;
    public void CreaterForest()
    {
        int type = Random.Range(0, 3);
        forestCount++;
        GameObject nextForest = GameObject.Instantiate(forest[type], new Vector3(0,0, forestCount*3000),Quaternion.identity) as GameObject;
        forest1 = forest2;
        forest2 = nextForest.GetComponent<Forest>();
    }
}
