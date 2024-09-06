using UnityEngine;
using Vices.Scripts;

public class TestShopInteractable : Interactable
{
    [SerializeField] private GameObject _shopImage;

    protected override void OnEnter(Collider other) { }

    protected override void OnInteract()
    {
        _shopImage.SetActive(true);
    }
}
