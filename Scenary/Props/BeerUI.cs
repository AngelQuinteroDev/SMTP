using UnityEngine;
using TMPro; // Necesario para usar TextMeshPro

public class BeerUI : MonoBehaviour
{
    public TextMeshProUGUI beerText; // Referencia al texto de UI

    void Update()
    {
        beerText.text = BeerPickup.beerCount + "/" + BeerPickup.totalBeers;
    }
}
