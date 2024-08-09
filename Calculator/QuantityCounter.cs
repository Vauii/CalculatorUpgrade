namespace Calculator
{
    public class QuantityCounter
    {
        private const double EPSILON = 1e-9;
        public int DistanceQuantNum { get; set; }
        public int TimeQuantNum { get; set; }
        public int SpeedQuantNum { get; set; }
        public double Scalar { get; set; } = 0.0;

        private static readonly Dictionary<(ValueType, ValueType), Action<QuantityCounter>> transformationRules = new()
        {
            { (ValueType.Speed, ValueType.Time), qc => { qc.SpeedQuantNum--; qc.TimeQuantNum--; qc.DistanceQuantNum++; } },
            { (ValueType.Distance, ValueType.Time), qc => { qc.DistanceQuantNum--; qc.TimeQuantNum++; qc.SpeedQuantNum++; } },
            { (ValueType.Distance, ValueType.Speed), qc => { qc.DistanceQuantNum--; qc.SpeedQuantNum++; qc.TimeQuantNum++; } }
        };

        public ValueType? GetSingleType()
        {
            return (DistanceQuantNum, TimeQuantNum, SpeedQuantNum) switch
            {
                (1, 0, 0) => ValueType.Distance,
                (0, 1, 0) => ValueType.Time,
                (0, 0, 1) => ValueType.Speed,
                (0, 0, 0) => ValueType.Scalar,
                _ => null
            };
        }

        public QuantityCounter Simplify()
        {
            bool simplified;
            do
            {
                simplified = false;
                foreach (var rule in transformationRules)
                {
                    while (CheckRule(rule.Key))
                    {
                        rule.Value(this);
                        simplified = true;
                    }
                }
            } while (simplified);

            return this;
        }

        private bool CheckRule((ValueType, ValueType) ruleKey)
        {
            return ruleKey switch
            {
                (ValueType.Speed, ValueType.Time) => SpeedQuantNum > 0 && TimeQuantNum > 0,
                (ValueType.Distance, ValueType.Time) => DistanceQuantNum > 0 && TimeQuantNum < 0,
                (ValueType.Distance, ValueType.Speed) => DistanceQuantNum > 0 && SpeedQuantNum < 0,
                _ => false,
            };
        }

        public static QuantityCounter operator *(QuantityCounter a, QuantityCounter b)
        {
            return new QuantityCounter
            {
                DistanceQuantNum = a.DistanceQuantNum + b.DistanceQuantNum,
                TimeQuantNum = a.TimeQuantNum + b.TimeQuantNum,
                SpeedQuantNum = a.SpeedQuantNum + b.SpeedQuantNum,
                Scalar = a.Scalar * b.Scalar
            };
        }

        public static QuantityCounter operator /(QuantityCounter a, QuantityCounter b)
        {
            if (Math.Abs(b.Scalar) < EPSILON)
            {
                throw new Exception("Division by zero");
            }
            return new QuantityCounter
            {
                DistanceQuantNum = a.DistanceQuantNum - b.DistanceQuantNum,
                TimeQuantNum = a.TimeQuantNum - b.TimeQuantNum,
                SpeedQuantNum = a.SpeedQuantNum - b.SpeedQuantNum,
                Scalar = a.Scalar / b.Scalar
            };
        }

        public static QuantityCounter? operator +(QuantityCounter a, QuantityCounter b)
        {
            if (a.DistanceQuantNum == b.DistanceQuantNum &&
                a.TimeQuantNum == b.TimeQuantNum &&
                a.SpeedQuantNum == b.SpeedQuantNum)
            {
                return new QuantityCounter
                {
                    DistanceQuantNum = a.DistanceQuantNum,
                    TimeQuantNum = a.TimeQuantNum,
                    SpeedQuantNum = a.SpeedQuantNum,
                    Scalar = a.Scalar + b.Scalar
                };
            }
            return null;

        }

        public static QuantityCounter? operator -(QuantityCounter a, QuantityCounter b)
        {
            if (a.DistanceQuantNum == b.DistanceQuantNum && 
                a.TimeQuantNum == b.TimeQuantNum &&
                a.SpeedQuantNum == b.SpeedQuantNum)
            {
                return new QuantityCounter
                {
                    DistanceQuantNum = a.DistanceQuantNum,
                    TimeQuantNum = a.TimeQuantNum,
                    SpeedQuantNum = a.SpeedQuantNum,
                    Scalar = a.Scalar - b.Scalar
                };
            }
            return null;

        }

        public static QuantityCounter ToQuantityCounter(ValueType type, double value)
        {
            var counter = new QuantityCounter();
            switch (type)
            {
                case ValueType.Distance:
                    counter.DistanceQuantNum = 1;
                    counter.Scalar = 1;
                    break;
                case ValueType.Time:
                    counter.TimeQuantNum = 1;
                    counter.Scalar = 1;
                    break;
                case ValueType.Speed:
                    counter.SpeedQuantNum = 1;
                    counter.Scalar = 1;
                    break;
                case ValueType.Scalar:
                    counter.Scalar = value;
                    break;
            }
            return counter;
        }


    }
}
