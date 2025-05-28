using SpiralLab.Sirius;
using System;
using System.Numerics;

namespace QMC.Common.Parts
{
    public class SpialData
    {
        private double m_dOuterDiameter;
        private double m_dInnerDiameter;
        private double m_dRevolutions;
        private double m_dAngleFactor;

        LwPolyline lwPolyLineVertices;

        public SpialData(double outerDiameter, double innerDiameter, double revolutions, double angleFactor)
        {
            m_dOuterDiameter = outerDiameter;
            m_dInnerDiameter = innerDiameter;
            m_dRevolutions = revolutions;
            m_dAngleFactor = angleFactor;
            //SetSpiralData(this.m_dOuterDiameter, this.m_dInnerDiameter, this.m_dRevolutions, this.m_dAngleFactor);
        }

        public void SetSpiralData(double outerDiameter, double innerDiameter, double revolutions, double angleFactor)
        {
            m_dOuterDiameter = outerDiameter;
            m_dInnerDiameter = innerDiameter;
            m_dRevolutions = revolutions;
            m_dAngleFactor = angleFactor;
            //lwPolyLineVertices = SpiralData_Create(m_dOuterDiameter, m_dInnerDiameter, m_dRevolutions, m_dAngleFactor, 0, 0);
        }

        public double GetOuterDiameter() => m_dOuterDiameter;
        public double GetInnerDiameter() => m_dInnerDiameter;
        public double GetRevolutions() => m_dRevolutions;
        public double GetAngleFactor() => m_dAngleFactor;
        public LwPolyline GetLwPolyLineVertices()
        {
            if(lwPolyLineVertices == null || lwPolyLineVertices.Count == 0)
            {
                lwPolyLineVertices = SpiralData_Create(m_dOuterDiameter, m_dInnerDiameter, m_dRevolutions, m_dAngleFactor, 0, 0);
            }
            return (LwPolyline)lwPolyLineVertices.Clone();
        }

        // == 연산자 재정의
        public static bool operator ==(SpialData left, SpialData right)
        {
            
            if(right is null)
            {
                return false;
            }
            if(left is null)
            {
                return false;
            }
            return left.m_dOuterDiameter == right.m_dOuterDiameter &&
                   left.m_dInnerDiameter == right.m_dInnerDiameter &&
                   left.m_dRevolutions == right.m_dRevolutions &&
                   left.m_dAngleFactor == right.m_dAngleFactor;
        }

        // != 연산자 재정의
        public static bool operator !=(SpialData left, SpialData right)
        {
            return !(left == right);
        }

        // Equals 메서드 재정의
        public override bool Equals(object obj)
        {
            if (obj is SpialData other)
            {
                return this == other;
            }
            return false;
        }
        public LwPolyline SpiralData_Create(double m_dOuterDiameter, double m_dInnerDiameter, double m_nRevolutions, double m_nAngleFactor, double m_dHoleCenter_X, double m_dHoleCenter_Y)
        {

            var entity = new LwPolyline();

            double radialPitch = (m_dOuterDiameter - m_dInnerDiameter) / 2.0 / (double)m_nRevolutions;
            double x = m_dInnerDiameter / 2.0;
            double y = 0;
            double angle = 0;
            double degInRad;
            double d;

            if (m_nAngleFactor <= 0)
            {
                m_nAngleFactor = 10;                        //  렌더링 최소 각도값이 0 이하일 경우, default로 10을준다.
            }

            for (int i = 0; i < m_nRevolutions; i++)
            {
                for (double t = 0; t < 360; t += /*SpiralLab.Sirius.Config.AngleFactor*/m_nAngleFactor)
                {
                    angle = t + 360.0 * (double)i;
                    degInRad = angle * MathHelper.DegToRad;
                    d = m_dInnerDiameter / 2.0 + radialPitch * (double)i + radialPitch * t / 360.0;
                    x = d * Math.Cos(degInRad);
                    y = d * Math.Sin(degInRad);

                    entity.Add(new LwPolyLineVertex((float)x, (float)y, 0));
                }
            }

            if (true)              //  닫힌 도형처럼 해야할듯? -> 이거 안하면 외곽 동그라미가 안됨
            {
                for (double t = 0; t < 360; t += /*SpiralLab.Sirius.Config.AngleFactor*/m_nAngleFactor)
                {
                    angle = t;
                    degInRad = angle * MathHelper.DegToRad;
                    d = m_dOuterDiameter / 2.0;
                    x = d * Math.Cos(degInRad);
                    y = d * Math.Sin(degInRad);
                    entity.Add(new LwPolyLineVertex((float)x, (float)y, 0));
                }
            }

            entity.Add(new LwPolyLineVertex((float)(m_dOuterDiameter / 2.0), 0, 0));
            //entity.Owner = this;
            entity.Regen();
            entity.Rotate((float)angle);
            entity.Transit(new Vector2((float)m_dHoleCenter_X, (float)m_dHoleCenter_Y));

            return entity;
        }
        // GetHashCode 재정의

    }
}
