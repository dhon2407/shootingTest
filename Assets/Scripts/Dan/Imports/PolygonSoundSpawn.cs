using Dan.Manager;
using UnityEngine;

namespace PolygonArsenal
{
    public class PolygonSoundSpawn : MonoBehaviour
    {
        public GameObject prefabSound;
        [Range(0.01f, 10f)]
        public float pitchRandomMultiplier = 1f;

        private AudioSource _sSource;
        
        public void Play()
        {
            _sSource.PlayOneShot(_sSource.clip);
        }
        
        private void Awake()
        {
            var m_Sound = Instantiate(prefabSound, transform.position, Quaternion.identity);
            m_Sound.transform.SetParent(FreeObjectPool.Transform);

            _sSource = m_Sound.GetComponent<AudioSource>();

            if (pitchRandomMultiplier != 1)
            {
                if (Random.value < .5)
                    _sSource.pitch *= Random.Range(1 / pitchRandomMultiplier, 1);
                else
                    _sSource.pitch *= Random.Range(1, pitchRandomMultiplier);
            }
        }
    }
}
