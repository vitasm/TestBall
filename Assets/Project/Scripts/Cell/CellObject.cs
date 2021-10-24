using System.Collections;
using UnityEngine;

namespace Scripts.Cell
{
    public class CellObject : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _sprite;

        private bool _enable = true;

        public void SetColor(Color newColor)
        {
            _sprite.color = newColor;
        }

        public void Show()
        {
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.SetActive(true);
            SetColor(Color.white);
            _enable = true;
        }

        public void Hide(bool fast = false)
        {
            if (!_enable)
                return;            
            _enable = false;
            StartCoroutine(PlayHide(fast));
        }

        public virtual IEnumerator PlayHide(bool fast = false)
        {
            var startTime = Time.time;
            while (Time.time - startTime < 1 && !fast)
            {
                _sprite.color = new Color(1, 1, 1, 1 - (Time.time - startTime));
                yield return new WaitForEndOfFrame();
            }
            _sprite.color = new Color(1, 1, 1, 0);
            gameObject.SetActive(false);
            yield return null;
        }
    }
}
