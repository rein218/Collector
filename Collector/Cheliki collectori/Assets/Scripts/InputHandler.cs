using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] CoinCounter coinCounter;

    private void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    private void Update()
    {
        HandleClick();
    }

    private void HandleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
            
            if (hit.collider != null)
            {
                Debug.Log("Clicked on: " + hit.collider.name); 

                if (hit.collider.CompareTag("Coin"))
                {
                    Coin clickedCoin = hit.collider.GetComponent<Coin>();

                    coinCounter.AddCoins(clickedCoin.ValueOfCoin);

                    Destroy(clickedCoin.gameObject);
                }
            }
    
        }
    }
}