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
        if (clickIsRequired) HandleClick();
        else MouseProcessing();
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MouseProcessing();
        }
    }

    private void MouseProcessing()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
        
        if (hit.collider != null)
        {
            Debug.Log("Clicked on: " + hit.collider.name); 

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