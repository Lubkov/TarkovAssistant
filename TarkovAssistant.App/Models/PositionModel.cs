using CommunityToolkit.Mvvm.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TarkovAssistant.App.Models
{
    public partial class PositionModel : ObservableObject
    {
        [ObservableProperty]
        private double _left;

        [ObservableProperty]
        private double _top;

        [ObservableProperty]
        private double _angle;

        public double OriginLeft { get; private set; }

        public double OriginTop { get; private set; }

        [ObservableProperty]
        private bool _isVisibile = false;

        public PositionModel(double top, double left, double angle)
        {
            Top = left;
            Left = top;
            Angle = angle;
            OriginTop = top;
            OriginLeft = left;
            IsVisibile = false;            
        }

        public PositionModel() : this (0, 0, 0) { }

        public static PositionModel? Parse(string filename)
        {
            // "2024-07-05[13-26]_-13.2, 19.9, 203.3_0.0, -1.0, 0.1, 0.3_16.07 (0).png"
            //  2025-09-05[18-49]_18.21, 2.64, 49.42_0.00018, 0.99980, -0.01870, 0.00650_15.47 (0).png
            // ]_ ищем начало блока после подчёркивания.
            // (-?\d +\.\d +) — первое число(с возможным минусом и дробной частью). 
            // ,\s * — запятая м возможные пробелы.
            // -?\d +\.\d + — второе число (пропускаем).
            // ,\s * — снова запятая и пробелы.
            // (-?\d +\.\d +) — третье число, которое мы хотим вытащить.
            // _ — конец блока координат.
            //string pattern = @"]_(-?\d+\.\d+),\s*-?\d+\.\d+,\s*(-?\d+\.\d+)_-?\d+\.\d+,\s*(-?\d+\.\d+)";

            try
            {
                string[] parts = filename.Split('_');

                // Координаты
                string[] pos = parts[1].Split(',');
                float posX = float.Parse(pos[0], CultureInfo.InvariantCulture);
                float posY = float.Parse(pos[1], CultureInfo.InvariantCulture);
                float posZ = float.Parse(pos[2], CultureInfo.InvariantCulture);

                // Кватернион
                string[] quat = parts[2].Split(',');
                float qx = float.Parse(quat[0], CultureInfo.InvariantCulture);
                float qy = float.Parse(quat[1], CultureInfo.InvariantCulture);
                float qz = float.Parse(quat[2], CultureInfo.InvariantCulture);
                float qw = float.Parse(quat[3], CultureInfo.InvariantCulture);

                double angle;
                double siny = 2 * (qw * qy - qz * qx);
                if (Math.Abs(siny) >= 1)
                    angle = (Math.CopySign(Math.PI / 2, siny));
                else
                    angle = Math.Asin(siny);

                angle *= (180.0 / Math.PI);
                if (angle < 0)
                {
                    angle = Math.Abs(angle);
                }
                else
                {
                    angle += 180;
                }

                return new PositionModel(posZ, posX, angle);
            }
            catch
            {
                return null;
            }            
        }
    }
}
