using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace DirectXCode
{
    public class DirectXCube : DirectXBase
    {
        public DirectXCube()
        {
            yaw = 0;
            pitch = 0;
            roll = 0;

            InitializeGraphics();

            // Create a mesh box
            box = Mesh.Box(device, 46.0f, 6.0f, 52.0f);

            box = box.Clone(box.Options.Value, CustomVertex.PositionNormalColored.Format, device);
            box.ComputeNormals();
   
            CustomVertex.PositionNormalColored[] vertices;

            Int32[] ranks = new Int32[1];
            ranks[0] = box.NumberVertices;

            vertices = (CustomVertex.PositionNormalColored[])box.LockVertexBuffer(typeof(CustomVertex.PositionNormalColored), LockFlags.None, ranks);

            for ( int i = 0; i < vertices.Length; i++ )
            {
                vertices[i].Color = Color.Brown.ToArgb();
            }

            GraphicsStream vertStream;
            vertStream = box.LockVertexBuffer(LockFlags.None);

            vertStream.Write(vertices);

            box.UnlockVertexBuffer();

            // Create the camera projection matrix
//            camProj = Matrix.PerspectiveFovRH((float)(3.14159 / 4), this.Width/this.Height, 1f, 1000f);
            camProj = Matrix.OrthoRH(this.Size.Width, this.Size.Height, -10000f, 10000f);
            
            device.SetTransform(TransformType.Projection, camProj);

            // Create the view matrix (translate the camera)
            cameraLook = new Vector3(0f, 0f, 0f);
            cameraPosition = new Vector3(0f, 0f, -4f);
            cameraUp = new Vector3(0f, 1f, 0f);

            camView = Matrix.LookAtRH(cameraPosition, cameraLook, cameraUp);
//            camView = Matrix.Identity;

            device.SetTransform(TransformType.View, camView);

            device.Lights[0].Type = LightType.Directional;
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Ambient = Color.Brown;
            device.Lights[0].Direction = new Vector3(0.5f, -.5f, -.5f);
            device.Lights[0].Range = 2000f;
            device.Lights[0].Enabled = true;

            Material cubeMaterial = new Material();
            cubeMaterial.Diffuse = cubeMaterial.Ambient = Color.Brown;
            device.Material = cubeMaterial;

            device.SetRenderState(RenderStates.Lighting, true);
            device.SetRenderState(RenderStates.AntialiasedLineEnable, true);
            device.SetRenderState(RenderStates.MultisampleAntiAlias, true);
        }

        protected override void Render()
        {
            Matrix R = Matrix.RotationYawPitchRoll(yaw, pitch, roll);

            Matrix worldView = Matrix.Multiply(R, camView);

            device.SetTransform(TransformType.World, R);
            
            box.DrawSubset(0);
        }

        float yaw, pitch, roll;
        Mesh box;
        Matrix camProj;
        Matrix camView;
        Vector3 cameraPosition;
        Vector3 cameraLook;
        Vector3 cameraUp;

        public float Yaw
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public float Pitch
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public float Roll
        {
            get { return roll; }
            set { roll = value; }
        }
    }
}
