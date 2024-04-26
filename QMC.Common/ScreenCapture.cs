using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class ImgCapture
    {
        private int _refX = 0;
        private int _refY = 0;
        private int _imgW = 0;
        private int _imgH = 0;

        private string filePath = null;

        public ImgCapture(int refX = 0, int refY = 0, int imgW = 0, int imgH = 0)
        {
            _refX = refX;
            _refY = refY;
            _imgW = imgW;
            _imgH = imgH;
        }

        public void SetPath(string path)
        {
            filePath = path;
        }

        public void DoCaptureImage()
        {
            if (filePath != null)
            {
                if (_imgW == 0 || _imgH == 0)
                    return;

                using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap((int)_imgW, (int)_imgH))
                {
                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(_refX, _refY, 0, 0, bitmap.Size);
                    }

                    bitmap.Save(filePath, ImageFormat.Png);
                }
            }
        }

        //캡처 함수
        public void ScreenCapture(string filename, int w, int h, Point pt)
        {
            Bitmap bitmap = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(pt, new Point(0, 0), new Size(w, h));
            }

            //저장 경로
            string path = filePath;

            //폴더 생성
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists == false)
            {
                di.Create();
            }

            //파일 이름
            //path += "\\" + DateTime.Now.ToString("yyMMdd_HHmmssfff") + ".png";
            path += "\\" + filename + ".png";

            bitmap.Save(path);
        }
    }
}
