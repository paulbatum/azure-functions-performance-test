﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessBenchmark.LoadProfiles
{
    public class LinearWithRampUp : TriggerTestLoadProfile
    {
        private int _targetEps;
        private int _startLinearLoad;
        private int _endLinearLoad;
        private double inclineRate;
        private bool _isFinished = false;
        private double _rampTimePercentage = 0.25;

        /// <summary>
        /// First 25% of time use for ramp up and last 25% for ramp down
        /// </summary>
        /// <param name="loadDuration"></param>
        /// <param name="eps"></param>
        public LinearWithRampUp(TimeSpan loadDuration, int eps, bool rampDown = true) : base(loadDuration)
        {
            _targetEps = eps;
            var totalSeconds = loadDuration.TotalSeconds;
            _startLinearLoad = (int)(totalSeconds * _rampTimePercentage);
            _endLinearLoad = (int)totalSeconds;

            if (rampDown)
            {
                _endLinearLoad = (int)(totalSeconds - _startLinearLoad);
            }

            inclineRate = eps / (double)_startLinearLoad;
        }

        protected override int ExecuteRate(int second)
        {
            if (second < _startLinearLoad)
            {
                return (int)(inclineRate * second);
            }

            if (second > _endLinearLoad)
            {
                return _targetEps - (int) (inclineRate*(second - _endLinearLoad));
            }

            return _targetEps;
        }

        protected override bool IsFinished()
        {
            return _isFinished;
        }
    }
}
