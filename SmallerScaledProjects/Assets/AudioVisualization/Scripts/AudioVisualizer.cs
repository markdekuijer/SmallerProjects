using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    public AudioClip audioClip;
    public bool useMic;
    public int microphoneIndex;
    public string selectedDevice;

    private AudioSource audio;
    [HideInInspector] public float[] samplesLeft = new float[512];
    [HideInInspector] public float[] samplesRight = new float[512];

    private float[] freqBand = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    private float[] freqBandHighest = new float[8];
    //audio64
    private float[] freqBand64 = new float[64];
    private float[] bandBuffer64 = new float[64];
    private float[] bufferDecrease64 = new float[64];
    private float[] freqBandHighest64 = new float[64];

    [HideInInspector] public float[] audioBand;
    [HideInInspector] public float[] audioBandBuffer;

    [HideInInspector] public float[] audioBand64;
    [HideInInspector] public float[] audioBandBuffer64;

    [HideInInspector] public float Amplitude, AmplitudeBuffer;
    private float AmplitudeHighest;
    public float audioProfile;

    public enum Channel
    {
        sterio,
        left,
        right
    };
    public Channel channel;

    void Start()
    {
        print(Microphone.devices.Length);
        Time.timeScale = 0.2f;
        audioBand = new float[8];
        audioBandBuffer = new float[8];

        audioBand64 = new float[64];
        audioBandBuffer64 = new float[64];

        audio = GetComponent<AudioSource>();
        AudioProfile(audioProfile);

        if (useMic)
        {
            if (Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[microphoneIndex].ToString();
                audio.clip = Microphone.Start(Microphone.devices[microphoneIndex].ToString(), true, 10, 44100);
                print("started successful");

                if (Microphone.IsRecording(selectedDevice))
                {
                    while (!(Microphone.GetPosition(selectedDevice) > 0))
                    {

                    }
                    print("working");
                    audio.Play();
                }
                else
                    useMic = false;
            }
            else
                useMic = false;
        }
        if(!useMic)
        {
            audio.time = 50;
            audio.clip = audioClip;
            audio.Play();
        }

    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeFrequencyBands64();
        BandBuffer();
        BandBuffer64();
        CreateAudioBands();
        CreateAudioBands64();
        GetAmplitude();
    }

    void AudioProfile(float _audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = _audioProfile;
        }
        for (int i = 0; i < 64; i++)
        {
            freqBandHighest64[i] = _audioProfile;
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if(currentAmplitude > AmplitudeHighest)
            AmplitudeHighest = currentAmplitude;

        Amplitude = currentAmplitude / AmplitudeHighest;
        AmplitudeBuffer = currentAmplitudeBuffer / AmplitudeHighest;
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
                freqBandHighest[i] = freqBand[i];

            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void CreateAudioBands64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > freqBandHighest64[i])
                freqBandHighest64[i] = freqBand64[i];

            audioBand64[i] = (freqBand64[i] / freqBandHighest64[i]);
            audioBandBuffer64[i] = (bandBuffer64[i] / freqBandHighest64[i]);
        }
    }

    void GetSpectrumAudioSource()
    {
        audio.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        audio.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);
    }

    void BandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if(freqBand[i] > bandBuffer[i])
            {
                bandBuffer[i] = freqBand[i];
                bufferDecrease[i] = 0.025f;
            }
            if (freqBand64[i] < bandBuffer[i])
            {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void BandBuffer64()
    {
        for (int i = 0; i < 64; i++)
        {
            if (freqBand64[i] > bandBuffer64[i])
            {
                bandBuffer64[i] = freqBand64[i];
                bufferDecrease64[i] = 0.075f;
            }
            if (freqBand64[i] < bandBuffer64[i])
            {
                bandBuffer64[i] -= bufferDecrease64[i];
                bufferDecrease64[i] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if(i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                if(channel == Channel.sterio)
                    average += (samplesLeft[count] + samplesRight[count]) * (count +1);
                else if (channel == Channel.left)
                    average += samplesLeft[count] * (count + 1);
                else if (channel == Channel.right)
                    average += samplesRight[count] * (count + 1);

                count++;
            }

            average /= count;
            freqBand[i] = average * 10;
        }
    }

    void MakeFrequencyBands64()
    {
        int count = 0;
        int sampleCount = 1;
        int power = 0;

        for (int i = 0; i < 64; i++)
        {
            float average = 0;

            if (i == 16 ||i == 32 || i == 40 || i == 48 || i == 56)
            {
                power++;
                sampleCount = (int)Mathf.Pow(2, power);
                if (power == 3)
                    sampleCount -= 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == Channel.sterio)
                    average += (samplesLeft[count] + samplesRight[count]) * (count + 1);
                else if (channel == Channel.left)
                    average += samplesLeft[count] * (count + 1);
                else if (channel == Channel.right)
                    average += samplesRight[count] * (count + 1);

                count++;
            }

            average /= count;
            freqBand64[i] = average * 80;
        }
    }
}
