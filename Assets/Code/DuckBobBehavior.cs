﻿using System;
using UnityEngine;

namespace Assets.Code
{
	public class DuckBobBehavior : MonoBehaviour
	{
		[SerializeField] private Manager _manager;
		[SerializeField] private Transform _transform;
		[SerializeField] private SurfaceBehaviour _surface;

		private Vector3 _velocity = new Vector3(0, 0, 0);

		private float _runtime;

		// Use this for initialization
		void Start ()
		{
			_runtime = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
			float DuckY = 0;
			//_velocity.x = 0;
			//_velocity.y = 0;
			//_velocity.z = 0;
			foreach (WaveOriginData wave in _surface.Waves)
			{
				Vector3 waveToDuck = _transform.position - wave.Origin;
				waveToDuck.Normalize();
				Vector3 wavePosition = wave.Origin + waveToDuck * WaveOriginData.WAVE_VELOCITY * wave.Age;
				float cosineInput = (-_runtime * 0.5f + (waveToDuck).magnitude);
				float waveScale = _surface.SmoothStep(0, WaveOriginData.WAVE_WIDTH, Mathf.Abs((wavePosition - _transform.position).magnitude));
				float appliedMagnitude = wave.Magnitude * wave.PercentLife;// * waveScale;
				DuckY += -Mathf.Cos(cosineInput);// * appliedMagnitude// * waveScale;

				// Gets 0-1 representation of cosine input. works because waves recur
				// transforms 0-1 into 0-255 for normal lookup
				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);

				Vector3 forceDirection = new Vector3();
				forceDirection.x = waveToDuck.x;
				forceDirection.y = _manager.Normals.Normals[normalLookupIndex].y;
				forceDirection.z = waveToDuck.z;
				forceDirection.Normalize();

				Vector3 waveVelocityContribution = forceDirection * appliedMagnitude;

				// Testing
				waveVelocityContribution = new Vector3(0, forceDirection.y, 0);

				//_velocity += waveVelocityContribution;
			}

			float vDecay = 3;

			_transform.position += _velocity;

			_velocity /= vDecay;

			_runtime += Time.deltaTime;
		}
	}
}
