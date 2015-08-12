﻿// ---------------------------------------------------------------------------------------
// <copyright file="Reactive.cs" company="Corale">
//     Copyright © 2015 by Adam Hellberg and Brandon Scott.
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy of
//     this software and associated documentation files (the "Software"), to deal in
//     the Software without restriction, including without limitation the rights to
//     use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//     of the Software, and to permit persons to whom the Software is furnished to do
//     so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in all
//     copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//     WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//     CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//     Disclaimer: Corale and/or Colore is in no way affiliated with Razer and/or any
//     of its employees and/or licensors. Corale, Adam Hellberg, and/or Brandon Scott
//     do not take responsibility for any harm caused, direct or indirect, to any
//     Razer peripherals via the use of Colore.
//
//     "Razer" is a trademark of Razer USA Ltd.
// </copyright>
// ---------------------------------------------------------------------------------------

namespace Corale.Colore.Razer.Keyboard.Effects
{
    using System.Runtime.InteropServices;

    using Corale.Colore.Annotations;
    using Corale.Colore.Core;

    /// <summary>
    /// Describes the reactive effect type.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Reactive
    {
        /// <summary>
        /// Color of the effect.
        /// </summary>
        [PublicAPI]
        public Color Color;

        /// <summary>
        /// The duration of the effect.
        /// </summary>
        [PublicAPI]
        public Duration Duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Reactive" /> struct.
        /// </summary>
        /// <param name="color">Color to apply when key is hit.</param>
        /// <param name="duration">Duration to illuminate the key.</param>
        public Reactive(Color color, Duration duration)
        {
            Color = color;
            Duration = duration;
        }
    }
}
