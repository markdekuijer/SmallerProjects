using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory 
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings settings)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.ridged:
                return new RidgedNoiseFilter(settings);
            case NoiseSettings.FilterType.simple:
                return new SimpleNoiseFilter(settings);
        }

        return null;
    }


}
