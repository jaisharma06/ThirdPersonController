/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace RayWenderlich.CommandPatternInUnity
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private Text textPrefab = null;
        [SerializeField]
        private Transform textParent = null;
        [SerializeField]
        private ScrollRect scrollRect = null;
        [SerializeField]
        private Canvas victoryText = null;
        private Coroutine scrollReset;
        private List<Text> commandTextList = new List<Text>();

        internal void InsertNewText(string value)
        {
            var textUI = Instantiate(textPrefab, textParent);
            textUI.text = $"<color=cyan>></color> {value};";
            commandTextList.Add(textUI);
            scrollReset = StartCoroutine(ResetScrollToBottom());
        }

        internal void RemoveFirstTextLine()
        {
            StartCoroutine(RemoveFirstLine());
        }

        internal void RemoveLastTextLine()
        {
            var lastLine = commandTextList[commandTextList.Count - 1];
            commandTextList.RemoveAt(commandTextList.Count - 1);

            Destroy(lastLine.gameObject);
        }

        internal void ShowVictory()
        {
            victoryText.enabled = true;
        }

        internal void ResetScrollToTop()
        {
            StopCoroutine(scrollReset);
            scrollRect.verticalNormalizedPosition = 1;
        }

        private IEnumerator ResetScrollToBottom()
        {
            yield return null;
            scrollRect.verticalNormalizedPosition = 0;
        }

        private IEnumerator RemoveFirstLine()
        {
            var firstLine = commandTextList[0];
            commandTextList.RemoveAt(0);

            firstLine.GetComponentInChildren<Text>().color = Color.green;
            yield return new WaitForSeconds(SceneManager.CommandPauseTime / 2f);
            Destroy(firstLine.gameObject);
        }
    }
}
