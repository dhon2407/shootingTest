using System;
using System.Collections;
using Dan.UI.Core;
using UnityEngine;

namespace Dan.UI
{
    public class TitleScreen : UIScreen
    {
        public event Action OnPressPlay;

        public void StartNewGame()
        {
            StartCoroutine(WaitToStartPlay());
        }
        
        
        private IEnumerator WaitToStartPlay()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnPressPlay?.Invoke();
                    yield break;
                }
                yield return null;
            }
        }
    }
}