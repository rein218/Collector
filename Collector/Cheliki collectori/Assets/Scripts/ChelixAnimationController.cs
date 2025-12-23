using UnityEngine;

public class ChelixAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sprite;

    public void DoFlip()
    {
        _animator.SetTrigger("Flip");
        _sprite.flipX = false;
    }  

    public void MoveTopLeft()
    {
        _animator.SetBool("GoesForward", true);
        _sprite.flipX = false;
    } 

    public void MoveTopRight()
    {
        _animator.SetBool("GoesForward", true);
        _sprite.flipX = true;
    }  

    public void MoveBottomLeft()
    {
        _animator.SetBool("GoesBack", true);
        _sprite.flipX = false;
    } 

    public void MoveBottomRight()
    {
        _animator.SetBool("GoesBack", true);
        _sprite.flipX = true;
    }  

    public void StopMoving()
    {
        _animator.SetBool("GoesForward", false);
        _animator.SetBool("GoesBack", false);
        _sprite.flipX = false;
    }
}
