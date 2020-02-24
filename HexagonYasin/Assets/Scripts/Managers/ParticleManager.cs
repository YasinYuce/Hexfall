using System;
using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class ParticleManager : MonoBehaviour
    {
        public GameObject PieceParticlePrefab;
        private Pool<PieceParticle> mParticlePool;

        public Color[] Colors { get; private set; }

        public void Initialize()
        {
            mParticlePool = new Pool<PieceParticle>(new PrefabFactory<PieceParticle>(PieceParticlePrefab, transform));
        }

        public void StartGame(Color[] colors)
        {
            Colors = colors;
        }

        public void ShowParticle(GridPiece explodedPiece)
        {
            if (explodedPiece.GetComponent<IColored>() != null)
            {
                PieceParticle particle = mParticlePool.Allocate();
                particle.gameObject.SetActive(true);
                Action action = null;

                action = () => { mParticlePool.Release(particle); particle.Release -= action; };
                particle.Release += action;

                particle.ShowParticle(explodedPiece.transform.position + Vector3.back, Colors[explodedPiece.GetComponent<IColored>().ColorIndex]);
            }
        }
    }
}