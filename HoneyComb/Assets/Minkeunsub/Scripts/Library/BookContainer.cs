using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookContainer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image Focus;
    [SerializeField] Image Icon;
    [SerializeField] Text Name;
    [SerializeField] Text Desc;
    Book thisBook;
    bool isChoosing;
    BookChoose manager;

    void Start()
    {
        OnButtonExit();
    }

    void Update()
    {
        
    }

    public void Init(BookChoose _manager, Book book, Sprite icon)
    {
        manager = _manager;
        thisBook = book;
        Icon.sprite = icon;

        Name.text = thisBook.BookName;
        Desc.text = thisBook.BookDesc1;
    }

    public void OnButtonClick()
    {
        
    }

    public void OnButtonEnter()
    {
        isChoosing = true;
        Focus.gameObject.SetActive(true);
    }

    public void OnButtonExit()
    {
        isChoosing = false;
        Focus.gameObject.SetActive(true);
    }
}
