using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ITTV.WPF.Core.Models
{ 
    public sealed class Settings
    {
        public event Action<Settings> SettingsUpdated;

        private void OnSettingsUpdated()
        {
            SettingsUpdated?.Invoke(this);
        }
        
        [JsonProperty("isAdminMode")]
        private bool _isAdminMode;

        public bool IsAdminMode
        {
            get => _isAdminMode;
            set
            {
                if (Equals(_isAdminMode, value))
                    return;
                
                _isAdminMode = value;
                OnSettingsUpdated();
            }
        }
        
        [JsonProperty("needCheckTime")]
        private bool _needCheckTime;

        public bool NeedCheckTime
        {
            get => _needCheckTime;
            set
            {
                if (Equals(_needCheckTime, value))
                    return;
                
                _needCheckTime = value;
                OnSettingsUpdated();
            }
        }
        
        [JsonProperty("startWorkTime")]
        private TimeSpan _startWorkTime;

        public TimeSpan StartWorkTime
        {
            get => _startWorkTime;
            set
            {
                if (Equals(_startWorkTime, value))
                    return;

                _startWorkTime = value;
                OnSettingsUpdated();
            }
        }
        
        [JsonProperty("endWorkTime")]
        private TimeSpan _endWorkTime;

        public TimeSpan EndWorkTime
        {
            get => _endWorkTime;
            set
            {
                if (Equals(_endWorkTime, value))
                    return;

                _endWorkTime = value;
                OnSettingsUpdated();
            }
        }
        
        [JsonProperty("videoVolume")]
        private double _videoVolume;

        public double VideoVolume
        {
            get => _videoVolume;
            set
            {
                if (Equals(_videoVolume, value))
                    return;
                
                _videoVolume = value;
                OnSettingsUpdated();
            }
        }
        
        [JsonProperty("backgroundVideoOrder")]
        private List<string> _backgroundVideoOrder;

        public List<string> BackgroundVideoOrder
        {
            get => _backgroundVideoOrder;
            set
            {
                if (Equals(_backgroundVideoOrder, value))
                    return;

                if (_backgroundVideoOrder != null && value != null)
                {
                    if (_backgroundVideoOrder.SequenceEqual(value))
                        return;
                }
                
                _backgroundVideoOrder = value;
                OnSettingsUpdated();
            }
        }

        [JsonProperty("inactiveModeTime")] 
        private TimeSpan _inactiveModeTime;

        public TimeSpan InactiveModeTime
        {
            get => _inactiveModeTime;
            set
            {
                if (Equals(_inactiveModeTime, value))
                    return;

                _inactiveModeTime = value;
                OnSettingsUpdated();
            }
        }

        [JsonProperty("cacheUpdateInterval")] 
        private TimeSpan _cacheUpdateInterval;

        public TimeSpan CacheUpdateInterval
        {
            get => _cacheUpdateInterval;
            set
            {
                if (Equals(_cacheUpdateInterval, value))
                    return;

                _cacheUpdateInterval = value;
                OnSettingsUpdated();
            }
        }

        [JsonProperty("eggVideoCommands")] 
        private string[] _eggVideoCommands;
        public string[] EggVideoCommands
        {
            get => _eggVideoCommands;
            set => _eggVideoCommands = value;
        }
    }
}
