using UnityEngine;
using System.Collections;

public class PoliceCar : MonoBehaviour {
    public Vector3 DIRVector;
    private bool hasPlay = false;
    private bool startEngine=false;//开始追捕
    public AudioSource _triseMusic;//刹车声音
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.state==GameState.Playing&&startEngine==false)
        {
            startEngine = true;
            return;
        }
        if (startEngine)
        {
            transform.position = Vector3.Lerp(transform.position, DIRVector + GameObject.FindGameObjectWithTag(Tags.player).transform.position, Time.deltaTime * 2);

        }
        if (GameController.state==GameState.End&&hasPlay==false)
        {
            hasPlay = true;
            _triseMusic.Play();
        }
    }
}
