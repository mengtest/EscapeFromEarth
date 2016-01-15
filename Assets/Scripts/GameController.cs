using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menul,
    Playing,
    End
}
public class GameController : MonoBehaviour {
    public static GameState state=GameState.Menul;
    public GameObject TapToStart;
    public GameObject GameOver;
    public Vector3 DVecctor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp( transform.position,DVecctor + GameObject.FindGameObjectWithTag(Tags.player).transform.position,Time.deltaTime*1000);
        if (state==GameState.Menul)
        {
            GameOver.SetActive(false);

            if (Input.GetMouseButton(0))
            {
                TapToStart.SetActive(false);
                state = GameState.Playing;
            }
        }
        if (state==GameState.End)
        {
            GameOver.SetActive(true);
            if (Input.GetMouseButton(0))
            {
                Application.LoadLevel(0);
                state = GameState.Menul;

            }
        }
	}
    
}
