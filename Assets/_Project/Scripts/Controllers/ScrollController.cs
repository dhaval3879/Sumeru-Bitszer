using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    private ScrollRect _scrollRect;
    private bool _dataLoadingStarted = false;

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();

        _scrollRect.onValueChanged.AddListener(OnScoll);
    }

    private void OnScoll(Vector2 pos)
    {
        if (_scrollRect.normalizedPosition.y < 1f)
        {
            if (!_dataLoadingStarted)
            {
                Events.OnMyAuctionsScrolledToBottom.Invoke();

                _dataLoadingStarted = true;
            }
        }
        else
            _dataLoadingStarted = false;
    }
}