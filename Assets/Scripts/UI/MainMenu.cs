using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header ("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    private void Awake()
    {
        mainPanel.SetActive (true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    #region Settings
    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f);
    }
    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f);
    }

    public void Return()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
    #endregion
}
