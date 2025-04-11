using System.Collections;
using TMPro;
using UnityEngine;

public class PowerUpUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private IEnumerator SetPowerUpText(string str)
    {
        this.text.gameObject.SetActive(true);
        this.text.text = $"Obtained {str}";
        yield return new WaitForSeconds(3f);
        this.text.gameObject.SetActive(false);
    }
    
    private void Start()
    {
        this.text.gameObject.SetActive(false);
        ConsolidatedPowerups.instance.OnPowerUp.AddListener((str) =>
            StartCoroutine(SetPowerUpText(str)));
    }

}
