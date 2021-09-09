using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI txtMessage;
    public TextMeshProUGUI txtStepsLeft;
    public TextMeshProUGUI txtHealth;
    public Button btnAttack;
    public MouseController _mouseController;

    private UIController() { }

    public static UIController _instance;

    private void Awake()
    {
        _instance = this;
    }

    public  void SetTxtMessage(string message)
    {
        txtMessage.text = message;
    }
    public void SetStepsLeftMessage(string message)
    {
        txtStepsLeft.text = message;
    }

    public void ClearStepsLeftMessage()
    {
        txtStepsLeft.text = "";
    }

    public void ClearHealthMessage()
    {
        txtHealth.text = "";
    }

    

    public void SetTxtMessagePlayerToMove()
    {
        txtMessage.text = "Select the Player to Move";
    }

    public void OnClickBtnAttack()
    {
        txtMessage.text = "Select a Target To Attack";
        _mouseController.ChangeMouseState(MouseController.MouseState.Attack);
    }

    public void SetCurrentHealth(int currentHealth)
    {
        txtHealth.text = currentHealth.ToString();
    }

}
