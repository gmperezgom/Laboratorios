using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_ParteB_Gerber_Pérez_1165825
{
    public class Barco
    {
        public string Nombre { get; set; }
        public int Tamaño { get; set; }
        public int Puntos { get; set; }
        public Coordenada[] Coordenadas { get; set; }
        private int impactos;

        //Constructor para la clase barco
        public Barco(string nombre, int tamaño, int puntos)
        {
            Nombre = nombre;
            Tamaño = tamaño;
            Puntos = puntos;
            Coordenadas = new Coordenada[tamaño];
            impactos = 0;
        }
        //Metodo para agregar las coordenadas
        public void AgregarCoordenadas(int seccion, int fila, int columna)
        {
            Coordenadas[seccion] = new Coordenada(fila, columna);
        }
        //Metodo para registrar los impactos 
        public void RegistrarImpacto()
        {
            impactos++;
        }
        //Metodo para registrar si un barco está hundido
        public bool Hundido()
        {
            return impactos == Tamaño;
        }
    }
}
