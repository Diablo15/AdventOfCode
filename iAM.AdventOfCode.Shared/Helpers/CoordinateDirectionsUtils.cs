using iAM.AdventOfCode.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023.Models
{

    public static class CoordinateDirectionsUtils
    {      
        public static Pipe TranslateCharToCoordinate(this char input, int inputX, int inputY)
        {
            switch (input)
            {
                case '|':
                    return new(inputX, inputY - 1, inputX, inputY + 1);
                case '-':
                    return new(inputX + 1, inputY, inputX - 1, inputY);
                case 'L':
                    return new(inputX, inputY - 1, inputX + 1, inputY);
                case 'J':
                    return new(inputX, inputY - 1, inputX - 1, inputY);
                case '7':
                    return new(inputX, inputY + 1, inputX - 1, inputY);
                case 'F':
                    return new(inputX, inputY + 1, inputX + 1, inputY);
                case '.':
                    return new(888, 888, 999, 999);
                case 'S':
                    return new(inputX, inputY, inputX, inputY);
                default:
                    throw new NotImplementedException();
            }
        }
        public static Pipe TranslateNegativesToPeriods(this Pipe input)
        {
            var output = input;
            if (output.fromX < 0 || output.fromY < 0)
            {
                output = new Pipe(888,888,999,999);
            }

            if (output.toX < 0 || output.toY < 0)
            {
                output = new Pipe(888, 888, 999, 999);
            }
            return output;
        }
    }
}
