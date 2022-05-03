using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BookChoose : MonoBehaviour
{

    [Header("UI objects")]
    [SerializeField] RectTransform BG;

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

    public void UIon()
    {
        BG.anchoredPosition = new Vector2(1500, 0);
        BG.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIoff()
    {
        BG.DOAnchorPosX(1500f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
