using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Change the behavior of UI Elements
/// </summary>
public class UIController : MonoBehaviour
{
    public TextMeshProUGUI txtMessage, txtStepsLeft, txtHealth;
    public Image imgPlayer, imgBoot, imgHeart;
    public Button btnAttack, btnDefense;
    public MouseController _mouseController;
    
    public static UIController _instance;

    private void Awake()
    {
        _instance = this;
        OnClickClearHUD();
    }

    public void SetTxtMessage(string message)
    {
        txtMessage.text = message;
    }

    public void SetStepsLeftMessage(string message)
    {
        txtStepsLeft.text = message;
    }

    public void OnClickBtnAttack()
    {
        SetTxtMessage("Select a Target To Attack");
        _mouseController.ChangeMouseState(MouseController.MouseState.Attack);
    }


    public void OnClickBtnBusy()
    {

    }

    public void SetImgPlayer(Sprite img)
    {
        imgPlayer.sprite = img;
    }

    public void OnClickClearHUD()
    {
        SetTxtMessage("Select the Player to Move");
        txtStepsLeft.text = "";
        txtHealth.text = "";
        imgPlayer.sprite = null;
        ShowHideUiImage(false);
    }

    public void OnClickCharacter(int currentHealth, string playerName)
    {
        SetTxtMessage(playerName + " Selected");
        txtHealth.text = currentHealth.ToString();
        ShowHideUiImage(true);
    }

    public void OnClickDefenseCharacter(string playerName, Sprite charImage)
    {
        SetTxtMessage(playerName + " is in defense mode please select another one");
        SetImgPlayer(charImage);
    }

    void ShowHideUiImage(bool show)
    {
        imgBoot.gameObject.SetActive(show);
        imgHeart.gameObject.SetActive(show);
        btnAttack.gameObject.SetActive(show);
        btnDefense.gameObject.SetActive(show);
    }

}
