using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.Matrox.Tools
{
    public class MImageBuffer
    {
        private MIL_ID _bufferId = MIL.M_NULL;
        private GCHandle _handle;

        public MIL_ID BufferID
        {
            get
            {
                return _bufferId;
            }
        }
        public MImageBuffer(MIL_ID systemId, Bitmap bitmap)
            : this(systemId, bitmap.Width, bitmap.Height, BitmapToBytes(bitmap))
        {
        }
        public MImageBuffer(MIL_ID systemId, int sizeX, int sizeY, byte[] imageData)
        {
            _handle = GCHandle.Alloc(imageData, GCHandleType.Pinned);
            ulong addressOfImageData = (ulong)_handle.AddrOfPinnedObject();
            //TODO : Image 받을때부터 4단위로 만들어주기.
            while (imageData.Length != sizeX * sizeY)
                sizeX++;
            MIL.MbufCreate2d(systemId, sizeX, sizeY, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_DISP + MIL.M_PROC, MIL.M_DEFAULT, sizeX, addressOfImageData, ref _bufferId);
        }
        public void Free()
        {
            if (_bufferId != MIL.M_NULL)
            {
                MIL.MbufFree(_bufferId);
                _handle.Free();
            }
        }

        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            BitmapData bmpdata = null;

            try
            {
                bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                int numbytes = bmpdata.Stride * bitmap.Height;
                byte[] bytedata = new byte[numbytes];
                IntPtr ptr = bmpdata.Scan0;

                Marshal.Copy(ptr, bytedata, 0, numbytes);

                return bytedata;
            }
            finally
            {
                if (bmpdata != null)
                    bitmap.UnlockBits(bmpdata);
            }
        }

    }
}
