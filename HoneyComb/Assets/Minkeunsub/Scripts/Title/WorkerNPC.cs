using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerNPC : NpcBase
{
    protected override void FirstMeetScript()
    {
        SpeechOn("안녕 나는 일벌 대장이야!", "너는 이제부터 우리 꿀벌들을 조작해 꿀을 채취하는 임무를 맡게 될 거야", "꿀벌들은 한 번 일을 하고 오면 5분정도 쉬어줘야 일을 계속할 수 있어", "지금은 벌들이 다 사라져서 15마리밖에 없지만 한때는 5만마리까지도 있었다.", "아무튼 우리 벌집의 부흥을 위해 수고해줘");
    }

    protected override void QuestClearMessage()
    {
    }

    protected override void QuestExistsMessage()
    {
    }
}
