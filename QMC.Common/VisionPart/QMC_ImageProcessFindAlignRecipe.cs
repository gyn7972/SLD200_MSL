namespace QMC.Common.VisionPart
{
    public class QMC_ImageProcessFindAlignRecipe
    {
        public int Radius { get; set; }
        public int Threshold { get; set; }

        public QMC_ImageProcessFindAlignRecipe(int radius, int threshold)
        {
            Radius = radius;
            Threshold = threshold;
        }
    }
}
