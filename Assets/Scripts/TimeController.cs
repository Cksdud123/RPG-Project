using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float timeMultiplier; // �ð� ���ӵ� ���� ����
    [SerializeField] private float startHour; // �Ϸ��� ���� �ð� (24�ð� ����)
    [SerializeField] TextMeshProUGUI timeText; // ���� �ð��� ǥ���� UI �ؽ�Ʈ ������Ʈ

    [Header("SunRise")]
    [SerializeField] private Light sunLight; // ���� �ִ� �¾� ����
    [SerializeField] private float sunriseHour; // �ذ� �ߴ� �ð� (24�ð� ����)
    [SerializeField] private float sunsetHour; // �ذ� ���� �ð� (24�ð� ����)

    [Header("Ambient Light")]
    [SerializeField] private Color dayAmbientLight; // �� ������ �ֺ� �� ����
    [SerializeField] private Color nightAmbientLight; // �� ������ �ֺ� �� ����
    [SerializeField] private AnimationCurve lightChangeCurve; // �� ���⸦ �ε巴�� ��ȯ�ϴ� �
    [SerializeField] private float maxSunLightIntensity; // �¾� ������ �ִ� ����
    [SerializeField] private Light moonLight; // ���� �ִ� �� ����
    [SerializeField] private float maxMoonLightIntensity; // �� ������ �ִ� ����

    private DateTime currentTime; // ���� ���� �� �ð�
    private TimeSpan sunriseTime; // �� �ߴ� �ð��� TimeSpan ��
    private TimeSpan sunsetTime; // �� ���� �ð��� TimeSpan ��

    // Start�� ù ������ ������Ʈ ���� ȣ��˴ϴ�.
    void Start()
    {
        // ���� �ð��� ���� �ð����� �ʱ�ȭ�մϴ�.
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        // �� �ߴ� �ð��� �� ���� �ð��� �ʱ�ȭ�մϴ�.
        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update�� �� �����Ӹ��� ȣ��˴ϴ�.
    void Update()
    {
        // ���� �� �ð��� ������Ʈ�մϴ�.
        UpdateTimeOfDay();
        // ���� �ð��� ���� �¾��� ȸ����ŵ�ϴ�.
        RotateSun();
        // �� ����(�ֺ� ��, �¾� ��, �� ��)�� ������Ʈ�մϴ�.
        UpdateLightSettings();
    }

    // ���� ���� �� �ð��� ������Ʈ�մϴ�.
    private void UpdateTimeOfDay()
    {
        // ��Ÿ �ð��� �ð� ���ӵ��� ������� ���� �ð��� ������ŵ�ϴ�.
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        // �ؽ�Ʈ ������Ʈ�� null�� �ƴ� ��� UI �ؽ�Ʈ�� ���� �ð����� ������Ʈ�մϴ�.
        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    // ���� �ð��� ���� �¾��� ȸ����ŵ�ϴ�.
    void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            // �� �ߴ� �ð��� �� ���� �ð� ������ �Ⱓ�� ����մϴ�.
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            // �� �� ���� ����� �ð��� ����մϴ�.
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            // �� �� ���� ����� �ð��� ������ ����մϴ�.
            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            // ������ ���� �¾��� ȸ���� 0������ 180�� ���̿��� ���� �����մϴ�.
            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            // �� ���� �ð��� ���� �� �ߴ� �ð� ������ �Ⱓ�� ����մϴ�.
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            // �� �� ���� ����� �ð��� ����մϴ�.
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            // �� �� ���� ����� �ð��� ������ ����մϴ�.
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            // ������ ���� �¾��� ȸ���� 180������ 360�� ���̿��� ���� �����մϴ�.
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        // ���� ȸ���� �¾� ������ �����մϴ�.
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    // �¾��� ��ġ�� ���� �ֺ� ��, �¾� ��, �� �� ������ ������Ʈ�մϴ�.
    private void UpdateLightSettings()
    {
        // �¾��� ���� ����� �Ʒ��� ������ ������ ����մϴ�.
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        // ������ �� ��ȭ ��� ������� �¾� ������ ���⸦ �����մϴ�.
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        // ������ �� ��ȭ ��� ������� �� ������ ���⸦ �ݴ�� �����մϴ�.
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));

        // ������ �� ��ȭ ��� ������� �ֺ� �� ������ �����մϴ�.
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    // �� TimeSpan �� ������ �ð� ���̸� ����ϰ�, ���� ���̸� �����մϴ�.
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        // ���̰� ������ ��� 24�ð��� ���� �����մϴ�.
        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
