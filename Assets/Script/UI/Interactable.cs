using UnityEngine;
using UnityEngine.SceneManagement; //  ”À√—∫‚À≈¥©“°

public class ShopInteraction : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject mapUI;
    [SerializeField] private Transform player;
    [SerializeField] private float maxInteractionDistance = 2.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isNearPlayer = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        float distance = Vector2.Distance(player.position, transform.position);
        isNearPlayer = distance <= maxInteractionDistance;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = isNearPlayer
                ? Color.Lerp(originalColor, Color.white, 1f)
                : originalColor;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryInteract(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            TryInteract(Input.GetTouch(0).position);
        }
    }

    private void TryInteract(Vector2 screenPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null && isNearPlayer)
        {
            GameObject target = hit.collider.gameObject;

            if (target.CompareTag("Shop"))
            {
                if (shopUI != null)
                    shopUI.SetActive(true);
            }
            else if (target.CompareTag("Play"))
            {
                if (mapUI != null)
                    mapUI.SetActive(true);
                
            }
            else if (target.CompareTag("Tutorial"))
            {

                if (tutorialUI != null)
                    tutorialUI.SetActive(true);
            }
        }
        }
    }
    

