using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatCatcher
{
    public static class SelectedGlyph
    {
        public static byte[,] glyph;
        public static List<byte[,]> glyphs = new List<byte[,]> {
            new byte[,] {{ 0, 0, 0, 0, 0},
                                          { 0, 1, 1, 0, 0},
                                          { 0, 0, 1, 1, 0},
                                          { 0, 1, 1, 0, 0},
                                          { 0, 0, 0, 0, 0} },

            new byte[,] {{ 0, 0, 0, 0, 0},
                                          { 0, 0, 1, 0, 0},
                                          { 0, 1, 0, 0, 0},
                                          { 0, 0, 1, 1, 0},
                                          { 0, 0, 0, 0, 0} },

            new byte[,] {{ 0, 0, 0, 0, 0},
                                          { 0, 0, 1, 0, 0},
                                          { 0, 0, 1, 0, 0},
                                          { 0, 1, 1, 1, 0},
                                          { 0, 0, 0, 0, 0} },

            new byte[,] {{ 0, 0, 0, 0, 0},
                                          { 0, 1, 0, 0, 0},
                                          { 0, 0, 1, 0, 0},
                                          { 0, 1, 0, 1, 0},
                                          { 0, 0, 0, 0, 0} } };
    }
}
