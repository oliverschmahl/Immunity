using UnityEngine;
using System.Collections;
 
public class CGFade : MonoBehaviour {
    private CanvasGroup _canvasGroup;
    [SerializeField] private float _duration = 3f;
 
    private void Awake() {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
 
    private void Start() {
        StartCoroutine(FadeCGAlpha(0f, 1f, _duration));
    }
 
    private IEnumerator FadeCGAlpha(float from, float to, float duration) {
        float elaspedTime = 0f;
        while (elaspedTime <= duration) {
            elaspedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(from, to, elaspedTime / duration);
            yield return null;
        }
        _canvasGroup.alpha = to;
    }
}
