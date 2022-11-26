using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : MonoBehaviour
{
    // ���� �� ���¿��� �����ؾ��� �׼ǵ��� ���⼭ ������ �ִ´�
    public List<AIAction> actions = null;

    // ���� �� ���¿��� ���̰� ������ ���·��� ���� ����Ʈ��
    public List<AITransition> transitions = null;

    private AIBrain _brain;

    private void Awake()
    {
        _brain = transform.parent.parent.GetComponent<AIBrain>();
    }

    public void UpdateState()
    {
        // ��� ���´� �� �����Ӹ��� �� �޼��带 �����Ѵ�
        foreach (AIAction a in actions)
        {
            a.TakeAction();
        }

        foreach (AITransition t in transitions)
        {
            bool result = false;

            foreach (AIDecision d in t.decisions)
            {
                result = d.MakeADecision(); // �����ض�
                if (!result) break;
            }

            if (result) // ��� ������� �����ϴ� ���´ϱ� ���̸� �ؾ��Ѵ�.
            {
                if (t.positiveResult != null)
                {
                    // positiveResult�� ���̸� �߻�
                    _brain.ChangeToState(t.positiveResult);
                }
            }
            else
            {
                if (t.negativeResult != null)
                {
                    // negativeResult�� ���̸� �ؾ���
                    _brain.ChangeToState(t.negativeResult);
                }
            }
        }
    }
}
