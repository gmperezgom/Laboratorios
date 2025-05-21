using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proyecto2_ParteB_Gerber_Pérez_1165825
{
    public class Jugador
    {
        public string Nombre { get; set; }
        public char[,] Flota { get; set; }
        public char[,] Ataque { get; set; }
        public int Puntaje { get; set; }
        public Barco[] FlotaBarcos { get; set; }
        private static readonly char Agua = '~';
        private static readonly char BarcoMarca = '■';

        //Constructor para el jugador
        public Jugador(string nombre)
        {
            Nombre = nombre;
            Flota = new char[6, 6];
            Ataque = new char[6, 6];
            FlotaBarcos = new Barco[3];
            InicializarTableros();
        }

        private void InicializarTableros()
        {
            //Se inicializa el tablero por medio de la matriz de 6x6
            for (int fila = 0; fila < 6; fila++)
                for (int columna = 0; columna < 6; columna++)
                {
                    //Los valores iniciales de cada uno de los tableros es de agua
                    Flota[fila, columna] = Agua;
                    Ataque[fila, columna] = Agua;
                }
        }

        public void DibujarTablero(int turnoJugador)
        {
            // Se muestra la información del jugador
            Console.WriteLine($"Jugador: {Nombre}   Turno: {turnoJugador}   Puntaje: {Puntaje}");
            Console.WriteLine("Flota Naval:\tTablero de Ataque:");
            Console.WriteLine("  1 2 3 4 5 6\t  1 2 3 4 5 6");

            // Recorremos cada fila (A–F)
            for (int fila = 0; fila < 6; fila++)
            {
                char letraTablero = (char)('A' + fila);
                // Se coloca la flota naval
                Console.Write(letraTablero + " ");
                for (int col = 0; col < 6; col++)
                {
                    // Agua o parte de barco
                    if (Flota[fila, col] == Agua)
                    {
                        // Agua
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(Agua + " ");
                        Console.ResetColor();
                    }
                    else
                    {
                        // Se busca si esa coordenada fue impactada
                        bool impactoBarco = false;
                        foreach (var barco in FlotaBarcos)
                        {
                            foreach (var coord in barco.Coordenadas)
                            {
                                if (coord.Fila == fila && coord.Columna == col && coord.Impactado)
                                {
                                    impactoBarco = true;
                                    break;
                                }
                            }
                            if (impactoBarco) {
                                // Pintamos el barco: rojo si impacto, azul si no
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                        }
                        Console.Write(BarcoMarca + " ");
                        Console.ResetColor();
                    }
                }

                // Tablero de Ataque
                Console.Write("\t" + letraTablero + " ");
                for (int columna = 0; columna < 6; columna++)
                {
                    char casilla = Ataque[fila, columna];
                    if (casilla == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (casilla == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    Console.Write(casilla + " ");
                    Console.ResetColor();
                }

                Console.WriteLine();
            }
        }
        public void GenerarFlotaAleatoria(Random rnd)
        {
            //Se reinician ambos tableros
            InicializarTableros();

            //Datos de los barcos en distintos arreglos
            string[] nombres = { "Submarino", "Fragata", "Destructor" };
            int[] tamaños = { 2, 3, 4 };
            int[] puntos = { 2, 3, 4 };
            bool[] soloVerticalB = { false, true, false };
            bool[] soloHorizontalB = { true, false, false };

            //Recorre cada tipo de barco por su indice
            for (int indice = 0; indice < nombres.Length; indice++)
            {
                //Asignación de variables correspondientes de cada arreglo
                string nombre = nombres[indice];
                int tamaño = tamaños[indice];
                int pts = puntos[indice];
                bool soloVertical = soloVerticalB[indice];
                bool soloHorizontal = soloHorizontalB[indice];


                //Indicación si el barco fue colocado con éxito
                bool colocadoBarco = false;
                //Intento de posiciones para la colocacion del barco
                while (!colocadoBarco)
                {
                    //Variable para determinar orientación: vertical u horizontal
                    bool vertical;
                    if (soloVertical)
                    {
                        //Si soloVertical es verdadero, se orienta de manera vertical
                        vertical = true;
                    }
                    else if (soloHorizontal)
                    {
                        vertical = false;
                    }
                    else
                    {
                        //Random para colocarlo de manera horizontal o vertical
                        int valor = rnd.Next(2);
                        vertical = valor == 0;
                    }

                    //Calculo de fila inicial segun orientación y tamaño de cada barco
                    int filaInicial;
                    if (vertical)
                    {
                        //Al ser vertical, deja un espacio hacia abajo
                        filaInicial = rnd.Next(0, 6 - tamaño + 1);
                    }
                    else
                    {
                        //Si es horizontal, puede ser cualquier valor desde 0 hasta 5
                        filaInicial = rnd.Next(0, 6);
                    }
                    //Calcula columna inicial segun tamaño y orientación
                    int columnaInicial;
                    if (vertical)
                    {
                        //Si es vertical puede tomar cualquier valor desde 0 hasta 5
                        columnaInicial = rnd.Next(0, 6);
                    }
                    else
                    {
                        //Si es horizontal, deja un espacio hacia la derecha 
                        columnaInicial = rnd.Next(0, 6 - tamaño + 1);
                    }
                    //Se comprueban que todas las casillas esten libres
                    bool libre = true;
                    for (int i = 0; i < tamaño; i++)
                    {
                        int filaFinal;
                        int columnaFinal;
                        //Se calcula la posicion concreta del barco
                        if (vertical)
                        {
                            filaFinal = filaInicial + i;
                            columnaFinal = columnaInicial;
                        }
                        else
                        {
                            filaFinal = filaInicial;
                            columnaFinal = columnaInicial + i;
                        }
                        //Si la ya casilla contiene otro valor que no es agua, se marca como ocupado
                        if (Flota[filaFinal, columnaFinal] != Agua)
                        {
                            libre = false;
                            break;
                        }
                        //Comprobacion de las casillas vacias para que no se toquen barcos entre si
                        for (int j = -1; j <= 1 && libre; j++)
                        {
                            for (int k = -1; k <= 1; k++)
                            {
                                int filaVecina = filaFinal + j;
                                int columnaVecina = columnaFinal + k;
                                if (filaVecina >= 0 && filaVecina < 6 && columnaVecina >= 0 && columnaVecina < 6)
                                {
                                    if (Flota[filaVecina, columnaVecina] != Agua)
                                    {
                                        libre = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    //Sino esta libre, vuelve al inicio del while para probar otra posicion
                    if (!libre)
                    {
                        continue;
                    }
                    //Creacion de un objeto barco con las propiedades
                    var barco = new Barco(Nombre, tamaño, pts);
                    //Colocacion del barco en el tablero y registra sus coordenadas
                    for (int i = 0; i < tamaño; i++)
                    {
                        int filaFinal;
                        int columnaFinal;
                        if (vertical)
                        {
                            filaFinal = filaInicial + i;
                            columnaFinal = columnaInicial;
                        }
                        else
                        {
                            filaFinal = filaInicial;
                            columnaFinal = columnaInicial + i;
                        }
                        //Marca la casilla con un cuadro
                        Flota[filaFinal, columnaFinal] = BarcoMarca;
                        //Guarda la coordenada en el objeto para futuros ataques
                        barco.AgregarCoordenadas(i, filaFinal, columnaFinal);
                    }
                    //Se asigna el valor del barco al arreglo de la flota del jugador
                    FlotaBarcos[indice] = barco;
                    //Se marca que el barco ya fue colocado, saliendo del ciclo.
                    colocadoBarco = true;
                }
            }
        }
        public Coordenada LanzarMisil()
        {
            var regex = new Regex(@"^([A-F])\-([1-6])$");

            while (true)
            {
                // Se ingresa la coordenada para realizar el ataque
                Console.Write("Ingresa una coordenada para realizar tu ataque (Por Ejemplo. B-3): ");
                string coordenada = Console.ReadLine().ToUpper().Trim();

                // Intentamos hacer match y capturar fila (letra) y columna (número)
                var capturaCoordenada = regex.Match(coordenada);
                if (capturaCoordenada.Success)
                {
                    // match.Groups[1] es la letra, match.Groups[2] es el dígito 1–6
                    int fila = capturaCoordenada.Groups[1].Value[0] - 'A';
                    int numero = int.Parse(capturaCoordenada.Groups[2].Value);
                    int columna = numero - 1;

                    // Se verifica si dichas casillas están marcadas como agua 
                    if (Ataque[fila, columna] == Agua)
                    {
                        // Se retorna una nueva coordenada con una nueva fila y columna
                        return new Coordenada(fila, columna);
                    }
                    Console.WriteLine("Ya has atacado a esta posición.");
                }
                else
                {
                    // Si no cumple con la condicion, se muestra un mensaje de error
                    Console.WriteLine("Coordenada inválida para realizar dicho ataque.");
                }
            }
        }
        public bool RecibirMisil(Coordenada posicion)
        {
            //Ciclo para recorrer cada uno de los barcos de la flota
            foreach (var barco in FlotaBarcos)
            {
                //Ciclo para recorrer cada una de las coordenadas
                foreach (var coord in barco.Coordenadas)
                {
                    //Se verifica si la condicion es válida dependiendo de las posiciones
                    if (coord.Fila == posicion.Fila && coord.Columna == posicion.Columna)
                    {
                        //Se coloca un O en el tablero de la flota y se registra el impacto
                        Flota[posicion.Fila, posicion.Columna] = 'O';
                        coord.Impactado = true;
                        barco.RegistrarImpacto();
                        return true;
                    }
                }
            }
            return false;
        }

        public void ApuntarResultado(Coordenada posicion, bool acierto)
        {
            //Si es un acierto
            if (acierto)
            {
                //Se coloca en el tablero de ataque un circulo, si se determina que es un acierto
                Ataque[posicion.Fila, posicion.Columna] = 'O';
            }
            else
            {
                //Caso contrario se coloca una X
                Ataque[posicion.Fila, posicion.Columna] = 'X';
            }
        }

        public bool VerificarVictoria()
        {
            foreach (var barco in FlotaBarcos)
            {
                //Si es distinta a la validación de hundimiento de los barcos
                if (!barco.Hundido())
                {
                    //Retorna un valor falso
                    return false;
                }
            }
            //Retorna valor verdadero si los barcos estan hundidos
            return true;
        }

        public int VerificarBarcoHundido(Coordenada posicion)
        {
            //Para cada barco en la flota de barcos
            foreach (var barco in FlotaBarcos)
            {
                //Si hay un barco hundido
                if (barco.Hundido())
                {
                    //Recorre cada una de las coordenadas 
                    foreach (var coord in barco.Coordenadas)
                    {
                        //Si coinciden esos datos con las coordenadas del barco
                        if (coord.Fila == posicion.Fila && coord.Columna == posicion.Columna)
                        {
                            //Retorna los puntos del barco
                            return barco.Puntos;
                        }
                    }
                }
            }
            //Retorna el valor 0 si no se encontró el punteo
            return 0;
        }
    }
}