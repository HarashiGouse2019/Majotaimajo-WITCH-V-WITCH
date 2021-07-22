
public class Prompt
{
    PromptMessage _message;
    PromptChoices[] _choices;
    public PromptChoices selectedChoice = null;
    ISelectable _previousSelectable;

    public PromptMessage Message
    {
        get
        {
            return _message;
        }
    }

    public PromptChoices[] Choices
    {
        get
        {
            return _choices;
        }
    }

    public Prompt(PromptMessage message, ISelectable previousSelectable = null, params PromptChoices[] choices)
    {
        _message = message ?? new PromptMessage("Pick an option");
        _choices = choices ?? new PromptChoices[2]
        {
                new PromptChoices("Yes"),
                new PromptChoices("No")
        };
        selectedChoice = null;
        _previousSelectable = previousSelectable;
        PromptSystem.PromptStack.Push(this);
    }

    public Prompt(string message, ISelectable previousSelectable, params PromptChoices[] choices)
    {
        new Prompt(new PromptMessage(message), previousSelectable, choices);
    }
}
