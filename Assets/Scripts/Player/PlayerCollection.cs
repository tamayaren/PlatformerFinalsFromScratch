using TMPro;
using Unity.UI;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollection : MonoBehaviour
{
    public TMP_Text itemcounter;
    public TMP_Text healthcounter;
    public Image paused;
    
    public int currentHealth;
    public int maxHealth = 5;
    
    public int currentItems;
    public string[] items;
    public int maxItems = 5;
    
    
    void Start()
    {
        
    }
    
    void Update()
    {
        itemcounter.text = "collected:" + currentItems.ToString() + "/" + maxItems.ToString();
        healthcounter.text = "health:" + currentHealth.ToString() + "/" + maxHealth.ToString();
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            paused.gameObject.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            paused.gameObject.SetActive(true);
        }
    }
}
