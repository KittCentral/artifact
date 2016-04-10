using UnityEngine;
using UnityEngine.Rendering;

public class Blur
{
    RenderTexture highlightRender;
    RenderTargetIdentifier renderID;
    CommandBuffer renderBuffer;
    GameObject blurred;

    void CreateBuffers ()
    {
        highlightRender = new RenderTexture(Screen.width, Screen.height, 0);
        renderID = new RenderTargetIdentifier(highlightRender);
        renderBuffer = new CommandBuffer();
    }

    void RenderHighlights()
    {
        renderBuffer.SetRenderTarget(renderID);
        Renderer renderer = blurred.GetComponent<Renderer>();
        RenderTexture.active = highlightRender;
        Graphics.ExecuteCommandBuffer(renderBuffer);
        RenderTexture.active = null;
    }
}
