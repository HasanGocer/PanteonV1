using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneySystem : MonoSingleton<MoneySystem>
{
    private int _billion = 1000000000, _million = 1000000, _thousand = 1000;
    [SerializeField] TMP_Text _PowerText, _moneyText;

    public void MoneyTextRevork(int plus)
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.money += plus;

        if (gameManager.money < _thousand)
        {
            _moneyText.text = gameManager.money.ToString();
        }
        else if (gameManager.money < _million)
        {
            int money = gameManager.money / 100;
            float floatMoney = (float)money / 10;
            _moneyText.text = floatMoney.ToString() + " K";
        }
        else if (gameManager.money < _billion)
        {
            int money = gameManager.money / 100;
            float floatMoney = (float)money / 10;
            _moneyText.text = floatMoney.ToString() + " M";
        }
        else
        {
            int money = gameManager.money / 100;
            float floatMoney = (float)money / 10;
            _moneyText.text = floatMoney.ToString() + " B";
        }

        gameManager.SetMoney();
    }

    public void PowerTextRevork(int plus)
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.power += plus;

        if (gameManager.power < _thousand)
        {
            _PowerText.text = gameManager.power.ToString();
        }
        else if (gameManager.power < _million)
        {
            int power = gameManager.power / 100;
            float floatPower = (float)power / 10;
            _PowerText.text = floatPower.ToString() + " K";
        }
        else if (gameManager.power < _billion)
        {
            int power = gameManager.power / 100;
            float floatPower = (float)power / 10;
            _PowerText.text = floatPower.ToString() + " M";
        }
        else
        {
            int power = gameManager.power / 100;
            float floatPower = (float)power / 10;
            _PowerText.text = floatPower.ToString() + " B";
        }

        gameManager.SetMoney();
    }

    public string NumberTextRevork(int number)
    {
        string numberString;

        if (number < _thousand)
        {
            numberString = number.ToString();
        }
        else if (number < _million)
        {
            int tempNumber = number / 100;
            float floatNumber = (float)tempNumber / 10;
            numberString = floatNumber.ToString() + " K";
        }
        else if (number < _billion)
        {
            int tempNumber = number / 100;
            float floatNumber = (float)tempNumber / 10;
            numberString = floatNumber.ToString() + " M";
        }
        else
        {
            int tempNumber = number / 100;
            float floatNumber = (float)tempNumber / 10;
            numberString = floatNumber.ToString() + " B";
        }
        return numberString;
    }
}
