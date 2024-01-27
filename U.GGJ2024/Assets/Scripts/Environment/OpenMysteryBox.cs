using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMysteryBox : MonoBehaviour
{
    public ListOfPrankItems prankItems;
    public GameObject chosenItem;

    private void OnEnable()
    {
        ChooseARandomItemFromTheList();
    }

    private void ChooseARandomItemFromTheList()
    {
        try
        {
            int itemIndex = Random.Range(0, prankItems.items.Count);
            chosenItem = prankItems.items[itemIndex];
        }
        catch (System.Exception)
        {
            Debug.Log("No Item from the List");
        }
    }

    public void OpenBox()
    {
        if (chosenItem != null)
        {
            Instantiate(chosenItem, transform.position, Quaternion.identity);
            chosenItem = null;
            gameObject.SetActive(false);
        }
    }
}
