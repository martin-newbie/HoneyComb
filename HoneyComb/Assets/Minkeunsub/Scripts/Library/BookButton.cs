using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image Icon;
    [SerializeField] Text Name;
    [SerializeField] Text Desc;
    Book thisBook;
    BookChoose manager;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void Init(BookChoose _manager, Book book)
    {
        manager = _manager;
        thisBook = book;

        thisBook.Init();

        Icon.sprite = thisBook.Icon;
        Name.text = thisBook.BookName;
        Desc.text = thisBook.BookDesc1;
    }

    public void OnButtonClick()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        manager.OpenDetail(thisBook);
    }
}
