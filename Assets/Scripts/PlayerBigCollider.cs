using UnityEngine;
using System.Collections;

public class PlayerBigCollider : MonoBehaviour {
    public PlayerAnimation _animation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag==Tags.obs&&GameController.state==GameState.Playing&&_animation._animationState!=GameAnimationState.Swap)
        {
            GameController.state = GameState.End;
        }
    }
}
