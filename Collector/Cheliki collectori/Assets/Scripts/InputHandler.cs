using UnityEngine;
using UnityEngine.EventSystems; // Add this namespace

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMaskInteractable;
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
            if (!IsPointerOverUI(Input.mousePosition))
            {
                TouchProcessing(Input.mousePosition);
            }
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || !clickIsRequired)
            {
                if (!IsPointerOverUI(touch.position))
                {
                    TouchProcessing(touch.position);
                }
            }
        }
    }

    private bool IsPointerOverUI(Vector2 position)
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData eventDataCurrentPosition = new(EventSystem.current) { position = position };

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        return results.Count > 0;
    }

    private void TouchProcessing(Vector2 touchPosition)
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(touchPosition);
            
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMaskInteractable);
        
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