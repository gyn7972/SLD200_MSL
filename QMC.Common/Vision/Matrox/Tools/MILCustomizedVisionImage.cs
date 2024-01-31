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
using System.Windows;

namespace QMC.Common.Vision.Matrox.Tools
{
    #region MILCustomizedVisionImage

    public class MILCustomizedVisionImage : ICustomizedVisionImage
    {
        #region Field
        private bool m_Disposed;
        private object m_Image;
        private Rect? m_Region;
        #endregion

        #region Constructor
        public MILCustomizedVisionImage()
        {
        }
        #endregion

        #region Finalizer
        ~MILCustomizedVisionImage()
        {
            this.Dispose();
        }
        #endregion

        #region Property
        public Rect? Region
        {
            get { return this.m_Region; }
            set { this.m_Region = value; }
        }
        #endregion

        #region Method
        public static int Create(ref VisionImage source)
        {
            int ret = 0;
            MILCustomizedVisionImage customized = null;
            MIL_ID milSystem = MSystem.Instance.MilSystem;
            MImageBuffer m_buffer2D = null;

            if (source.RawData == null || source.Header.Width == 0 || source.Header.Height == 0)
            {
                throw new Exception("Vision image is null.");
            }

            switch (source.Header.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    throw new NotImplementedException();

                case PixelFormat.Format4bppIndexed:
                    throw new NotImplementedException();

                case PixelFormat.Format8bppIndexed:

                    Bitmap bmp = source.GetImage() as Bitmap;

                    if (m_buffer2D != null)
                        m_buffer2D.Free();
                    m_buffer2D = new MImageBuffer(milSystem, bmp);

                    customized = new MILCustomizedVisionImage();

                    source.CustomizedData = customized;
                    source.CustomizedData.Image = m_buffer2D.BufferID;

                    break;

                case PixelFormat.Format16bppGrayScale:

                    //opencvImage = new Mat(new OpenCvSharp.Size(source.Header.Width, source.Header.Height), MatType.CV_16UC1);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)source.GetImage());

                    //customized = new OpenCVCustomizedVisionImage();

                    //source.CustomizedData = customized;
                    //source.CustomizedData.Image = opencvImage;
                    break;

                case PixelFormat.Format16bppRgb555:
                    break;

                case PixelFormat.Format16bppRgb565:
                    break;

                case PixelFormat.Format16bppArgb1555:
                    break;

                case PixelFormat.Format24bppRgb:
                    //opencvImage = new Mat(new OpenCvSharp.Size(source.Header.Width, source.Header.Height), MatType.CV_8UC3);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)source.GetImage());

                    //customized = new OpenCVCustomizedVisionImage();

                    //source.CustomizedData = customized;
                    //source.CustomizedData.Image = opencvImage;
                    break;

                case PixelFormat.Format32bppRgb:
                    //opencvImage = new Mat(new OpenCvSharp.Size(source.Header.Width, source.Header.Height), MatType.CV_32SC3);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)source.GetImage());

                    //customized = new OpenCVCustomizedVisionImage();

                    //source.CustomizedData = customized;
                    //source.CustomizedData.Image = opencvImage;
                    break;

                case PixelFormat.Format32bppArgb:
                    throw new NotImplementedException();
                    break;

                case PixelFormat.Format32bppPArgb:
                    throw new NotImplementedException();

                case PixelFormat.Format48bppRgb:
                    throw new NotImplementedException();

                case PixelFormat.Format64bppArgb:
                    throw new NotImplementedException();

                default:
                    throw new Exception("Invalid Vision Image");
            }

            return ret;
        }
        #endregion

        #region ICustomizedVisionImage Method
        public object Image
        {
            get { return this.m_Image; }
            set { this.m_Image = value; }
        }

        public bool Disposed
        {
            get { return this.m_Disposed; }
            private set { this.m_Disposed = value; }
        }

        public Image GetImage()
        {
            Bitmap bmp = null;

            if (this.Image == null)
            {
                throw new Exception("Customized image is null.");
            }

            if (this.Image is MIL_ID)
            {
                MIL_ID image = (MIL_ID)this.Image;

                //TODO : GetImage() 완료하기.
                //bmp = image.ToBitmap();
                return bmp;
            }
            else
            {
                throw new Exception("Invalid OpenCV Image");
            }
            return null;
        }

        public void SetImage(Image image)
        {
            //Mat opencvImage = null;

            switch (image.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    throw new NotImplementedException();

                case PixelFormat.Format4bppIndexed:
                    throw new NotImplementedException();

                case PixelFormat.Format8bppIndexed:
                    Bitmap format8bppIndexedImage = (Bitmap)image;

                    //opencvImage = new Mat();
                    //opencvImage = BitmapConverter.ToMat(format8bppIndexedImage);

                    //this.Image = opencvImage;
                    break;

                case PixelFormat.Format16bppGrayScale:
                    //opencvImage = new Mat(new OpenCvSharp.Size(image.Width, image.Width), MatType.CV_16UC1);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)image);

                    //this.Image = opencvImage;
                    break;

                case PixelFormat.Format16bppRgb555:
                    break;

                case PixelFormat.Format16bppRgb565:
                    break;

                case PixelFormat.Format16bppArgb1555:
                    break;

                case PixelFormat.Format24bppRgb:
                    Bitmap format24bppRgbImage = (Bitmap)image;

                    //opencvImage = new Mat(new OpenCvSharp.Size(image.Width, image.Width), MatType.CV_8UC3);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)format24bppRgbImage);

                    //this.Image = opencvImage;

                    break;

                case PixelFormat.Format32bppRgb:
                    Bitmap format32bppRgbImage = (Bitmap)image;

                    //opencvImage = new Mat(new OpenCvSharp.Size(image.Width, image.Width), MatType.CV_8UC4);
                    //opencvImage = BitmapConverter.ToMat((Bitmap)format32bppRgbImage);

                    //this.Image = opencvImage;
                    break;

                case PixelFormat.Format32bppArgb:
                    throw new NotImplementedException();

                case PixelFormat.Format32bppPArgb:
                    throw new NotImplementedException();

                case PixelFormat.Format48bppRgb:
                    throw new NotImplementedException();

                case PixelFormat.Format64bppArgb:
                    throw new NotImplementedException();

                default:
                    throw new Exception("Invalid Vision Image");
            }
        }

        public VisionImage GetVisionImage()
        {
            VisionImage visionImage = new VisionImage();

            //if (this.Image is Mat)
            //{
            //    Mat image = this.Image as Mat;
            //    Bitmap bmpImage = null;
            //    BitmapData bmpData = null;

            //    float stride;
            //    bmpImage = BitmapConverter.ToBitmap(image);

            //    bmpData = bmpImage.LockBits(new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(bmpImage.Width, bmpImage.Height)),
            //            ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            //    stride = bmpData.Stride;

            //    visionImage.Header.Width = bmpData.Width;
            //    visionImage.Header.Height = bmpData.Height;

            //    visionImage.Header.PixelFormat = PixelFormat.Format8bppIndexed;
            //    visionImage.Header.Stride = bmpData.Stride;
            //    visionImage.Header.BufferSize = bmpData.Stride * bmpData.Height;
            //    visionImage.Header.BitsPerPixel = (int)Math.Round((8 * stride - 7) / bmpData.Width);

            //    if (visionImage.RawData == null)
            //    {
            //        visionImage.RawData = new byte[bmpData.Stride * bmpData.Height];
            //    }
            //    else if (visionImage.RawData.Length != visionImage.Header.BufferSize)
            //    {
            //        visionImage.RawData = null;
            //        visionImage.RawData = new byte[bmpData.Stride * bmpData.Height];
            //    }

            //    Marshal.Copy(bmpData.Scan0, visionImage.RawData, 0, visionImage.Header.BufferSize);

            //    bmpImage.UnlockBits(bmpData);

            //    return visionImage;
            //}
            //else
            //{
            //    throw new Exception("Invalid VisionPro Image");
            //}
            return visionImage;
        }

        public Image GetRotateImage(double angle, PointD point)
        {
            //if (this.Image == null) return null;

            //if (this.Image is Mat)
            //{
            //    Mat inputImage = this.Image as Mat;
            //    Mat outputImage = new Mat(inputImage);

            //    Mat rotationValue = Cv2.GetRotationMatrix2D(new Point2f((float)point.X, (float)point.Y), angle, 1.0);
            //    Cv2.WarpAffine(inputImage, outputImage, rotationValue, new OpenCvSharp.Size(inputImage.Width, inputImage.Height));

            //    Bitmap bitmap = null;

            //    //bitmap = outputImage.ToBitmap(PixelFormat.Format8bppIndexed);
            //    bitmap = outputImage.ToBitmap();

            //    return bitmap;
            //}

            return null;
        }

        public VisionImage GetRotateVisionImage(double angle, PointD point)
        {
            VisionImage visionImage = new VisionImage();

            //if (this.Image is Mat)
            //{
            //    Bitmap bitmap = null;
            //    BitmapData data = null;

            //    bitmap = (Bitmap)this.GetRotateImage(angle, point);
            //    float stride;

            //    data = bitmap.LockBits(
            //        new Rectangle(new System.Drawing.Point(0, 0), new System.Drawing.Size(bitmap.Width, bitmap.Height)),
            //        ImageLockMode.ReadOnly, bitmap.PixelFormat);
            //    stride = data.Stride;

            //    visionImage.Header.Width = data.Width;
            //    visionImage.Header.Height = data.Height;
            //    visionImage.Header.PixelFormat = data.PixelFormat;
            //    visionImage.Header.Stride = data.Stride;
            //    visionImage.Header.BufferSize = data.Stride * data.Height;
            //    visionImage.Header.BitsPerPixel = (int)Math.Round((8 * stride - 7) / data.Width);

            //    if (visionImage.RawData == null)
            //    {
            //        visionImage.RawData = new byte[data.Stride * data.Height];
            //    }
            //    else if (visionImage.RawData.Length != visionImage.Header.BufferSize)
            //    {
            //        visionImage.RawData = null;
            //        visionImage.RawData = new byte[data.Stride * data.Height];
            //    }

            //    Marshal.Copy(data.Scan0, visionImage.RawData, 0, visionImage.Header.BufferSize);

            //    bitmap.UnlockBits(data);

            //    return visionImage;
            //}
            //else
            //{
            //    throw new NotImplementedException();
            //}

            return null;
        }

        MImageBuffer m_buffer2D;

        public int Load(string fileName)
        {
            int ret = 0;
            string extension = "";
            this.Image = null;
            MIL_ID provider = MIL.M_NULL;
            MIL_ID MilSystem = MSystem.Instance.MilSystem;
            GC.Collect();

            if (fileName == string.Empty) return ret;

            extension = Path.GetExtension(fileName);
            extension = extension.Split('.')[1];

            if (extension == VisionImage.FileFilter.bmp.ToString()
                || extension == VisionImage.FileFilter.tif.ToString()
                || extension == VisionImage.FileFilter.jpg.ToString())
            {
                Bitmap bmp = new Bitmap(fileName);
                if (bmp != null)
                {
                    if (m_buffer2D != null)
                        m_buffer2D.Free();
                    m_buffer2D = new MImageBuffer(MilSystem, bmp);
                    MIL.MbufCopy(m_buffer2D.BufferID, provider);
                }
            }
            else
                throw new InvalidOperationException("Unsupported format");

            this.Image = provider;

            return ret;
        }

        public void Save(string fileName)
        {
            string extension = "";
            //Mat provider = new Mat();
            //ICogImageFileProvider provider = null;

            if (fileName == string.Empty) return;

            extension = Path.GetExtension(fileName);
            extension = extension.Split('.')[1];

            Bitmap bmp = new Bitmap(this.Image as Bitmap);

            if (extension == VisionImage.FileFilter.bmp.ToString())
            {
                bmp.Save(fileName, ImageFormat.Bmp);
            }
            else if (extension == VisionImage.FileFilter.tif.ToString())
            {
                bmp.Save(fileName, ImageFormat.Tiff);

            }
            else if (extension == VisionImage.FileFilter.jpg.ToString())
            {
                bmp.Save(fileName, ImageFormat.Jpeg);
            }
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(this);

            this.Disposed = true;
        }
        #endregion
    }
    #endregion
}
