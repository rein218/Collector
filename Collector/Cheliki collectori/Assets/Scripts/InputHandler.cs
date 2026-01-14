using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] CurrenciesWallet currenciesWallet;
    [SerializeField] private bool clickIsRequired = true;

    private void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        HandleMouse();
        HandleTouch();
    }

    private void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0) || !clickIsRequired)
        {
            TouchProcessing(Input.mousePosition);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began  || !clickIsRequired)
            {
                TouchProcessing(touch.position);
            }
        }
    }

    private void TouchProcessing(Vector2 touchPosition)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(touchPosition);
            
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
        
        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Coin"))
            {
                Coin clickedCoin = hit.collider.GetComponent<Coin>();
                clickedCoin.Interact();
            }
            else if (hit.collider.CompareTag("Chelix"))
            {
                Chelix clickedChelix = hit.collider.GetComponent<Chelix>();
                clickedChelix.SetNewGoal();
            }
        }
    }

    public void SetClickNotRequired()
    {
        clickIsRequired = false;
    }
}