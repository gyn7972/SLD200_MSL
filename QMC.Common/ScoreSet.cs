using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class ScoreSet
    {
        #region Field
        private double m_Score;
        private double m_Position;
        #endregion

        #region Constructor
        public ScoreSet(double score, double position)
        {
            this.Score = score;
            this.Position = position;
        }
        #endregion

        #region Property
        public double Score
        {
            get { return this.m_Score; }
            set { this.m_Score = value; }
        }

        public double Position
        {
            get { return this.m_Position; }
            set { this.m_Position = value; }
        }
        #endregion
    }

    public class ScoreCollection : Collection<ScoreSet>
    {
        #region Method
        public int GetMaxScoreIndex()
        {
            int index = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[index].Score < this[i].Score)
                    index = i;
            }
            return index;
        }

        public double GetMaxScore()
        {
            double score = 0;
            for (int i = 0; i < this.Count; i++)
            {
                if (score < this[i].Score)
                    score = this[i].Score;
            }
            return score;
        }

        public double GetMinScore()
        {
            double score = 100.0;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Score < score)
                    score = this[i].Score;
            }
            return score;
        }
        #endregion
    }
}
