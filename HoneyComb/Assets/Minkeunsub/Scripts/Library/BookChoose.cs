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
        gameObject.SetActive(false);
    }

    private void Update()
    {

    }

    void InitBooks()
    {
        BookDatas = StatusManager.Instance.BookDatas;

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
        SoundManager.Instance.PlaySound("Button_Click");
        DetailObject.SetActive(true);
        DetailName.text = _book.BookName;
        DetailDesc.text = _book.BookDesc2;
        DetailImage.sprite = _book.Image;
    }

    public void CloseDetail()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        DetailObject.SetActive(false);
    }

    public void UIon()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        BG.anchoredPosition = new Vector2(1500, 0);
        BG.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack);
    }

    public void UIoff()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        BG.DOAnchorPosX(1500f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
