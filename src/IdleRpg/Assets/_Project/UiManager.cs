using UnityEngine;
using QGame;
using UnityEngine.UIElements;

public class UiManager : QScript
{
    [SerializeField]
    private UIDocument _wholeScreenDocument;

    [SerializeField]
    private QuestTabController _questTabController;
    public QuestTabController QuestTabController { get { return _questTabController; } }

	private void Start()
	{
		_questTabController.Initialize(_wholeScreenDocument);
	}
}