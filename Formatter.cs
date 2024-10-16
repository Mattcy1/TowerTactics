using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerTactics
{
        public static class NumberFormatter
        {
            public static string FormatNumber(double number)
            {
                if (number >= 1e3 && number < 1e6)
                {
                    return $"{number / 1e3:F1}k";
                }
                else if (number >= 1e6 && number < 1e9)
                {
                    return $"{number / 1e6:F1}M";
                }
                else if (number >= 1e9 && number < 1e12)
                {
                    return $"{number / 1e9:F1}B";
                }
                else if (number >= 1e12)
                {
                    return $"{number / 1e12:F1}T";
                }
                else
                {
                    return $"{number:F0}";
                }
            }
        }
    }
