using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Photon.Pun;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUPInputField();

    private void SetUPInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;

        SetPlayername(defaultName);
    }

    public void SetPlayername(string name)
    {
        //For some reason using just the player name doesn't work

        //Debug.Log(nameInputField.text);
        continueButton.interactable = (nameInputField.text.Length > 0) ? true : false;
        //continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void SavePlayerName() {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNameKey, playerName);
    }
}
