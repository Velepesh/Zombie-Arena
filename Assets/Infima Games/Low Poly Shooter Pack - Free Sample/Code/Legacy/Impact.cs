using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
    public class Impact : MonoBehaviour
	{
		[SerializeField] private float despawnTimer = 10.0f;

        public event UnityAction Despawned;
        public event UnityAction<Impact> Impacted;

        public void Play()
		{
			StartCoroutine(DespawnTimer());

            Despawned?.Invoke();
        }

		private IEnumerator DespawnTimer()
		{
			yield return new WaitForSeconds(despawnTimer);

            Impacted?.Invoke(this);
		}
	}
}