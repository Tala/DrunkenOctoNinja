using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corale.Colore.Core;

namespace ChromaGestureMagic
{
    class KinectChromaLogic
    {
        // Hand hoch - runter Bewegen -> Farbe ändern
        Color leftHandColor1 = Color.Red;
        Color leftHandColor2 = Color.Yellow;
        Color rightHandColor1 = Color.Blue;
        Color rightHandColor2 = Color.Green;


        
        // Faust öffnen -> Ripple Effect
        // Hand rechts - links Bewegen -> Areal verschieben 

        // Body und Handtracking analyse
        // Pro Event: Y, X und Z sowie Handstate checken und derzeitigen Zustand updaten
        // Bei Update -> Änderung an Chroma notifien? oder selbst neu setzen

    }
}
