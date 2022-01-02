using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignTexture : MonoBehaviour
{
    public ComputeShader shader;
    public int texResolution  = 256;
    public string KernelName  = "SolidRed";
    Renderer rend;
    RenderTexture outputTexture;
    int kernelHandle;

    private void Start()
    {
        outputTexture = new RenderTexture(texResolution, texResolution, 0);
        outputTexture.enableRandomWrite = true;
        outputTexture.Create();

        rend = GetComponent<Renderer>();
        rend.enabled = true;

        InitShader();
    }

    private void InitShader()
    {
        kernelHandle = shader.FindKernel(KernelName);

        int halfRes = texResolution >> 1;
        int quarterRes = texResolution >> 2;

        Vector4 rect = new Vector4( quarterRes, quarterRes, halfRes, halfRes);

        shader.SetVector("rect",rect);

        shader.SetInt("texResolution", texResolution);
        shader.SetTexture(kernelHandle, "Result", outputTexture);
        rend.material.SetTexture("_MainTex",outputTexture);

        DispatchShader(texResolution /8, texResolution /8);
    }

    private void DispatchShader(int x, int y)
    {
        shader.Dispatch(kernelHandle, x, y, 1);
    }

    private void Update()
    {

    }

}
