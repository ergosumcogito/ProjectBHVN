using UnityEngine;

namespace Core.Enemy_Logic
{
    public class SortByY : MonoBehaviour
    {
        private SpriteRenderer sr;


        void Awake()
        {
            if (sr == null)
                sr = GetComponent<SpriteRenderer>();
        }

        /*
         * Late Update is called after Update()
         * It ensures that the player / enemy is sorted by its position on the y-axis
         * Game Object with small y position should overlapp other gameobject
         * e.g. enemy 3 * (-100) &&  player 1 * (-100) -> player overlaps enemy
         */
        void LateUpdate()
        {
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }
}