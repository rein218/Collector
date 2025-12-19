using UnityEngine;

public class ChelixAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public void DoFlip()
    {
        _animator.SetTrigger("Flip");
    }   
}
