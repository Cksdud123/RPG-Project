using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float timeMultiplier; // 시간 가속도 조절 변수
    [SerializeField] private float startHour; // 하루의 시작 시간 (24시간 형식)
    [SerializeField] TextMeshProUGUI timeText; // 현재 시간을 표시할 UI 텍스트 컴포넌트

    [Header("SunRise")]
    [SerializeField] private Light sunLight; // 씬에 있는 태양 광원
    [SerializeField] private float sunriseHour; // 해가 뜨는 시간 (24시간 형식)
    [SerializeField] private float sunsetHour; // 해가 지는 시간 (24시간 형식)

    [Header("Ambient Light")]
    [SerializeField] private Color dayAmbientLight; // 낮 동안의 주변 광 색상
    [SerializeField] private Color nightAmbientLight; // 밤 동안의 주변 광 색상
    [SerializeField] private AnimationCurve lightChangeCurve; // 광 세기를 부드럽게 전환하는 곡선
    [SerializeField] private float maxSunLightIntensity; // 태양 광원의 최대 세기
    [SerializeField] private Light moonLight; // 씬에 있는 달 광원
    [SerializeField] private float maxMoonLightIntensity; // 달 광원의 최대 세기

    private DateTime currentTime; // 현재 게임 내 시간
    private TimeSpan sunriseTime; // 해 뜨는 시간의 TimeSpan 값
    private TimeSpan sunsetTime; // 해 지는 시간의 TimeSpan 값

    // Start는 첫 프레임 업데이트 전에 호출됩니다.
    void Start()
    {
        // 현재 시간을 시작 시간으로 초기화합니다.
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        // 해 뜨는 시간과 해 지는 시간을 초기화합니다.
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update는 매 프레임마다 호출됩니다.
    void Update()
    {
        // 게임 내 시간을 업데이트합니다.
        UpdateTimeOfDay();
        // 현재 시간에 따라 태양을 회전시킵니다.
        RotateSun();
        // 광 설정(주변 광, 태양 광, 달 광)을 업데이트합니다.
        UpdateLightSettings();
    }

    // 현재 게임 내 시간을 업데이트합니다.
    private void UpdateTimeOfDay()
    {
        // 델타 시간과 시간 가속도를 기반으로 현재 시간을 증가시킵니다.
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        // 텍스트 컴포넌트가 null이 아닌 경우 UI 텍스트를 현재 시간으로 업데이트합니다.
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    // 현재 시간에 따라 태양을 회전시킵니다.
    void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            // 해 뜨는 시간과 해 지는 시간 사이의 기간을 계산합니다.
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            // 해 뜬 이후 경과된 시간을 계산합니다.
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            // 해 뜬 이후 경과된 시간의 비율을 계산합니다.
            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            // 비율에 따라 태양의 회전을 0도에서 180도 사이에서 선형 보간합니다.
            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            // 해 지는 시간과 다음 해 뜨는 시간 사이의 기간을 계산합니다.
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            // 해 진 이후 경과된 시간을 계산합니다.
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            // 해 진 이후 경과된 시간의 비율을 계산합니다.
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            // 비율에 따라 태양의 회전을 180도에서 360도 사이에서 선형 보간합니다.
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        // 계산된 회전을 태양 광원에 적용합니다.
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    // 태양의 위치에 따라 주변 광, 태양 광, 달 광 설정을 업데이트합니다.
    private void UpdateLightSettings()
    {
        // 태양의 전방 방향과 아래쪽 방향의 내적을 계산합니다.
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // 내적과 광 변화 곡선을 기반으로 태양 광원의 세기를 조절합니다.
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        // 내적과 광 변화 곡선을 기반으로 달 광원의 세기를 반대로 조절합니다.
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));

        // 내적과 광 변화 곡선을 기반으로 주변 광 색상을 조절합니다.
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    // 두 TimeSpan 값 사이의 시간 차이를 계산하고, 음수 차이를 조정합니다.
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        // 차이가 음수인 경우 24시간을 더해 조정합니다.
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
