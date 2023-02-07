using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] public Light Sun;
    [SerializeField] public Light Moon;

    [SerializeField] public Volume GlobalVolume;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 2400)] private float TimeOfDay;

    [SerializeField, Range(0, 300000)] private float BaseValue;

    [SerializeField, Range(0, 1)] public float IsNight;
    private float timeValue;
    [SerializeField, Range(0, 100)] private float DayPercent;
    [SerializeField, Range(0, 100)]private float NightPercent;

    [SerializeField, Range(0, 100)]private float SpeedFactor;
    public AnimationCurve curve;    

    Fog fog;   
    Fog fogset;  
   //  void Start()
   // {


      //  if (GlobalVolume.profile.TryGet<Fog>(out fogset))
      //  {
      //      fog = fogset;
      //  }
  //  }

    private void Update()
    {
        if (GlobalVolume.profile.TryGet<Fog>(out fogset))
        {
            fog = fogset;
        }
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += Time.deltaTime *SpeedFactor;
            TimeOfDay %= 2400; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }


    private void UpdateLighting(float timePercent)
    {   
        //GlobalVolume.profile.TryGet( test, fog);
        if(timePercent < 50){
            timeValue = timePercent;
            DayPercent = (timePercent -25)*2;
            NightPercent = 50- 2* timePercent;
        } else{
            timeValue = 100-timePercent;
            DayPercent = (75-timePercent )*2;
            NightPercent = -150+  timePercent*2;
        }
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (Sun != null)
        {
            CheckNightDayTransition();
            Sun.color = Preset.DirectionalColor.Evaluate(timePercent);
            
            if (TimeOfDay < 1200) {
                    Sun.transform.localRotation = Quaternion.Euler(new Vector3((DayPercent), ((timePercent/100) * 120f) - 60f, 0));
                   // Sun.intensity = timeValue*timeValue *200 + BaseValue;
                   // Sun.colorTemperature = 13000 - (3.6f*timeValue*timeValue);
                    Moon.transform.localRotation = Quaternion.Euler(new Vector3((NightPercent), ((timePercent/100) * 120f) - 30f, 0));
            }
            else{
                Sun.transform.localRotation = Quaternion.Euler(new Vector3((DayPercent), ((timePercent/100) * 120f) - 60f, 0));
                Moon.transform.localRotation = Quaternion.Euler(new Vector3((NightPercent), ((timePercent/100) * 120f) - 150f, 0));
               // Sun.intensity = timeValue*timeValue *200 + BaseValue;
               // Sun.colorTemperature = 13000 - (3.6f*timeValue*timeValue);
            }
            
        }

        

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (Sun != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            Sun = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    Sun = light;
                    return;
                }
            }
        }
    }

    private void CheckNightDayTransition(){
        if (IsNight==1) {
            if (TimeOfDay > 600 && TimeOfDay < 1850){
                StartDay();
            }
        }
        else{
            if (TimeOfDay < 600 || TimeOfDay >= 1850){
                StartNight();
            }
        }
    }

    private void StartDay() {
        IsNight = 0;
        Sun.shadows = LightShadows.Hard;
       // Sun.intensity = 300000;
        //Sun.Enable = false;
         RenderSettings.fogDensity = 0.2f;
         RenderSettings.fogStartDistance = 25f;
        Moon.shadows = LightShadows.None;
        RenderSettings.fog = false;
        fog.meanFreePath.value = 70;
        
       // Moon.intensity = 0;
    }

    private void StartNight() {
        IsNight = 1;
        Sun.shadows = LightShadows.None;
        RenderSettings.fogDensity = 0.8f;
        RenderSettings.fogStartDistance = -25f;
        RenderSettings.fog = true;
        //Sun.intensity = 0;
        //Sun.Enable = false;
        Moon.shadows = LightShadows.Hard;
        fog.meanFreePath.value = 30;
        
       // Moon.intensity = 300000;
       //GlobalVolume.fog.meanFreePath.value = 20;
}
}
