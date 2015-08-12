using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Corale.Colore.Core;
using Corale.Colore.Razer.Keyboard;
using Corale.Colore.Razer.Keyboard.Effects;

namespace SimpleChromaApp
{
    internal class Program
    {
        private static Timer timer;
        private static uint keyIndex;
        private static Color color;
        private static readonly uint maxRows = Constants.MaxRows;
        private static readonly uint maxCols = Constants.MaxColumns;

        private static void Main(string[] args)
        {
            color = Color.Orange;
            Chroma.Instance.Keyboard.Set(Color.HotPink);
            keyIndex = 1;
            timer = new Timer(100);
            timer.Elapsed += timer_Elapsed;
            timer.Start();

            Task.WaitAll(Task.Delay(TimeSpan.FromSeconds(20)));
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (keyIndex >= Constants.MaxColumns)
            {
                keyIndex = 1;
                color = Color.Black;
            }
            SetKeysAround(2, keyIndex);
            //Chroma.Instance.Keyboard.Set(Color.HotPink);
            keyIndex++;
        }

        private static void SetKeysAround(uint rowStart, uint colStart)
        {
            var colors = new Color[6][];
            var colorsColumns = new Color[22];
            for (var i = 0; i < colorsColumns.Length; i++)
            {
                colorsColumns[i] = Color.Black;
            }
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = (Color[]) colorsColumns.Clone();
            }


            var indexList = new List<Tuple<int, int>>
            {
                Tuple.Create(-1, -1),
                Tuple.Create(-1, 0),
                Tuple.Create(-1, +1),
                Tuple.Create(0, -1),
                Tuple.Create(0, 0),
                Tuple.Create(0, +1),
                Tuple.Create(+1, -1),
                Tuple.Create(+1, 0),
                Tuple.Create(+1, +1)
            };

            foreach (var tuple in indexList)
            {
                if (((2 + tuple.Item1) > 0 && (2 + tuple.Item1) < maxRows)
                    && ((keyIndex + tuple.Item2) > 0 && (keyIndex + tuple.Item2) < maxCols))
                {
                    float brightnessModifier = (tuple.Item1 == 0 && tuple.Item2 == 0) ? 0f : 0.85f;
                    colors[2 + tuple.Item1][keyIndex + tuple.Item2] = new Color((1f - brightnessModifier), 0f, 0f);
                }
            }

            Keyboard.Instance.Set(new CustomGrid(colors));
            if (keyIndex >= maxCols)
            {
                keyIndex = 0;
            }
            else
            {
                keyIndex++;
            }
        }
    }
}