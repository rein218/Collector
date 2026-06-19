using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Add this namespace

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMaskInteractable;

    Dictionary<string,bool> features = new Dictionary<string,bool>
    {
        {"coin_bronse", false},
        {"coin_silver", false},
        {"coin_gold", false},
    };

    private void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    private void OnEnable()
    {
        EventBus.OnStateChanged += UpdateCoinMask;
    }

    private void OnDisable()
    {
        EventBus.OnStateChanged -= UpdateCoinMask;
    }

    public void UpdateCoinMask()
    {
        if(GameManager.Instance.IsFeatureUnlocked("hovering_bronse")) features["coin_bronse"] = true;
        if(GameManager.Instance.IsFeatureUnlocked("hovering_silver")) features["coin_silver"] = true;
        if(GameManager.Instance.IsFeatureUnlocked("hovering_gold")) features["coin_gold"] = true;
    }



    private void Update()
    {
        HandleMouse();
        HandleTouch();
    }

    private void HandleMouse()
    {
        if (!IsPointerOverUI(Input.mousePosition))
        {
            TouchProcessing(Input.mousePosition);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (!IsPointerOverUI(touch.position))
            {
                TouchProcessing(touch.position);
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
            if (hit.collider.TryGetComponent<Coin>(out var coin))
            {
                if(Input.GetMouseButtonDown(0) || features[coin.id] == true)
                {
                    coin.Interact();
                }
            }
            else if (hit.collider.TryGetComponent<Chelix>(out var chelix))
            {
                chelix.SetNewGoal();
            }
            else if(hit.collider.TryGetComponent<AdThing>(out var AdThing))
            {
                if(Input.GetMouseButtonDown(0)) AdThing.Interact();
            }
        }
    }
}