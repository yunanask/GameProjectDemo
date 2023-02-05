using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/RayMarchingFog")]
public sealed class RayMarchingFog : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    public ClampedFloatParameter accuracy = new ClampedFloatParameter(0f, 0f, 0.5f);
    public ClampedIntParameter steps = new ClampedIntParameter(32, 16, 256);
    public ClampedFloatParameter thickness = new ClampedFloatParameter(100f, 0f, 1000f);
    public ClampedFloatParameter density = new ClampedFloatParameter(0.1f, 0f, 1f);
    public ClampedFloatParameter distance = new ClampedFloatParameter(0f, 0f, 200f);
    public FloatParameter frontBoundary = new FloatParameter(0);
    public FloatParameter backBoundary = new FloatParameter(0);
    public FloatParameter leftBoundary = new FloatParameter(0);
    public FloatParameter rightBoundary = new FloatParameter(0);

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    // Do not forget to add this post process in the Custom Post Process Orders list (Project Settings > Graphics > HDRP Settings).
    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "TA Shaders/RayMarchingFog";

    public override void Setup()
    {
        if (Shader.Find(kShaderName) != null)
            m_Material = new Material(Shader.Find(kShaderName));
        else
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume RayMarchingFog is unable to load.");
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);

        m_Material.SetMatrix("_InvViewM", Camera.main.cameraToWorldMatrix);
        m_Material.SetMatrix("_InvProjectionM", Camera.main.projectionMatrix.inverse);

        m_Material.SetFloat("_Accuracy", accuracy.value);
        m_Material.SetFloat("_Steps", steps.value);
        m_Material.SetFloat("_FogThickness", thickness.value);
        m_Material.SetFloat("_FogDensity", density.value);
        m_Material.SetFloat("_FogDist", distance.value);
        m_Material.SetInt("_Steps", steps.value);

        m_Material.SetFloat("_MapF", frontBoundary.value);
        m_Material.SetFloat("_MapB", backBoundary.value);
        m_Material.SetFloat("_MapR", rightBoundary.value);
        m_Material.SetFloat("_MapL", leftBoundary.value);

        cmd.Blit(source, destination, m_Material, -1);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
