using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerNPC : NpcBase
{
    protected override void FirstMeetScript()
    {
        SpeechOn("�ȳ� ���� �Ϲ� �����̾�!", "�ʴ� �������� �츮 �ܹ����� ������ ���� ä���ϴ� �ӹ��� �ð� �� �ž�", "�ܹ����� �� �� ���� �ϰ� ���� 5������ ������� ���� ����� �� �־�", "������ ������ �� ������� 15�����ۿ� ������ �Ѷ��� 5������������ �־���.", "�ƹ�ư �츮 ������ ������ ���� ��������");
    }

    protected override void QuestClearMessage()
    {
    }

    protected override void QuestExistsMessage()
    {
    }
}
