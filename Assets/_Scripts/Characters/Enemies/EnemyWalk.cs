using UnityEngine;

namespace Enemy
{
    public class EnemyWalk : EnemyBase
    {
        public GameObject[] waypoints;
        public float speed = 5f;
        public float minDistance = 1f;

        private int _index;

        protected override void Update()
        {
            base.Update();
            if (waypoints.Length == 0) return;
            if (Vector3.Distance(transform.position, waypoints[_index].transform.position) < minDistance)
            {
                _index++;
                if ( _index >= waypoints.Length )
                {
                    _index = 0;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[_index].transform.position, Time.deltaTime * speed);
            transform.LookAt(waypoints[_index].transform.position);
        }
    }
}

