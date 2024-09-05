using QGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : QScript
{
    public UIDocument UIDocument;
    StopWatchNode _node;

    // Start is called before the first frame update
    void Start()
    {
        var label = UIDocument.rootVisualElement.Q<Label>("char_name");
        label.text = "Hello World!";

        _node = StopWatch.AddNode("test", 10);
        var slider = UIDocument.rootVisualElement.Q<Slider>("my_slider");
        OnEveryUpdate += () =>
        {
			slider.value = _node.ElapsedLifetimeAsZeroToOne * 100;
            Debug.Log(_node.ElapsedLifetimeAsZeroToOne);
		};
    }
}
