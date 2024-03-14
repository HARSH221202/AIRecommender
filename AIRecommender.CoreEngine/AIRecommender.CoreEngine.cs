using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRecommender.CoreEngine
{
    public interface IRecommender
    {
        double Getcorrelation(int[] basedata, int[] otherdata);
    }

    public class PearsonRecommender : IRecommender
    {
        public double Getcorrelation(int[] bases, int[] other)
        {
            int[] basedata = new int[bases.Length];
            int[] otherdata = new int[other.Length];
            if (basedata == null || otherdata == null || basedata.Length == 0 || otherdata.Length == 0)
            {
                throw new Inputnull("Input arrays cannot be null or empty.");
            }
            if (basedata.Length != otherdata.Length)
            {
                if(basedata.Length > otherdata.Length)
                {
                    int diff = basedata.Length- otherdata.Length;
                    int[] ints = new int[basedata.Length];
                    for(int i =  0; i < otherdata.Length; i++)
                    {
                        ints[i] = otherdata[i];
                    }
                    for(int i = otherdata.Length;i< basedata.Length; i++)
                    {
                        ints[i] = 1;
                        basedata[i] += 1;
                    }
                    otherdata = ints;
                }
                else
                {
                    int diff =  basedata.Length;
                    int[] ints = new int[(int)diff];
                    Array.Copy(otherdata, 0, ints, 0, diff);
                    otherdata = ints;
                }
            }
            for (int i = 0; i < basedata.Length; i++)
            {
                if (basedata[i] == 0 || otherdata[i]==0)
                {
                    basedata[i] += 1;
                    otherdata[i] += 1;
                }
            }

            int n = basedata.Length;

            double meanBase = CalculateMean(basedata);
            double meanOther = CalculateMean(otherdata);

            double numerator = 0;
            double denomBase = 0;
            double denomOther = 0;

            for (int i = 0; i < n; i++)
            {
                double diffBase = basedata[i] - meanBase;
                double diffOther = otherdata[i] - meanOther;

                numerator += diffBase * diffOther;
                denomBase += diffBase * diffBase;
                denomOther += diffOther * diffOther;
            }
            if(denomBase == 0 || denomOther == 0)
            {
                return 0;
            }
            double correlation = numerator / (Math.Sqrt(denomBase) * Math.Sqrt(denomOther));

            return correlation;
        }

        private double CalculateMean(int[] data)
        {
            int n = data.Length;
            double sum = 0;

            foreach (var value in data)
            {
                sum += value;
            }

            return sum / n;
        }
    }
}
