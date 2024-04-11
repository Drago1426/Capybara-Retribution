using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public static HealthScript Instance;

    [SerializeField]
    private int maxHealth = 3;
    private int _currentHealth;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadPlayerHealth();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        Debug.Log("Health: " + _currentHealth);
        SavePlayerHealth();

        if (_currentHealth <= 0)
        {
            Debug.Log("Player has died");
        }
    }

    private void SavePlayerHealth()
    {
        PlayerData data = new PlayerData {currentHealth = this._currentHealth};
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    private void LoadPlayerHealth()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            _currentHealth = data.currentHealth;
        }
        else
        {
            _currentHealth = maxHealth;
        }
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        SavePlayerHealth();
    }
}

