using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LibraryManager : MonoBehaviour
{
    [Header("UI objects")]
    [SerializeField] Text WaxText;
    [SerializeField] Text HoneyText;
    [SerializeField] Image[] Locked = new Image[2];

    [Header("Cartoon")]
    [SerializeField] GameObject CartoonChoose;
    [SerializeField] Sprite[] ButtonSprites;
    [SerializeField] RectTransform button;
    [SerializeField] RectTransform ContentContainer;
    [SerializeField] RectTransform rightVoid;
    List<RectTransform> buttonList = new List<RectTransform>();

    [Header("Book Choose")]
    [SerializeField] BookChoose bookChoose;

    [Header("Data")]
    [SerializeField] int count = 5;
    [SerializeField] int curIdx;
    [SerializeField] int maxIdx;

    Vector2 DefaultSize = new Vector2(700, 1000);
    Vector2 SelectSize = new Vector2(800, 1200);

    void Start()
    {
        InitScroll(CartoonManager.Instance.cartoons.Count);
    }

    void Update()
    {
        WaxText.text = Format(StatusManager.Instance.BeeWax);
        HoneyText.text = Format(StatusManager.Instance.Honey);
        Locked[0].gameObject.SetActive(!StatusManager.Instance.bookAble);
        Locked[1].gameObject.SetActive(!StatusManager.Instance.bookAble);

        SetScrollRect();
        SetButtonSize();
    }

    string Format(float value)
    {
        string retStr = string.Format("{0:#,0}", value);
        return retStr;
    }

    public void BookOn()
    {

        if (StatusManager.Instance.bookAble)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            bookChoose.gameObject.SetActive(true);
            bookChoose.UIon();
        }
        else
            SoundManager.Instance.PlaySound("Button_Click_Fail");
    }

    void SetButtonSize()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            if (i == curIdx)
                buttonList[i].sizeDelta = Vector2.Lerp(buttonList[i].sizeDelta, SelectSize, Time.deltaTime * 15f);
            else
                buttonList[i].sizeDelta = Vector2.Lerp(buttonList[i].sizeDelta, DefaultSize, Time.deltaTime * 15f);
        }
    }

    public void Back()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        SceneLoadManager.Instance.LoadScene("TitleScene");
    }

    void SetScrollRect()
    {
        float posX = -(curIdx * 800f + 480f);
        ContentContainer.anchoredPosition = Vector2.Lerp(ContentContainer.anchoredPosition, new Vector2(posX, 0), Time.deltaTime * 15f);
    }

    void InitScroll(int n)
    {
        maxIdx = n;
        for (int i = 0; i < maxIdx-1; i++)
        {
            RectTransform temp = Instantiate(button, ContentContainer.transform);
            temp.GetComponent<Image>().sprite = ButtonSprites[i];

            int idx = i;
            temp.GetComponent<Button>().onClick.AddListener(() => { OnClickEvent(idx); });
            buttonList.Add(temp);
        }
        button.gameObject.SetActive(false);
        rightVoid.SetAsLastSibling();
    }

    void OnClickEvent(int idx)
    {
        SoundManager.Instance.PlaySound("Button_Click");
        SoundManager.Instance.PlaySound("PageFlip");
        CartoonManager.Instance.CartoonStartByLibrary(idx);
    }

    public void SetScrollIdx(int idx)
    {
        SoundManager.Instance.PlaySound("PageFlip",SoundType.SE,2,1.5f);
        curIdx += idx;
        if (curIdx == maxIdx - 1) curIdx = 0;
        else if (curIdx < 0) curIdx = maxIdx - 2;
    }

    public void OpenCartoon()
    {
        if (StatusManager.Instance.bookAble)
        {
            SoundManager.Instance.PlaySound("Button_Click");
            CartoonChoose.SetActive(true);
        }
        else
            SoundManager.Instance.PlaySound("Button_Click_Fail");
    }

    public void CloseCartoon()
    {
        SoundManager.Instance.PlaySound("Button_Click");
        CartoonChoose.SetActive(false);
    }
}
