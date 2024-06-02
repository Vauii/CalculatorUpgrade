namespace Calculator
{
    public class QuantityCounter
    {
        public int Distance { get; set; }
        public int Time { get; set; }
        public int Speed { get; set; }
        public double Scalar { get; set; } = 0.0;
     

        public ValueType? GetSingleType()
        {
            if (Distance == 1 && Time == 0 && Speed == 0) return ValueType.Distance;
            if (Distance == 0 && Time == 1 && Speed == 0) return ValueType.Time;
            if (Distance == 0 && Time == 0 && Speed == 1) return ValueType.Speed;
            if (Distance == 0 && Time == 0 && Speed == 0) return ValueType.Scalar;
            return null;
        }
        
        public QuantityCounter Simplify()
        {
            while (Speed > 0 && Time > 0)
            {
                Speed -= 1;
                Time -= 1;
                Distance += 1;
            }
            while (Speed < 0 && Distance > 0)
            {
                Speed += 1;
                Time += 1;
                Distance -= 1;
            }
            return this;

        }

        public static QuantityCounter operator *(QuantityCounter a, QuantityCounter b)
        {
            return new QuantityCounter
            {
                Distance = a.Distance + b.Distance,
                Time = a.Time + b.Time,
                Speed = a.Speed + b.Speed,
                Scalar = a.Scalar * b.Scalar
            };
        }

        public static QuantityCounter operator /(QuantityCounter a, QuantityCounter b)
        {
            return new QuantityCounter
            {
                Distance = a.Distance - b.Distance,
                Time = a.Time - b.Time,
                Speed = a.Speed - b.Speed,
                Scalar = a.Scalar / b.Scalar
            };
        }

        public static QuantityCounter? operator +(QuantityCounter a, QuantityCounter b)
        {
            if (a.Distance == b.Distance && a.Time == b.Time && a.Speed == b.Speed)
            {
                return new QuantityCounter
                {
                    Distance = a.Distance,
                    Time = a.Time,
                    Speed = a.Speed,
                    Scalar = a.Scalar + b.Scalar
                };
            }
            return null;

        }

        public static QuantityCounter? operator -(QuantityCounter a, QuantityCounter b)
        {
            if (a.Distance == b.Distance && a.Time == b.Time && a.Speed == b.Speed)
            {
                return new QuantityCounter
                {
                    Distance = a.Distance,
                    Time = a.Time,
                    Speed = a.Speed,
                    Scalar = a.Scalar - b.Scalar
                };
            }
            return null;

        }


    }
}
