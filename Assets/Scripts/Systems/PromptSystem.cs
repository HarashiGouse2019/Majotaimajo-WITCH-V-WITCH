using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

using static Keymapper;

public class PromptSystem : Singleton<PromptSystem>
{

    [SerializeField]
    Canvas _promptCanvas;

    [SerializeField]
    TextMeshProUGUI promptMessageText;

    [SerializeField]
    PromptChoicesObject[] promptChoicesObjects = new PromptChoicesObject[4];

    [SerializeField]
    Color selectedOption, unselectedOption;

    public static Stack<Prompt> PromptStack = new Stack<Prompt>(4);

    IEnumerator _promptCycle;

    // Start is called before the first frame update
    private void Start()
    {
        _promptCycle = PromptCycle();
        StartCoroutine(_promptCycle);
    }


    IEnumerator PromptCycle()
    {
        int choiceIndex = 0;
        int promptsToAnswer = 0;
        Prompt prompt = null;
        while (true)
        {

            while (PromptStack.Count > 0)
            {
                _promptCanvas.gameObject.SetActive(true);
                promptsToAnswer++;
                prompt = PromptStack.Pop();

                for (int i = 0; i < prompt.Choices.Length; i++)
                {
                    PromptChoices choice = prompt.Choices[i];
                    promptChoicesObjects[i].GetMessageText().text = choice.Message;
                }
            }

            if (promptsToAnswer > 0)
            {
                //TODO: Set up prompt values into thingy. Wait until choice has been made.
                promptMessageText.text = prompt.Message.Message;

                SelectCurrentChoice(choiceIndex);

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    choiceIndex--;
                    if (choiceIndex < 0)
                    {
                        choiceIndex = prompt.Choices.Length - 1;
                    }
                    prompt.selectedChoice = prompt.Choices[choiceIndex];
                    SelectCurrentChoice(choiceIndex);
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    choiceIndex++;
                    if (choiceIndex > prompt.Choices.Length -1)
                    {
                        choiceIndex = 0;
                    }
                    prompt.selectedChoice = prompt.Choices[choiceIndex];
                    SelectCurrentChoice(choiceIndex);
                }

                EventManager.Watch(OnKeyDown("start"), () =>
                {
                    prompt.selectedChoice.OnSelect.Trigger();
                    promptsToAnswer--;
                }, null);
            }
           

                _promptCanvas.gameObject.SetActive(promptsToAnswer == 0 ? false : true);

            yield return null;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(_promptCycle);
    }

    private void SelectCurrentChoice(int choiceIndex)
    {
        foreach(PromptChoicesObject obj in promptChoicesObjects)
        {
            if(Array.IndexOf(promptChoicesObjects, obj) == choiceIndex)
            {
                //Enable choice cursor;
                obj.EnableCursor();
                obj.GetMessageText().color = selectedOption;
            } else
            {
                //Disable choice cursor and set unselected color
                obj.DisableCursor();
                obj.GetMessageText().color = unselectedOption;
            }
        }
    }
}
