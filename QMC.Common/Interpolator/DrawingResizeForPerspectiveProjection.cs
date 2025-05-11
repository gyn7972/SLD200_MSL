using QMC.Common.Vision.VisionAlign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Interpolator
{
    public class DrawingResizeForPerspectiveProjection
    {
        XyCoordinate m_SouceleftTop;
        XyCoordinate m_SourcerightBottom;
        XyCoordinate m_DestleftTop;
        XyCoordinate m_DestrightBottom;
        PerspectiveProjection m_PerspectiveProjection = new PerspectiveProjection();
        QMCMatrix CorrectionMatrix;

        
        public void MakeCorrectionMatrix(XyCoordinate sourceLeftTop, XyCoordinate sourceRightBottom, XyCoordinate destLeftTop, XyCoordinate destRightBottom)
        {
            
            m_SouceleftTop = sourceLeftTop;
            m_SourcerightBottom = sourceRightBottom;
            m_DestleftTop = destLeftTop;
            m_DestrightBottom = destRightBottom;

            XyCoordinateCollection coordinatesSource = new XyCoordinateCollection();
            XyCoordinateCollection coordinatesTarget = new XyCoordinateCollection();
            coordinatesSource.Add(m_SouceleftTop);
            coordinatesSource.Add(m_SourcerightBottom);
            coordinatesSource.Add(new XyCoordinate(m_SouceleftTop.X, m_SourcerightBottom.Y));
            coordinatesSource.Add(new XyCoordinate(m_SourcerightBottom.X, m_SouceleftTop.Y));

            coordinatesTarget.Add(m_DestleftTop);
            coordinatesTarget.Add(m_DestrightBottom);
            coordinatesTarget.Add(new XyCoordinate(m_DestleftTop.X, m_DestrightBottom.Y));
            coordinatesTarget.Add(new XyCoordinate(m_DestrightBottom.X, m_DestleftTop.Y));
            m_PerspectiveProjection = new PerspectiveProjection();
            CorrectionMatrix = m_PerspectiveProjection.GetCorrectionMatrix(coordinatesSource, coordinatesTarget);
        }
        public PointD[] Resize(PointD[] source)
        {
            if (CorrectionMatrix == null)
            {
                throw new InvalidOperationException("Correction matrix is not initialized.");
            }
            List<XyCoordinate> dest = new List<XyCoordinate>();
            List<PointD> Result = new List<PointD>();
            foreach (XyCoordinate point in source)
            {   
                XyCoordinate resizedPoint = m_PerspectiveProjection.GetPerspectiveProjectionPoint(point,CorrectionMatrix);
                Result.Add(new PointD(resizedPoint.X, resizedPoint.Y));
            }
             
            
            return Result.ToArray();
        }

        private XyCoordinate Resize(XyCoordinate point)
        {
            throw new NotImplementedException();
        }
    }
}
