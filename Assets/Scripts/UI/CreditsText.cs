using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float scrollSpeed = 80f;
    public float endY = 1250f;
    public RectTransform creditsText;

    private Vector3 startPos;

    [Header("UI Panels")]
    public GameObject mainPanel;
    public GameObject creditsPanel;

    private void OnEnable()
    {
        // Reset position every time it's re-activated
        if (creditsText != null)
            creditsText.localPosition = startPos;
    }

    private void Awake()
    {
        // Store the initial position of the text
        if (creditsText != null)
            startPos = creditsText.localPosition;
    }

    private void Update()
    {
        // Scroll the text upward
        creditsText.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;

        // Check for Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }

        // Auto-return when credits reach the end
        if (creditsText.localPosition.y >= endY)
        {
            ReturnToMainMenu();
        }
    }

    public void ReturnToMainMenu()
    {
        creditsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
