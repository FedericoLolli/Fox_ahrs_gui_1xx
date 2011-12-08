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
    public abstract partial class DirectXBase : UserControl
    {
        protected Device device = null;
        protected bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }
        public virtual void InitializeGraphics()
        {
            PresentParameters pp = new PresentParameters();
            pp.Windowed = true;
            pp.SwapEffect = SwapEffect.Discard;
            pp.EnableAutoDepthStencil = true;
            pp.AutoDepthStencilFormat = DepthFormat.D16;
            pp.DeviceWindowHandle = this.Handle;
            device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, pp);
            device.DeviceReset += new EventHandler(this.OnDeviceReset);
            OnDeviceReset(device, null);

            initialized = true;
        }

        public virtual void OnDeviceReset(object sender, EventArgs e)
        {
            device = sender as Device;
            //derived types can override this to add functionality
        }

        protected abstract void Render();
        protected override void OnPaint(PaintEventArgs e)
        {
            if (device == null)
            {
                //draw black background if no device
                Graphics graphics = e.Graphics;
                graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, Width, Height));
                return;
            }
            else
            {
                //clear the device to black
                device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Gray, 1.0f, 0);
                device.BeginScene();
                //call abstract Render which derived types will define
                Render();
                device.EndScene();
                device.Present();
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do nothing to eliminate flicker
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            Invalidate();
        }

    }
}
