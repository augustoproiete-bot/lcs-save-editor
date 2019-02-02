﻿#region License
/* Copyright(c) 2016-2019 Wes Hampson
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion

using LcsSaveEditor.DataTypes;
using LcsSaveEditor.Infrastructure;

namespace LcsSaveEditor.Models
{
    /// <summary>
    /// Miscellaneous game variables.
    /// Theses are stored in block 0 of the save data file.
    /// </summary>
    public abstract class SimpleVars : SerializableObject
    {
        /* Disable "unused variable" warning.
         * Unused variables are denoted with a preceding '_'
         * and are included for reference only.
         */
        #pragma warning disable 0169

        protected string m_saveNameGxt;
        protected uint _m_currLevel;
        protected uint _m_currArea;
        protected Language m_prefsLanguage;
        protected uint m_millisecondsPerGameMinute;
        protected uint m_lastClockTick;
        protected byte m_gameClockHours;
        protected byte m_gameClockMinutes;
        protected ushort _m_gameClockSeconds;
        protected uint m_totalTimePlayedInMilliseconds;
        protected float _m_timeScale;
        protected float _m_timeStep;
        protected float _m_timeStepNonClipped;
        protected float _m_framesPerUpdate;
        protected uint _m_frameCounter;
        protected Weather m_prevWeatherType;
        protected Weather m_currWeatherType;
        protected Weather m_forcedWeatherType;
        protected uint m_weatherTypeInList;
        protected float _m_interpolationValue;
        protected Vector3d m_cameraPosition;
        protected VehicleCameraMode m_prefsVehicleCameraMode;
        protected uint m_prefsBrightness;
        protected bool m_prefsDisplayHud;
        protected bool m_prefsShowSubtitles;
        protected RadarMode m_prefsRadarMode;
        protected bool m_blurOn;
        protected bool m_prefsUseWideScreen;
        protected uint m_prefsRadioVolume;
        protected uint m_prefsSfxVolume;
        protected RadioStation _m_prefsRadioStation;
        protected byte _m_prefsOutput;
        protected ControllerConfig m_prefsControllerConfig;
        protected bool m_prefsDisableInvertLook;  // TODO: this is on PS2, I think 1 = disabled
        protected bool m_prefsInvertLook;         // TODO: this is on mobile, I think 1 = enabled
        protected bool m_prefsUseVibration;
        protected bool _m_prefsSwapNippleAndDPad;
        protected bool m_playerHasCheated;
        protected bool _m_allTaxisHaveNitro;
        protected bool m_targetIsOn;
        protected Vector2d m_targetPosition;
        protected Timestamp m_saveTime;

        #pragma warning restore 0169

        public SimpleVars()
        {
            m_prefsDisableInvertLook = true;
        }

        public string SaveNameGxt
        {
            get { return m_saveNameGxt; }
            set { m_saveNameGxt = value; OnPropertyChanged(); }
        }

        public Language Language
        {
            get { return m_prefsLanguage; }
            set { m_prefsLanguage = value; OnPropertyChanged(); }
        }

        public uint MillisecondsPerGameMinute
        {
            get { return m_millisecondsPerGameMinute; }
            set { m_millisecondsPerGameMinute = value; OnPropertyChanged(); }
        }

        public uint LastClockTick
        {
            get { return m_lastClockTick; }
            set { m_lastClockTick = value; OnPropertyChanged(); }
        }

        public byte GameClockHours
        {
            get { return m_gameClockHours; }
            set { m_gameClockHours = value; OnPropertyChanged(); }
        }

        public byte GameClockMinutes
        {
            get { return m_gameClockMinutes; }
            set { m_gameClockMinutes = value; OnPropertyChanged(); }
        }

        public uint TotalTimePlayedInMilliseconds
        {
            get { return m_totalTimePlayedInMilliseconds; }
            set { m_totalTimePlayedInMilliseconds = value; OnPropertyChanged(); }
        }

        public Weather PreviousWeather
        {
            get { return m_prevWeatherType; }
            set { m_prevWeatherType = value; OnPropertyChanged(); }
        }

        public Weather CurrentWeather
        {
            get { return m_currWeatherType; }
            set { m_currWeatherType = value; OnPropertyChanged(); }
        }

        public Weather ForcedWeather
        {
            get { return m_forcedWeatherType; }
            set { m_forcedWeatherType = value; OnPropertyChanged(); }
        }

        public uint WeatherIndex
        {
            get { return m_weatherTypeInList; }
            set { m_weatherTypeInList = value; OnPropertyChanged(); }
        }

        public Vector3d CameraPosition
        {
            get { return m_cameraPosition; }
            set { m_cameraPosition = value; OnPropertyChanged(); }
        }

        public VehicleCameraMode VehicleCamera
        {
            get { return m_prefsVehicleCameraMode; }
            set { m_prefsVehicleCameraMode = value; OnPropertyChanged(); }
        }

        public uint Brightness
        {
            get { return m_prefsBrightness; }
            set { m_prefsBrightness = value; OnPropertyChanged(); }
        }

        public bool ShowHud
        {
            get { return m_prefsDisplayHud; }
            set { m_prefsDisplayHud = value; OnPropertyChanged(); }
        }

        public bool ShowSubtitles
        {
            get { return m_prefsShowSubtitles; }
            set { m_prefsShowSubtitles = value; OnPropertyChanged(); }
        }

        public RadarMode RadarMode
        {
            get { return m_prefsRadarMode; }
            set { m_prefsRadarMode = value; OnPropertyChanged(); }
        }

        public bool BlurOn
        {
            get { return m_blurOn; }
            set { m_blurOn = value; OnPropertyChanged(); }
        }

        public bool Widescreen
        {
            get { return m_prefsUseWideScreen; }
            set { m_prefsUseWideScreen = value; OnPropertyChanged(); }
        }

        public uint RadioVolume
        {
            get { return m_prefsRadioVolume; }
            set { m_prefsRadioVolume = value; OnPropertyChanged(); }
        }

        public uint SfxVolume
        {
            get { return m_prefsSfxVolume; }
            set { m_prefsSfxVolume = value; OnPropertyChanged(); }
        }

        public ControllerConfig ControllerConfiguration
        {
            get { return m_prefsControllerConfig; }
            set { m_prefsControllerConfig = value; OnPropertyChanged(); }
        }

        public bool InvertLook
        {
            get { return m_prefsInvertLook || !m_prefsDisableInvertLook; }
            set {
                m_prefsInvertLook = value;
                m_prefsDisableInvertLook = !value;
                OnPropertyChanged();
            }
        }

        public bool Vibration
        {
            get { return m_prefsUseVibration; }
            set { m_prefsUseVibration = value; OnPropertyChanged(); }
        }

        public bool PlayerHasCheated
        {
            get { return m_playerHasCheated; }
            set { m_playerHasCheated = value; OnPropertyChanged(); }
        }

        public bool ShowWaypoint
        {
            get { return m_targetIsOn; }
            set { m_targetIsOn = value; OnPropertyChanged(); }
        }

        public Vector2d WaypointPosition
        {
            get { return m_targetPosition; }
            set { m_targetPosition = value; OnPropertyChanged(); }
        }

        public Timestamp Timestamp
        {
            get { return m_saveTime; }
            set { m_saveTime = value; OnPropertyChanged(); }
        }
    }
}