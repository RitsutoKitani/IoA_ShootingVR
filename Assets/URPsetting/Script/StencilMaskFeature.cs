using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StencilMaskFeature : ScriptableRendererFeature
{
    class StencilPass : ScriptableRenderPass
    {
        private Material maskMaterial;
        private RenderTargetIdentifier cameraColorTarget;

        public StencilPass(Material mat)
        {
            this.maskMaterial = mat;
            renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
        }

        public void Setup(RenderTargetIdentifier colorTarget)
        {
            cameraColorTarget = colorTarget;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (maskMaterial == null) return;

            CommandBuffer cmd = CommandBufferPool.Get("StencilMaskPass");
            cmd.SetRenderTarget(cameraColorTarget);
            cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, maskMaterial);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    public Material stencilMaskMaterial; // Inspector Ç©ÇÁê›íË

    StencilPass stencilPass;

    public override void Create()
    {
        stencilPass = new StencilPass(stencilMaskMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        stencilPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(stencilPass);
    }
}
