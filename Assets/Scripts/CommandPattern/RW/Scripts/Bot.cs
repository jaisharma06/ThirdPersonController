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
    using UnityEngine;

    public enum CardinalDirection
    {
        Up, Down, Right, Left
    }

    public class Bot : MonoBehaviour
    {
        private const float MoveTime = 0.3f;
        private const float MoveOffset = 1f;

        private static Vector3 DefaultPosition = new Vector3(4, 0, 0);
        private static Quaternion DefaultModelRotation = Quaternion.identity;

        [Header("Set In Inspector")]       
        [SerializeField]
        private Animator botAnimator = null;
        [SerializeField]
        private AudioClip botMove = null;
        [SerializeField]
        private AudioClip botMoveFail = null;
        [SerializeField]
        private AudioClip shoot = null;
        [SerializeField]
        private Transform botBody = null;
        [SerializeField]
        private Transform muzzlePoint = null;
        [SerializeField]
        private GameObject bulletPrefab = null;
        [SerializeField]
        private LayerMask wallLayerMask = new LayerMask();
        private Coroutine moveRoutine;
        private Vector3 lastCheckpointPosition = DefaultPosition;
        
        public void Move(CardinalDirection direction)
        {
            if (moveRoutine != null)
            {
                return;
            }

            Vector3 directionVector = Vector3.zero;
            switch (direction)
            {
                case CardinalDirection.Up:
                    directionVector = Vector3.forward;
                    break;
                case CardinalDirection.Down:
                    directionVector = Vector3.back;
                    break;
                case CardinalDirection.Right:
                    directionVector = Vector3.right;
                    break;
                case CardinalDirection.Left:
                    directionVector = Vector3.left;
                    break;
            }

            botBody.rotation = Quaternion.LookRotation(directionVector);

            if (Physics.Raycast(botBody.position, directionVector, MoveOffset, wallLayerMask))
            {
                AudioPlayer.Instance.PlaySFX(botMoveFail);
                return;
            }

            moveRoutine = StartCoroutine(MoveRoutine(directionVector * MoveOffset));
        }

        public void ResetToLastCheckpoint()
        {
            if (moveRoutine != null)
            {
                StopCoroutine(moveRoutine);
                moveRoutine = null;
            }

            botAnimator.SetTrigger("Idle");
            botBody.rotation = DefaultModelRotation;
            transform.position = lastCheckpointPosition;
        }

        public void Shoot()
        {
            botAnimator.SetTrigger("Shoot");
            AudioPlayer.Instance.PlaySFX(shoot);
            Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);
        }

        public void SetCheckpoint(Checkpoint checkpoint)
        {
            lastCheckpointPosition = checkpoint.transform.position;
        }

        private IEnumerator MoveRoutine(Vector3 positionOffset)
        {
            botAnimator.SetTrigger("Move");
            AudioPlayer.Instance.PlaySFX(botMove);
            var startPos = transform.position;
            var targetPos = transform.position + positionOffset;
            for (float t = 0; t < MoveTime; t += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, t / MoveTime);
                yield return null;
            }

            transform.position = targetPos;
            moveRoutine = null;
        }
    }
}
