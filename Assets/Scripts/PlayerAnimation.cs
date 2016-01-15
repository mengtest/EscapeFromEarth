using UnityEngine;
using System.Collections;
public enum GameAnimationState
{
    Run,
    Idel,
    Jump,
    Swap,
    TurnLeft,
    TurnRight,
    death
}
public class PlayerAnimation : MonoBehaviour {
    public Animation _animation;
    private PlayerMove _playerMove;
    public  GameAnimationState _animationState = GameAnimationState.Idel;
    public AudioSource Music_footStep;
	// Use this for initialization
	void Start () {
        _playerMove = GetComponent<PlayerMove>();

    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.state == GameState.Menul)
        {
            _animationState = GameAnimationState.Idel;
        }
        else if (GameController.state == GameState.Playing)
        {
            _animationState = GameAnimationState.Run;
            if (_playerMove.nowIndex<_playerMove.targetIndex)
            {
                _animationState = GameAnimationState.TurnRight;
            }
            if (_playerMove.nowIndex > _playerMove.targetIndex)
            {
                _animationState = GameAnimationState.TurnLeft;
            }
            if (_playerMove.isSlide)
            {
                _animationState = GameAnimationState.Swap;

            }
            if (_playerMove.isJump)
            {
                _animationState = GameAnimationState.Jump;
            }
        }
        else if(GameController.state == GameState.End)
        {
            _animationState = GameAnimationState.death;
            
        }
        //播放声音

        if (_animationState == GameAnimationState.Run)
        {
            if (!Music_footStep.isPlaying)
            {
                Music_footStep.Play();

            }
        }
        else if (Music_footStep.isPlaying)
        {
            Music_footStep.Stop();

        }

    }
    void LateUpdate()
    {
        switch (_animationState)
        {
            case GameAnimationState.Run:
                PlayAnima("run");
                break;
            case GameAnimationState.Idel:
                PlayIdel();
                break;
            case GameAnimationState.Jump:
                PlayAnima("jump");

                break;
            case GameAnimationState.death:
                PlayAnima("death");
                break;
            case GameAnimationState.TurnRight:
                //动画做的太差
                break;
            case GameAnimationState.Swap:
                PlayAnima("slide");
                break;
            
        }
    }
    private void PlayIdel()
    {
        if (!_animation.IsPlaying("Idle_1")&& !_animation.IsPlaying("Idle_2"))
        {
            _animation.Play("Idle_1");
            _animation.PlayQueued("Idle_2");//队列播放
        }
    }
    private bool havePlay=false;
    private void PlayAnima(string name)
    {
        if (name=="death"&&havePlay==false)
        {
            _animation.CrossFade(name, 0.3f);
            havePlay = true;
            return;
        }
        else if (!_animation.IsPlaying(name)&& (name != "death"))
        {
            _animation.CrossFade(name, 0.3f);
        }

    }

}
