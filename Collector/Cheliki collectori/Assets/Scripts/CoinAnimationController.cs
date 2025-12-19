using UnityEngine;

public class CoinAnimationControllers : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public void StartRotation()
    {
        _animator.SetBool("Rotation", true);
    }  

    public void EndRotation()
    {
        _animator.SetBool("Rotation", false);
    }  
    public void ChangeSpeed(float newSpeed)
    {
        _animator.SetFloat("RotationSpeed", newSpeed);
    }

}
