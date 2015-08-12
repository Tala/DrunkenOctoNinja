﻿// ---------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Corale">
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

namespace Corale.Colore.Razer.Keyboard
{
    using Corale.Colore.Annotations;

    /// <summary>
    /// Holds various keyboard constants used in Razer's API.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The maximum number of rows on the keyboard
        /// </summary>
        [PublicAPI]
        public const uint MaxRows = 6;

        /// <summary>
        /// The maximum number of columns on the keyboard
        /// </summary>
        [PublicAPI]
        public const uint MaxColumns = 22;

        /// <summary>
        /// The maximum number of keys on the keyboard
        /// </summary>
        [PublicAPI]
        public const uint MaxKeys = MaxRows * MaxColumns;

        /// <summary>
        /// The maximum number of custom effects based on the maximum keys
        /// </summary>
        [PublicAPI]
        public const uint MaxCustomEffects = MaxKeys;
    }
}
