using Scripts.Cell;
using System.Collections;
using UnityEngine;

namespace Scripts.Ball
{  
    public class BallView : CellObject
    {       
        public override IEnumerator PlayHide(bool fast = false)
        {
            SetColor(Color.red);
            var startTime = Time.time;
            while (Time.time - startTime < 1 && !fast)
            {
                var scale = 1 - (Time.time - startTime);
                _sprite.transform.localScale =new Vector3(scale, scale, scale);
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
            yield return null;
        }
    }
}
