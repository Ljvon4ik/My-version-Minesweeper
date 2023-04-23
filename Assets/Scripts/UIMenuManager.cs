using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _startMenu;

    public void CloseStartMenu()
    {
        _startMenu.SetActive(false);
    }
}
