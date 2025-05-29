using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchController : MonoBehaviour
{
    public GameObject player;
    public GameObject textObject;
    public GameObject targetObject;
    public GameObject lightOn;

    public AudioSource Alarm;
    public AudioSource horde;
    public AudioSource music;

    public AudioClip clickSound;
    public AudioClip alarmSound;
    public AudioClip hordeSound;
    public AudioClip musicSound;
    private AudioSource audioSource;

    private MonoBehaviour scriptToEnable;
    public float activateDistance = 3f;

    private bool playerInRange = false;

    void Start()
    {
        if (textObject != null) textObject.SetActive(false);

        if (targetObject != null)
        {
            scriptToEnable = targetObject.GetComponent<MonoBehaviour>();
            if (scriptToEnable != null)
                scriptToEnable.enabled = false;
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null || textObject == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        playerInRange = distance <= activateDistance;
        textObject.SetActive(playerInRange);

        // รองรับคลิกเมาส์หรือแตะจอบนมือถือ
        if (Input.GetMouseButtonDown(0) && playerInRange)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(worldPoint.x, worldPoint.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                TryActivate();
            }
        }
    }

    private void TryActivate()
    {
        if (!playerInRange) return;

        if (scriptToEnable != null)
            scriptToEnable.enabled = true;

        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
        Alarm.PlayOneShot(alarmSound);
        horde.PlayOneShot(hordeSound);
        music.Play();

        if (lightOn != null)
            lightOn.SetActive(true);

        if (textObject != null)
            textObject.SetActive(false);

        Destroy(this);
    }
}
