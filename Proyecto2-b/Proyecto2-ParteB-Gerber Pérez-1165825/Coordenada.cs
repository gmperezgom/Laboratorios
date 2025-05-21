using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_ParteB_Gerber_Pérez_1165825
{
    public class Coordenada
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool Impactado { get; set; }

        //Constructor para la clase coordenada
        public Coordenada(int fila, int columna)
        {
            Fila = fila;
            Columna = columna;
            Impactado = false;
        }
    }
}
