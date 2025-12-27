using System.Collections;
using UnityEngine;

namespace Core.Enemy_Logic
{
    public class DamageFlash : MonoBehaviour
    {
        [Tooltip("Material to switch to during the flash")] [SerializeField]
        private Material flashMaterial;

        [Tooltip("Duration of the flas")] [SerializeField]
        private float duration;

        //the SpriteRenderer that should flash
        private SpriteRenderer _spriteRenderer;

        //the material that was in use when the script started
        private Material originalMaterial;

        //the currently running corutine
        private Coroutine flashRoutine;


        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            originalMaterial =
                _spriteRenderer.material; // get material of sprite renderer - switch back to it after end flash
        }

        public void Flash()
        {
            if (flashRoutine != null)
            {
                StopCoroutine(flashRoutine);
            }

            flashRoutine = StartCoroutine(FlashRoutine());
        }

        private IEnumerator FlashRoutine()
        {
            // switch to the flashMaterial
            _spriteRenderer.material = flashMaterial;
            //Pause execution
            yield return new WaitForSeconds(duration);
            //continue
            _spriteRenderer.material = originalMaterial;
            //set to null when finished
            flashRoutine = null;
        }
    }
}