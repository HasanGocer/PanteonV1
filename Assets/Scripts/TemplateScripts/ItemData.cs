using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoSingleton<ItemData>
{
    [System.Serializable]
    public class Field
    {
        public List<float> fields = new List<float>();
    }

    public Field field;
    public Field standart;
    public Field factor;
    public Field constant;
    public Field maxFactor;
    public Field fieldPrice;

    //oyun içerisinde kullandýðým bazý zamanla ilerleyen deðiþkenleri iþliyor

    public void AwakeID()
    {
        for (int i = 0; i < field.fields.Count; i++)
        {
            field.fields[i] = standart.fields[i] + (factor.fields[i] * constant.fields[i]);
            fieldPrice.fields[i] = fieldPrice.fields[i] * factor.fields[i];
        }

        for (int i = 0; i < field.fields.Count; i++)
        {
            if (factor.fields[i] > maxFactor.fields[i])
            {
                factor.fields[i] = maxFactor.fields[i];
                field.fields[i] = standart.fields[i] + (factor.fields[i] * constant.fields[i]);
                fieldPrice.fields[i] = fieldPrice.fields[i] / (factor.fields[i] - 1);
                fieldPrice.fields[i] = fieldPrice.fields[i] * factor.fields[i];
            }
        }

        StartFuncs();
    }

    private void StartFuncs()
    {
        MarketPanel.Instance.MarketPanelStart();
        GameManager.Instance.GameStatStart();
        InfoPanel.Instance.ButtonPlacement();
        InfoPanel.Instance.TextPlacement();

        if (Load.Instance.isReturn)
            GridSystem.Instance.ResumeGameGridPlacement();
        else
            GridSystem.Instance.NewGameGridPlacement();

        FirstTapMechanic.Instance.FTStart();
    }

    public void SetField(int fieldCount)
    {
        factor.fields[fieldCount]++;

        field.fields[fieldCount] = standart.fields[fieldCount] + (factor.fields[fieldCount] * constant.fields[fieldCount]);
        fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] / (factor.fields[fieldCount] - 1);
        fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] * factor.fields[fieldCount];

        if (factor.fields[fieldCount] > maxFactor.fields[fieldCount])
        {
            factor.fields[fieldCount] = maxFactor.fields[fieldCount];
            field.fields[fieldCount] = standart.fields[fieldCount] + (factor.fields[fieldCount] * constant.fields[fieldCount]);
            fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] / (factor.fields[fieldCount] - 1);
            fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] * factor.fields[fieldCount];
        }

        GameManager.Instance.FactorPlacementWrite(factor);
    }
    public void SetBackField(int fieldCount)
    {
        factor.fields[fieldCount]--;

        field.fields[fieldCount] = standart.fields[fieldCount] + (factor.fields[fieldCount] * constant.fields[fieldCount]);
        fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] / (factor.fields[fieldCount] + 1);
        fieldPrice.fields[fieldCount] = fieldPrice.fields[fieldCount] * factor.fields[fieldCount];

        GameManager.Instance.FactorPlacementWrite(factor);
    }

}
