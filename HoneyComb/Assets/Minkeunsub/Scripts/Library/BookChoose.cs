using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookChoose : MonoBehaviour
{
    [Header("Detail")]
    [SerializeField] GameObject DetailObject;
    [SerializeField] Text DetailName;
    [SerializeField] Text DetailDesc;
    [SerializeField] Image DetailImage;

    [Header("Book")]
    [SerializeField] BookButton book;
    [SerializeField] RectTransform bookContainer;
    [SerializeField] List<Book> BookDatas = new List<Book>();

    void Start()
    {
        InitBooks();
    }

    void InitBooks()
    {
        for (int i = 0; i < StatusManager.Instance.BookUnlocked.Count; i++)
        {
            if (StatusManager.Instance.BookUnlocked[i])
            {
                BookButton temp = Instantiate(book, bookContainer);
                temp.Init(this, BookDatas[i]);
            }
        }
    }

    public void OpenDetail(Book _book)
    {
        DetailObject.SetActive(true);
        DetailName.text = _book.BookName;
        DetailDesc.text = _book.BookDesc2;
        DetailImage.sprite = _book.Image;
    }

    public void CloseDetail()
    {
        DetailObject.SetActive(false);
    }
}
