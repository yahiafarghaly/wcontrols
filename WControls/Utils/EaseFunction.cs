using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WControls.Utils
{
    public enum EaseFunctionType
    {
        Linear,
        QuadraticInOut,
        SineInOut,
        CubicInOut
    }

    public class EaseFunction
    {
        private EaseFunctionType m_type = EaseFunctionType.Linear;
        private double m_dLength = 1000;//in ms
        private double m_dFrom = 0;
        private double m_dTo = 0;
        private Func<double, double> m_function;

        public double ToValue
        {
            get { return m_dTo; }
        }

        public EaseFunction(EaseFunctionType type, double lengthMs, double fromValue, double toValue)
        {
            m_type = type;
            m_dLength = lengthMs;
            m_dFrom = fromValue;
            m_dTo = toValue;
            m_function = GetEasingFunction(m_type);
        }

        public double GetValue(double curMs)
        {
            if (curMs > m_dLength)
            {
                throw new ArgumentOutOfRangeException("curMs");
            }

            //compute what percentage (0 - 1) we are through the function
            double percentEase = curMs / m_dLength;
            double percentValue = m_function(percentEase);

            //be sure to return a value within from/to range
            double dValue = m_dFrom + ((m_dTo - m_dFrom) * percentValue);
            if (m_dFrom <= m_dTo)
            {
                dValue = Math.Min(dValue, m_dTo);
            }
            else
            {
                dValue = Math.Max(dValue, m_dTo);
            }

            return dValue;
        }

        /// <summary>
        /// Return a function that maps the percentage of the easing function
        /// to the percentage between the begin and end values
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Func<double, double> GetEasingFunction(EaseFunctionType type)
        {
            Func<double, double> func = null;

            switch (type)
            {
                case EaseFunctionType.Linear:
                    func = d => d;
                    break;
                case EaseFunctionType.QuadraticInOut:
                    func = GetPowerEase(2);
                    break;
                case EaseFunctionType.CubicInOut:
                    func = GetPowerEase(3);
                    break;
                case EaseFunctionType.SineInOut:
                    func = d => (Math.Sin((d - .5) * Math.PI) + 1) / 2d;
                    break;
                default:
                    throw new NotImplementedException("Easing function not supported");
            }

            return func;
        }

        private static Func<double, double> GetPowerEase(int power)
        {
            return d =>
            {
                double dReturn = 0;

                if (d <= .5)
                {
                    dReturn = Math.Pow((d * 2), power) / 2d;
                }
                else
                {
                    double opposite = Math.Pow(((1 - d) * 2), power) / 2d;
                    dReturn = 1 - opposite;
                }

                return dReturn;
            };
        }
    }
}
